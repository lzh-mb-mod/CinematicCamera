using CinematicCamera.Actions;
using CinematicCamera.FacialAnimation;
using CinematicCamera.MissionBehaviors;
using HarmonyLib;
using MissionLibrary.Provider;
using MissionLibrary.View;
using MissionSharedLibrary.Provider;
using MissionSharedLibrary.Utilities;
using MissionSharedLibrary.View.ViewModelCollection;
using MissionSharedLibrary.View.ViewModelCollection.Options;
using MissionSharedLibrary.View.ViewModelCollection.Options.Selection;
using SandBox;
using SandBox.Missions.AgentBehaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.Screens;

namespace CinematicCamera
{
    public enum AgentWatchState
    {
        Patrolling,
        Cautious,
        Alarmed,
        Count
    }

    public class CurrentAgentSelectionData
    {
        public SelectionOptionData SelectionOptionData;
        private readonly CinematicCameraLogic _logic = Mission.Current.GetMissionBehavior<CinematicCameraLogic>();
        private readonly OptionCategory _optionCategory;

        public CurrentAgentSelectionData(MissionScreen missionScreen, OptionCategory optionCategory)
        {
            var agents = GetAgentList();
            _optionCategory = optionCategory;
            SelectionOptionData = new SelectionOptionData(i =>
                {
                    var agents = GetAgentList();
                    if (Mission.Current?.Mode == MissionMode.Deployment)
                        return;
                    if (i >= 0 && i < agents.Count)
                    {
                        var previousAgent = CinematicCameraLogic.GetCurrentAgent();
                        var agentToSet = agents[i];
                        CinematicCameraLogic.SelectAgent(agents[i]);
                        if (previousAgent != agentToSet)
                        {
                            _optionCategory.UpdateAllOptions();
                        }
                    }
                },
                () => {
                    var agents = GetAgentList();
                    return agents.IndexOf(_logic.CurrentAgent);
                },
                () => {
                    var agents = GetAgentList();
                    return agents.Count;
                },
                () => {
                    var agents = GetAgentList();
                    return agents.Select(agent => new SelectionItem(false, agent.Name));
                });
        }
        private List<Agent> GetAgentList()
        {
            var agents = (Mission.Current.Agents).Where(agent => agent.IsHuman && agent.Character != null && agent.IsHero).ToList();
            var agent = _logic.CurrentAgent;
            if (agent != null && agents.IndexOf(agent) == -1)
            {
                agents.Add(agent);
            }
            return agents;
        }
    }


