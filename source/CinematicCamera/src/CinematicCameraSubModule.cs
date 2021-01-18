using System.Linq;
using MissionLibrary;
using MissionLibrary.Controller;
using MissionLibrary.Extension;
using MissionLibrary.View;
using MissionSharedLibrary;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace CinematicCamera
{
    public class CinematicCameraSubModule : MBSubModuleBase
    {
        public static string ModuleId = "CinematicCamera";
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            Initialize();
            Utility.ShouldDisplayMessage = false;
            Module.CurrentModule.GlobalTextManager.LoadGameTexts(BasePath.Name + "Modules/CinematicCamera/ModuleData/module_strings.xml");
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);

            game.GameTextManager.LoadGameTexts(BasePath.Name + "Modules/CinematicCamera/ModuleData/module_strings.xml");
        }

        private void Initialize()
        {
            if (!Initializer.Initialize())
                return;
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();

            if (!SecondInitialize())
                return;
        }

        private bool SecondInitialize()
        {
            if (!Initializer.SecondInitialize())
                return false;
            
            Global.GetProvider<AMissionStartingManager>().AddHandler(new MissionStartingHandler());
            var menuClassCollection = AMenuManager.Get().MenuClassCollection;
            menuClassCollection.AddOptionClass(CinematicCameraOptionClassFactory.CreateOptionClassProvider(menuClassCollection));
            AMenuManager.Get().OnMenuClosedEvent += ModifyCameraHelper.UpdateDepthOfFieldParameters;
            AMenuManager.Get().OnMenuClosedEvent += ModifyCameraHelper.UpdateDepthOfFieldDistance;
            return true;
        }

        public override void OnMissionBehaviourInitialize(Mission mission)
        {
            base.OnMissionBehaviourInitialize(mission);

            ModifyCameraHelper.OnBehaviourInitialize();
        }
    }
}
