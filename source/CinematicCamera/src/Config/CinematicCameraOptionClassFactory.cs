using CinematicCamera.MissionBehaviors;
using MissionLibrary.Provider;
using MissionLibrary.View;
using MissionSharedLibrary.Provider;
using MissionSharedLibrary.Utilities;
using MissionSharedLibrary.View.ViewModelCollection;
using MissionSharedLibrary.View.ViewModelCollection.Options;
using MissionSharedLibrary.View.ViewModelCollection.Options.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.Screens;

namespace CinematicCamera
{
    public class CurrentAgentSelectionData
    {
        public SelectionOptionData SelectionOptionData;
        private readonly CinematicCameraLogic _logic = Mission.Current.GetMissionBehavior<CinematicCameraLogic>();

        public CurrentAgentSelectionData(MissionScreen missionScreen)
        {
            var agents = GetAgentList();
            SelectionOptionData = new SelectionOptionData(i =>
                {
                    var agents = GetAgentList();
                    if (Mission.Current?.Mode == MissionMode.Deployment)
                        return;
                    if (i >= 0 && i < agents.Count)
                    {
                        CinematicCameraLogic.SelectAgent(agents[i]);
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
                var currentAgentOption = new SelectionOptionViewModel(
                    GameTexts.FindText("str_cinematic_camera_current_agent"),
                    GameTexts.FindText("str_cinematic_camera_current_agent_hint"),
                    new CurrentAgentSelectionData(Utility.GetMissionScreen()).SelectionOptionData, true);
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
                            CinematicCameraLogic.SelectAgent(agentToFollow);
                            currentAgentOption.UpdateData(false);
                        }
                    }));

                cameraOptionCategory.AddOption(new ActionOptionViewModel(GameTexts.FindText("str_cinematic_camera_add_to_player_team"), null,
                    () =>
                    {
                        CinematicCameraLogic.AddToPlayerTeam();
                    }));
                cameraOptionCategory.AddOption(new ActionOptionViewModel(GameTexts.FindText("str_cinematic_camera_add_to_enemy_team"), null,
                    () =>
                    {
                        CinematicCameraLogic.AddToEnemyTeam();
                    }));
                cameraOptionCategory.AddOption(new BoolOptionViewModel(GameTexts.FindText("str_cinematic_camera_enable_giving_commands_in_regular_scene"),
                    GameTexts.FindText("str_cinematic_camera_enable_giving_commands_in_regular_scene_hint"),
                    () => CinematicCameraConfig.Get().OrderUIInRegularScene,
                    b => { CinematicCameraConfig.Get().OrderUIInRegularScene = b; }));
                optionClass.AddOptionCategory(0, cameraOptionCategory);
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
                optionClass.AddOptionCategory(0, cameraOptionCategory);

                return optionClass;
            }, CinematicCameraSubModule.ModuleId, new Version(1, 0, 0));
        }
    }
}