    public class CinematicCameraOptionClassFactory
    {
        public static IProvider<AOptionClass> CreateOptionClassProvider(AMenuClassCollection menuClassCollection)
        {
            return ProviderCreator.Create(() =>
            {
                var menuManager = AMenuManager.Get();
                var optionClass = new OptionClass(CinematicCameraSubModule.ModuleId,
                    GameTexts.FindText("str_cinematic_camera_cinematic_camera"), menuClassCollection);
                var cameraOptionCategory =
                    new OptionCategory("Camera", GameTexts.FindText("str_cinematic_camera_cinematic_camera"));

                cameraOptionCategory.AddOption(new ActionOptionViewModel(GameTexts.FindText("str_cinematic_camera_open_menu"), null,
                    () =>
                    {
                        menuManager.RequestToCloseMenu();
                        Mission.Current.GetMissionBehavior<CinematicCameraMenuView>()?.ActivateMenu();
                    }));
                cameraOptionCategory.AddOption(new BoolOptionViewModel(GameTexts.FindText("str_cinematic_camera_enable_giving_commands_in_regular_scene"),
                    GameTexts.FindText("str_cinematic_camera_enable_giving_commands_in_regular_scene_hint"),
                    () => CinematicCameraConfig.Get().OrderUIInRegularScene,
                    b => { CinematicCameraConfig.Get().OrderUIInRegularScene = b; }));
                var currentAgentOption = new SelectionOptionViewModel(
                    GameTexts.FindText("str_cinematic_camera_current_agent"),
                    GameTexts.FindText("str_cinematic_camera_current_agent_hint"),
                    new CurrentAgentSelectionData(Utility.GetMissionScreen(), cameraOptionCategory).SelectionOptionData, true);
                cameraOptionCategory.AddOption(currentAgentOption);
                cameraOptionCategory.AddOption(new ActionOptionViewModel(GameTexts.FindText("str_cinematic_camera_set_current_agent"),
                    GameTexts.FindText("str_cinematic_camera_set_current_agent_hint"),
                    () =>
                    {
                        var logic = Mission.Current.GetMissionBehavior<CinematicCameraLogic>();
                        if (logic != null)
                        {
                            var missionScreen = Utility.GetMissionScreen();
                            var agentToFollow = missionScreen.LastFollowedAgent;
                            var previousAgent = CinematicCameraLogic.GetCurrentAgent();
                            CinematicCameraLogic.SelectAgent(agentToFollow);
                            if (previousAgent != agentToFollow)
                            {
                                cameraOptionCategory.UpdateAllOptions();
                            }
                            else
                            {
                                currentAgentOption.UpdateData(false);
                            }
                        }
                    }));
                cameraOptionCategory.AddOption(new SelectionOptionViewModel(
                    GameTexts.FindText("str_cinematic_camera_agent_watch_state_option"),
                    GameTexts.FindText("str_cinematic_camera_agent_watch_state_option_hint"),
                    new SelectionOptionData(
                    i =>
                    {
                        var currentAgent = CinematicCameraLogic.GetCurrentAgent();
                        if (currentAgent == null)
                            return;

                        if (i < 0 || i >= (int)AgentWatchState.Count)
                            return;

                        var agentWatchState = (AgentWatchState)i;
                        currentAgent.SetWatchState((Agent.WatchState)agentWatchState);
                    },
                    () =>
                    {
                        var currentAgent = CinematicCameraLogic.GetCurrentAgent();
                        if (currentAgent == null)
                            return -1;

                        var watchState = currentAgent.CurrentWatchState;
                        return (int)watchState;
                    },
                    () =>
                    {
                        return (int)AgentWatchState.Count;
                    },
                    () =>
                    {
                        return new List<SelectionItem>()
                        {
                            new SelectionItem(true, "str_cinematic_camera_agent_watch_state", nameof(AgentWatchState.Patrolling)),
                            new SelectionItem(true, "str_cinematic_camera_agent_watch_state" ,nameof(AgentWatchState.Cautious)),
                            new SelectionItem(true, "str_cinematic_camera_agent_watch_state" ,nameof(AgentWatchState.Alarmed))
                        };
                    }), true, false));

                cameraOptionCategory.AddOption(new SelectionOptionViewModel(
                    GameTexts.FindText("str_cinematic_camera_current_behavior"),
                    GameTexts.FindText("str_cinematic_camera_current_behavior_hint"),
                    new SelectionOptionData(
                        i => {
                            var activeBehaviorGroup = GetActiveBehaviorGroup();
                            if (activeBehaviorGroup == null)
                                return;

                            var indexInBehaviors = i - 1;
                            if (indexInBehaviors < 0 || indexInBehaviors >= activeBehaviorGroup.Behaviors.Count)
                            {
                                activeBehaviorGroup.DisableScriptedBehavior();
                                return;
                            }
                            var behaviorToSet = activeBehaviorGroup.Behaviors[indexInBehaviors];
                            // referencing AgentBehaviorGroup.SetScriptedBehavior
                            var scriptedBehavior = AccessTools.Property(typeof(AgentBehaviorGroup), nameof(AgentBehaviorGroup.ScriptedBehavior));
                            scriptedBehavior.SetValue(activeBehaviorGroup, behaviorToSet);
                            activeBehaviorGroup.ForceThink(0.0f);
                            foreach (AgentBehavior behavior in activeBehaviorGroup.Behaviors)
                            {
                                if (behavior != activeBehaviorGroup.ScriptedBehavior)
                                    behavior.IsActive = false;
                            }
                        },
                        () =>
                        {
                            var activeBehaviorGroup = GetActiveBehaviorGroup();
                            if (activeBehaviorGroup == null || activeBehaviorGroup.ScriptedBehavior == null)
                            {
                                return 0;
                            }
                            return activeBehaviorGroup.Behaviors.IndexOf(activeBehaviorGroup.ScriptedBehavior) + 1;
                        },
                        () =>
                        {
                            var activeBehaviorGroup = GetActiveBehaviorGroup();
                            if (activeBehaviorGroup == null)
                                return 0;
                            return activeBehaviorGroup.Behaviors.Count + 1;
                        },
                        () =>
                        {
                            var result = new List<SelectionItem>()
                            {
                                new SelectionItem(false, "default")
                            };
                            var activeBehaviorGroup = GetActiveBehaviorGroup();
                            if (activeBehaviorGroup == null)
                            {
                                return result;
                            }

                            var activeBehavior = activeBehaviorGroup.GetActiveBehavior();
                            if (activeBehavior != null && activeBehaviorGroup.ScriptedBehavior == null)
                            {
                                result[0] = new SelectionItem(false, "default: " + activeBehavior.GetType().Name);
                            }
                            result.AddRange(activeBehaviorGroup.Behaviors.Select(behavior => new SelectionItem(false, behavior.GetType().Name)));
                            return result;
                        }), true));

                cameraOptionCategory.AddOption(new BoolOptionViewModel(
                    GameTexts.FindText("str_cinematic_camera_crouch_mode"),
                    GameTexts.FindText("str_cinematic_camera_crouch_mode_hint"),
                    () =>
                    {
                        var currentAgent = CinematicCameraLogic.GetCurrentAgent();
                        if (currentAgent == null)
                            return false;
                        return (currentAgent.GetScriptedFlags() & Agent.AIScriptedFrameFlags.Crouch) != 0;
                    },
                    b =>
                    {
                        var currentAgent = CinematicCameraLogic.GetCurrentAgent();
                        if (currentAgent == null)
                            return;
                        if (b)
                        {
                            currentAgent.SetScriptedFlags(currentAgent.GetScriptedFlags() | Agent.AIScriptedFrameFlags.Crouch);
                        }
                        else
                        {
                            currentAgent.SetScriptedFlags(currentAgent.GetScriptedFlags() & ~Agent.AIScriptedFrameFlags.Crouch);
                        }
                    }));
                cameraOptionCategory.AddOption(new BoolOptionViewModel(
                    GameTexts.FindText("str_cinematic_camera_walk_mode"),
                    GameTexts.FindText("str_cinematic_camera_walk_mode_hint"),
                    () =>
                    {
                        var currentAgent = CinematicCameraLogic.GetCurrentAgent();
                        if (currentAgent == null)
                            return false;
                        return (currentAgent.GetScriptedFlags() & Agent.AIScriptedFrameFlags.DoNotRun) != 0;
                    },
                    b =>
                    {
                        var currentAgent = CinematicCameraLogic.GetCurrentAgent();
                        if (currentAgent == null)
                            return;
                        if (b)
                        {
                            currentAgent.SetScriptedFlags(currentAgent.GetScriptedFlags() | Agent.AIScriptedFrameFlags.DoNotRun);
                        }
                        else
                        {
                            currentAgent.SetScriptedFlags(currentAgent.GetScriptedFlags() & ~Agent.AIScriptedFrameFlags.DoNotRun);
                        }
                    }));

                cameraOptionCategory.AddOption(new ActionOptionViewModel(
                    GameTexts.FindText("str_cinematic_camera_add_current_agent_to_player_team"), null,
                    () =>
                    {
                        CinematicCameraLogic.AddToPlayerTeam();
                    }));
                cameraOptionCategory.AddOption(new ActionOptionViewModel(
                    GameTexts.FindText("str_cinematic_camera_add_current_agent_to_enemy_team"), null,
                    () =>
                    {
                        CinematicCameraLogic.AddToEnemyTeam();
                    }));
                cameraOptionCategory.AddOption(new ActionOptionViewModel(GameTexts.FindText("str_cinematic_camera_move_all_units_to_formation"),
                    GameTexts.FindText("str_cinematic_camera_move_all_units_to_formation_hint"),
                    () =>
                    {
                        CinematicCameraLogic.MoveAllUnitsWithoutFormationToFormation5();
                    }));
                cameraOptionCategory.AddOption(new ActionOptionViewModel(GameTexts.FindText("str_cinematic_camera_move_bodyguards_to_formation"),
                    GameTexts.FindText("str_cinematic_camera_move_bodyguards_to_formation_hint"),
                    () =>
                    {
                        CinematicCameraLogic.MoveAllBodyguardsToFormation6();
                    }));
                cameraOptionCategory.AddOption(new ActionOptionViewModel(GameTexts.FindText("str_cinematic_camera_move_heroes_to_formation"),
                    GameTexts.FindText("str_cinematic_camera_move_heroes_to_formation_hint"),
                    () =>
                    {
                        CinematicCameraLogic.MoveAllHeroesToFormation7();
                    }));
                cameraOptionCategory.AddOption(new ActionOptionViewModel(GameTexts.FindText("str_cinematic_camera_disable_calvary_charge"),
                    GameTexts.FindText("str_cinematic_camera_disable_calvary_charge_hint"),
                    () =>
                    {
                        var currentAgent = CinematicCameraLogic.GetCurrentAgent();
                        if (currentAgent == null)
                            return;

                        currentAgent.HumanAIComponent?.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.ChargeHorseback, 0, 25f, 0f, 30f, 0f);
                        currentAgent.HumanAIComponent?.SetBehaviorParams(HumanAIComponent.AISimpleBehaviorKind.RangedHorseback, 0, 10f, 0f, 20f, 0f);
                        currentAgent.HumanAIComponent?.SyncBehaviorParamsIfNecessary();
                    }));

                optionClass.AddOptionCategory(0, cameraOptionCategory);

                var actionOptionCategory =
                    new OptionCategory("Action", GameTexts.FindText("str_cinematic_camera_action_option"));
                actionOptionCategory.AddOption(new SelectionOptionViewModel(
                    GameTexts.FindText("str_cinematic_camera_all_actions_list"),
                    GameTexts.FindText("str_cinematic_camera_all_actions_list_hint"),
                    new ActionSelectionData(false, actionOptionCategory).SelectionOptionData, true));
                var favoriteActions = new SelectionOptionViewModel(
                    GameTexts.FindText("str_cinematic_camera_favorite_actions_list"),
                    GameTexts.FindText("str_cinematic_camera_favorite_actions_list_hint"),
                    new ActionSelectionData(true, actionOptionCategory).SelectionOptionData, true);
                actionOptionCategory.AddOption(new ActionOptionViewModel(
                    GameTexts.FindText("str_cinematic_camera_add_action_to_favorites"),
                    GameTexts.FindText("str_cinematic_camera_add_action_to_favorites_hint"),
                    () =>
                    {
                        var actionName = CinematicCameraConfig.Get().ActionName;
                        if (string.IsNullOrEmpty(actionName))
                        {
                            return;
                        }

                        var favorites = CinematicCameraConfig.Get().FavoriteActions;
                        if (favorites.Contains(actionName))
                            return;

                        favorites.Add(actionName);
                        favoriteActions.UpdateData(false);
                    }));
                actionOptionCategory.AddOption(new ActionOptionViewModel(
                    GameTexts.FindText("str_cinematic_camera_remove_action_from_favorites"),
                    GameTexts.FindText("str_cinematic_camera_remove_action_from_favorites_hint"),
                    () =>
                    {
                        var actionName = CinematicCameraConfig.Get().ActionName;
                        if (string.IsNullOrEmpty(actionName))
                        {
                            return;
                        }

                        var favorites = CinematicCameraConfig.Get().FavoriteActions;
                        if (!favorites.Contains(actionName))
                            return;

                        favorites.Remove(actionName);
                        favoriteActions.UpdateData(false);
                    }));
                actionOptionCategory.AddOption(favoriteActions);
                actionOptionCategory.AddOption(new ActionOptionViewModel(GameTexts.FindText("str_cinematic_camera_execute_action"),
                    GameTexts.FindText("str_cinematic_camera_execute_action_hint"),
                    () =>
                    {
                        var actionName = CinematicCameraConfig.Get().ActionName;
                        if (string.IsNullOrEmpty(actionName))
                        {
                            return;
                        }

                        CinematicCameraLogic.ExecuteAction(actionName);
                        menuManager.RequestToCloseMenu();
                    }));

                optionClass.AddOptionCategory(1, actionOptionCategory);

                var facialAnimationOptionCategory =
                    new OptionCategory("FacialAnimation", GameTexts.FindText("str_cinematic_camera_facial_animation_option"));
                facialAnimationOptionCategory.AddOption(new SelectionOptionViewModel(
                    GameTexts.FindText("str_cinematic_camera_all_facial_animations_list"),
                    GameTexts.FindText("str_cinematic_camera_all_facial_animations_list_hint"),
                    new FacialAnimationSelectionData(false, facialAnimationOptionCategory).SelectionOptionData, true));
                var favoriteFacialAnimations = new SelectionOptionViewModel(
                    GameTexts.FindText("str_cinematic_camera_favorite_facial_animations_list"),
                    GameTexts.FindText("str_cinematic_camera_favorite_facial_animations_list_hint"),
                    new FacialAnimationSelectionData(true, facialAnimationOptionCategory).SelectionOptionData, true);
                facialAnimationOptionCategory.AddOption(new ActionOptionViewModel(
                    GameTexts.FindText("str_cinematic_camera_add_facial_animation_to_favorites"),
                    GameTexts.FindText("str_cinematic_camera_add_facial_animation_to_favorites_hint"),
                    () =>
                    {
                        var facialAnimationName = CinematicCameraConfig.Get().FacialAnimation;
                        if (string.IsNullOrEmpty(facialAnimationName))
                        {
                            return;
                        }
                        var favorites = CinematicCameraConfig.Get().FavoriteFacialAnimations;
                        if (favorites.Contains(facialAnimationName))
                            return;
                        favorites.Add(facialAnimationName);
                        favoriteFacialAnimations.UpdateData(false);
                    }));
                facialAnimationOptionCategory.AddOption(new ActionOptionViewModel(
                    GameTexts.FindText("str_cinematic_camera_remove_facial_animation_from_favorites"),
                    GameTexts.FindText("str_cinematic_camera_remove_facial_animation_from_favorites_hint"),
                    () =>
                    {
                        var facialAnimationName = CinematicCameraConfig.Get().FacialAnimation;
                        if (string.IsNullOrEmpty(facialAnimationName))
                        {
                            return;
                        }
                        var favorites = CinematicCameraConfig.Get().FavoriteFacialAnimations;
                        if (!favorites.Contains(facialAnimationName))
                            return;
                        favorites.Remove(facialAnimationName);
                        favoriteFacialAnimations.UpdateData(false);
                    }));
                facialAnimationOptionCategory.AddOption(favoriteFacialAnimations);
                facialAnimationOptionCategory.AddOption(new ActionOptionViewModel(GameTexts.FindText("str_cinematic_camera_execute_facial_animation"),
                    GameTexts.FindText("str_cinematic_camera_execute_facial_animation_hint"),
                    () =>
                    {
                        var facialAnimationName = CinematicCameraConfig.Get().FacialAnimation;
                        if (string.IsNullOrEmpty(facialAnimationName))
                        {
                            return;
                        }
                        CinematicCameraLogic.ExecuteFacialAnimation(facialAnimationName);
                        menuManager.RequestToCloseMenu();
                    }));
                optionClass.AddOptionCategory(1, facialAnimationOptionCategory);

                return optionClass;
            }, CinematicCameraSubModule.ModuleId, new Version(1, 0, 0));
        }

        public static AgentBehaviorGroup GetActiveBehaviorGroup()
        {
            var currentAgent = CinematicCameraLogic.GetCurrentAgent();
            if (currentAgent == null)
            {
                return null;
            }
            var activeBehaviorGroup = currentAgent.GetComponent<CampaignAgentComponent>()?.AgentNavigator?.GetActiveBehaviorGroup();
            if (activeBehaviorGroup == null)
            {
                return null;
            }

            return activeBehaviorGroup;
        }
    }
}
