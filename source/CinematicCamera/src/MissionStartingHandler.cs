using CinematicCamera.MissionBehaviors;
using MissionLibrary.Controller;
using MissionSharedLibrary.Controller;
using System.Collections.Generic;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace CinematicCamera
{
    public class MissionStartingHandler : AMissionStartingHandler
    {
        public override void OnCreated(MissionView entranceView)
        {
            List<MissionBehavior> list = new List<MissionBehavior>
            {
                new SetPlayerHealthLogic(),
                new CinematicCameraMenuView()
            };


            foreach (var missionBehavior in list)
            {
                MissionStartingManager.AddMissionBehavior(entranceView, missionBehavior);
            }
        }

        public override void OnPreMissionTick(MissionView entranceView, float dt)
        {
        }
    }
}