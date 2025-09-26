using CinematicCamera.Config.HotKey;
using MissionLibrary.Event;
using MissionSharedLibrary.Utilities;
using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace CinematicCamera
{
    public class CinematicCameraLogic : MissionLogic
    {
        private readonly CinematicCameraConfig _config = CinematicCameraConfig.Get();

        public static List<string> RegularMissionNames = new List<string>
        {
            "TownCenter",
            "TownAmbush",
            "Indoor",
            "PrisonBreak",
            "Village",
            "Alley",
            "Ambush",
            "Camp",
        };

        public static bool ShouldAddOrderUI(string missionName)
        {
            return CinematicCameraConfig.Get().OrderUIInRegularScene && RegularMissionNames.Contains(missionName);
        }

        public Agent CurrentAgent;

        public static void SelectAgent(Agent agent)
        {
            var logic = Mission.Current.GetMissionBehavior<CinematicCameraLogic>();
            if (logic == null)
                return;
            if (agent == null)
            {
                Utility.DisplayMessage(GameTexts.FindText("str_cinematic_camera_clear_current_agent").ToString());
                logic.CurrentAgent = null;
            }
            else
            {
                logic.CurrentAgent = agent;
                Utility.DisplayMessage(GameTexts.FindText("str_cinematic_camera_current_agent_set_to").SetTextVariable("AgentName", agent.Name).ToString());
            }
        }

        public static void AddToPlayerTeam()
        {
            var logic = Mission.Current.GetMissionBehavior<CinematicCameraLogic>();
            var missionScreen = Utility.GetMissionScreen();
            var agentToAdd = logic.CurrentAgent ?? missionScreen.LastFollowedAgent;
            if (agentToAdd != null)
            {
                agentToAdd.SetTeam(Mission.Current.PlayerTeam, true);
            }
            else
            {
                Utility.DisplayMessage(GameTexts.FindText("str_cinematic_camera_no_agent_to_add").ToString());
            }
        }

        public static void AddToEnemyTeam()
        {
            var logic = Mission.Current.GetMissionBehavior<CinematicCameraLogic>();
            var missionScreen = Utility.GetMissionScreen();
            var agentToAdd = logic.CurrentAgent ?? missionScreen.LastFollowedAgent;
            if (agentToAdd != null)
            {
                agentToAdd.SetTeam(Mission.Current.PlayerEnemyTeam, true);
            }
            else
            {
                Utility.DisplayMessage(GameTexts.FindText("str_cinematic_camera_no_agent_to_add").ToString());
            }
        }

        public override void OnBehaviorInitialize()
        {
            base.OnBehaviorInitialize();

            Mission.OnMainAgentChanged += Mission_OnMainAgentChanged;
            MissionEvent.MainAgentWillBeChangedToAnotherOne += MainAgentWillBeChangedToAnotherOne;
        }

        public override void OnRemoveBehavior()
        {
            base.OnRemoveBehavior();

            CurrentAgent = null;
            Mission.OnMainAgentChanged -= Mission_OnMainAgentChanged;
            MissionEvent.MainAgentWillBeChangedToAnotherOne -= MainAgentWillBeChangedToAnotherOne;
        }

        public override void OnAgentTeamChanged(Team prevTeam, Team newTeam, Agent agent)
        {
            base.OnAgentTeamChanged(prevTeam, newTeam, agent);

            if (MissionState.Current == null)
                return;
            if (!ShouldAddOrderUI(MissionState.Current.MissionName))
                return;
            if (newTeam != Mission.PlayerTeam)
                return;
            // crash if agent.Equipment is null and agent is added to formation.
            if (agent.Equipment != null)
                agent.Formation = newTeam.GetFormation(FormationClass.Infantry);
            if (!agent.IsPlayerControlled)
                return;
            newTeam.PlayerOrderController.Owner = agent;
            if (newTeam.IsPlayerGeneral)
            {
                foreach (Formation formation in (List<Formation>)newTeam.FormationsIncludingEmpty)
                    formation.PlayerOwner = agent;
            }
            newTeam.PlayerOrderController.SelectAllFormations();
        }

        public override void OnAgentBuild(Agent agent, Banner banner)
        {
            base.OnAgentBuild(agent, banner);

            if (!ShouldAddOrderUI(MissionState.Current.MissionName))
                return;
            if (agent.Team != Mission.PlayerTeam || agent.Team == null)
                return;
            if (agent.Equipment != null)
                agent.Formation = agent.Team.GetFormation(FormationClass.Infantry);
            if (!agent.IsPlayerControlled)
                return;
            agent.Team.PlayerOrderController.Owner = agent;
            if (agent.Team.IsPlayerGeneral)
            {
                foreach (Formation formation in (List<Formation>)agent.Team.FormationsIncludingEmpty)
                    formation.PlayerOwner = agent;
            }
            agent.Team.PlayerOrderController.SelectAllFormations();
        }

        public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
        {
            base.OnAgentRemoved(affectedAgent, affectorAgent, agentState, blow);
            if (CurrentAgent == affectedAgent)
                CurrentAgent = null;
        }

        public override void OnMissionTick(float dt)
        {
            base.OnMissionTick(dt);

            if (CinematicCameraGameKeyCategory.GetKey(GameKeyEnum.TogglePlayerInvulnerable)
                .IsKeyPressed(Mission.InputManager))
            {
                _config.PlayerInvulnerable = !_config.PlayerInvulnerable;
                UpdateInvulnerable(_config.PlayerInvulnerable);
            }
            if (CinematicCameraGameKeyCategory.GetKey(GameKeyEnum.IncreaseDepthOfFieldDistance)
                .IsKeyDown(Mission.InputManager))
            {
                _config.DepthOfFieldDistance = MathF.Clamp(_config.DepthOfFieldDistance + 0.05f, 0, 1000);
                ModifyCameraHelper.UpdateDepthOfFieldDistance();
                ModifyCameraHelper.UpdateDepthOfFieldParameters();
            }
            if (CinematicCameraGameKeyCategory.GetKey(GameKeyEnum.DecreaseDepthOfFieldDistance)
                .IsKeyDown(Mission.InputManager))
            {
                _config.DepthOfFieldDistance = MathF.Clamp(_config.DepthOfFieldDistance - 0.05f, 0, 1000);
                ModifyCameraHelper.UpdateDepthOfFieldDistance();
                ModifyCameraHelper.UpdateDepthOfFieldParameters();
            }
            if (CinematicCameraGameKeyCategory.GetKey(GameKeyEnum.IncreaseDepthOfFieldStart)
                .IsKeyDown(Mission.InputManager))
            {
                _config.DepthOfFieldStart = MathF.Clamp(_config.DepthOfFieldStart + 0.05f, 0, 1000);
                ModifyCameraHelper.UpdateDepthOfFieldDistance();
                ModifyCameraHelper.UpdateDepthOfFieldParameters();
            }
            if (CinematicCameraGameKeyCategory.GetKey(GameKeyEnum.DecreaseDepthOfFieldStart)
                .IsKeyDown(Mission.InputManager))
            {
                _config.DepthOfFieldStart = MathF.Clamp(_config.DepthOfFieldStart - 0.05f, 0, 1000);
                ModifyCameraHelper.UpdateDepthOfFieldDistance();
                ModifyCameraHelper.UpdateDepthOfFieldParameters();
            }
            if (CinematicCameraGameKeyCategory.GetKey(GameKeyEnum.IncreaseDepthOfFieldEnd)
                .IsKeyDown(Mission.InputManager))
            {
                _config.DepthOfFieldEnd = MathF.Clamp(_config.DepthOfFieldEnd + 0.05f, 0, 1000);
                ModifyCameraHelper.UpdateDepthOfFieldParameters();
            }
            if (CinematicCameraGameKeyCategory.GetKey(GameKeyEnum.DecreaseDepthOfFieldEnd)
                .IsKeyDown(Mission.InputManager))
            {
                _config.DepthOfFieldEnd = MathF.Clamp(_config.DepthOfFieldEnd - 0.05f, 0, 1000);
                ModifyCameraHelper.UpdateDepthOfFieldParameters();
            }

            if (CinematicCameraGameKeyCategory.GetKey(GameKeyEnum.IncreaseFieldOfView).IsKeyDown(Mission.InputManager))
            {
                _config.CameraFov = MathF.Clamp(_config.CameraFov + 0.05f, 1, 179);
                ModifyCameraHelper.UpdateFov();
            }
            if (CinematicCameraGameKeyCategory.GetKey(GameKeyEnum.DecreaseFieldOfView).IsKeyDown(Mission.InputManager))
            {
                _config.CameraFov = MathF.Clamp(_config.CameraFov - 0.05f, 1, 179);
                ModifyCameraHelper.UpdateFov();
            }
            if (CinematicCameraGameKeyCategory.GetKey(GameKeyEnum.ResetFieldOfView).IsKeyDown(Mission.InputManager))
            {
                _config.CameraFov = 65.0f;
                ModifyCameraHelper.UpdateFov();
            }
            if (CinematicCameraGameKeyCategory.GetKey(GameKeyEnum.CameraSpeedLow).IsKeyPressed(Mission.InputManager))
            {
                _config.SpeedFactor = _config.CameraSpeedLow;
                ModifyCameraHelper.UpdateSpeed();
            }
            if (CinematicCameraGameKeyCategory.GetKey(GameKeyEnum.CameraSpeedMiddle).IsKeyPressed(Mission.InputManager))
            {
                _config.SpeedFactor = _config.CameraSpeedMiddle;
                ModifyCameraHelper.UpdateSpeed();
            }
            if (CinematicCameraGameKeyCategory.GetKey(GameKeyEnum.CameraSpeedHigh).IsKeyPressed(Mission.InputManager))
            {
                _config.SpeedFactor = _config.CameraSpeedHigh;
                ModifyCameraHelper.UpdateSpeed();
            }
        }

        private void MainAgentWillBeChangedToAnotherOne(Agent newAgent)
        {
            if (_config.PlayerInvulnerable)
                UpdateInvulnerable(false);
        }

        private void Mission_OnMainAgentChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (Mission.MainAgent != null)
            {
                if (_config.PlayerInvulnerable)
                    UpdateInvulnerable(true);
            }
        }

        public void UpdateInvulnerable(bool invulnerable)
        {
            if (Mission.MainAgent == null)
                return;
            var agent = Mission.MainAgent;
            agent.SetMortalityState(invulnerable ? Agent.MortalityState.Immortal : Agent.MortalityState.Mortal);
            agent.MountAgent?.SetMortalityState(invulnerable ? Agent.MortalityState.Immortal : Agent.MortalityState.Mortal);
        }

        public static void MoveAllUnitsWithoutFormationToFormation5()
        {
            foreach (var agent in Mission.Current.PlayerTeam.ActiveAgents)
            {
                if (agent.Formation == null)
                {
                    agent.Formation = Mission.Current.PlayerTeam.GetFormation(FormationClass.Skirmisher);
                }
            }
        }

        public static void MoveAllBodyguardsToFormation6()
        {
            Mission.Current.PlayerTeam.GetFormation(FormationClass.Bodyguard).ApplyActionOnEachUnitViaBackupList(
                agent => agent.Formation = Mission.Current.PlayerTeam.GetFormation(FormationClass.HeavyInfantry));
        }

        public static void MoveAllHeroesToFormation7()
        {
            foreach (var agent in Mission.Current.PlayerTeam.GetHeroAgents())
            {
                agent.Formation = Mission.Current.PlayerTeam.GetFormation(FormationClass.LightCavalry);
            }
        }
    }
}
