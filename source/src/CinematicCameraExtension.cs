using RTSCamera;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace CinematicCamera
{
    public class CinematicCameraExtension : RTSCameraExtension
    {
        public override void OpenModMenu(Mission mission)
        {
        }

        public override void CloseModMenu(Mission mission)
        {
            mission.GetMissionBehaviour<ModifyCameraLogic>()?.UpdateDepthOfFieldParameters();
            mission.GetMissionBehaviour<ModifyCameraLogic>()?.UpdateDepthOfFieldDistance();
        }

        public override void OpenExtensionMenu(Mission mission)
        {
            mission.GetMissionBehaviour<CinematicCameraMenuView>()?.ActivateMenu();
        }

        public override List<MissionBehaviour> CreateMissionBehaviours(Mission mission)
        {
            return new List<MissionBehaviour>
            {
                new SetPlayerHealthLogic(),
                new ModifyCameraLogic(),
                new CinematicCameraMenuView(),
            };
        }

        public override string ExtensionName => "Cinematic Camera";

        public override string ButtonName => GameTexts.FindText("str_extension_cinematic_camera").ToString();
    }
}
