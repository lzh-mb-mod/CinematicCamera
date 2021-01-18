using System.Linq;
using MissionLibrary.Extension;
using MissionSharedLibrary;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace CinematicCamera
{
    public class CinematicCameraSubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            Utility.ShouldDisplayMessage = false;
            Module.CurrentModule.GlobalTextManager.LoadGameTexts(BasePath.Name + "Modules/CinematicCamera/ModuleData/module_strings.xml");
            MissionExtensionCollection.AddExtension(new CinematicCameraExtension());
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);

            game.GameTextManager.LoadGameTexts(BasePath.Name + "Modules/CinematicCamera/ModuleData/module_strings.xml");
        }

        private T GetGameModel<T>(IGameStarter gameStarter) where T : GameModel
        {
            GameModel[] gameModels = gameStarter.Models.ToArray();
            for (int index = gameModels.Length - 1; index >= 0; --index)
            {
                if (gameModels[index] is T gameModel)
                    return gameModel;
            }
            return default(T);
        }
    }
}
