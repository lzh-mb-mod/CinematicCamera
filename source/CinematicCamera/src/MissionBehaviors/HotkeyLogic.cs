﻿using CinematicCamera.Config.HotKey;
using MissionLibrary.Event;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace CinematicCamera
{
    public class HotkeyLogic : MissionLogic
    {
        private readonly CinematicCameraConfig _config = CinematicCameraConfig.Get();

        public override void OnBehaviorInitialize()
        {
            base.OnBehaviorInitialize();

            Mission.OnMainAgentChanged += Mission_OnMainAgentChanged;
            MissionEvent.MainAgentWillBeChangedToAnotherOne += MainAgentWillBeChangedToAnotherOne;
        }

        public override void OnRemoveBehavior()
        {
            base.OnRemoveBehavior();

            Mission.OnMainAgentChanged -= Mission_OnMainAgentChanged;
            MissionEvent.MainAgentWillBeChangedToAnotherOne -= MainAgentWillBeChangedToAnotherOne;
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
    }
}
