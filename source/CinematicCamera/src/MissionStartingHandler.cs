using CinematicCamera.MissionBehaviors;
using System.Collections.Generic;
using MissionLibrary.Controller;
using MissionSharedLibrary.Controller;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.Missions;

namespace CinematicCamera
{
    public class MissionStartingHandler : AMissionStartingHandler
    {
        public override void OnCreated(MissionView entranceView)
        {
            List<MissionBehaviour> list = new List<MissionBehaviour>
            {
                new SetPlayerHealthLogic(),
                new CinematicCameraMenuView()
            };


            foreach (var missionBehaviour in list)
            {
                MissionStartingManager.AddMissionBehaviour(entranceView, missionBehaviour);
            }
        }

        public override void OnPreMissionTick(MissionView entranceView, float dt)
        {
        }
    }
}