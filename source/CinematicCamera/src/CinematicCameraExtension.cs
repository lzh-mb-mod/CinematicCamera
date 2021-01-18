using MissionLibrary.Extension;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace CinematicCamera
{
    public class CinematicCameraExtension : IMissionExtension
    {
        public void OpenModMenu(Mission mission)
        {
        }

        public void CloseModMenu(Mission mission)
        {
            mission.GetMissionBehaviour<ModifyCameraLogic>()?.UpdateDepthOfFieldParameters();
            mission.GetMissionBehaviour<ModifyCameraLogic>()?.UpdateDepthOfFieldDistance();
        }

        public void OpenExtensionMenu(Mission mission)
        {
            mission.GetMissionBehaviour<CinematicCameraMenuView>()?.ActivateMenu();
        }

        public List<MissionBehaviour> CreateMissionBehaviours(Mission mission)
        {
            return new List<MissionBehaviour>
            {
                new SetPlayerHealthLogic(),
                new ModifyCameraLogic(),
                new CinematicCameraMenuView(),
            };
        }

        public string ExtensionName => "Cinematic Camera";

        public string ButtonName => GameTexts.FindText("str_extension_cinematic_camera").ToString();
    }
}
