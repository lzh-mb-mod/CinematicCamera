using CinematicCamera.Config.HotKey;
using MissionLibrary;
using MissionLibrary.Controller;
using MissionLibrary.View;
using MissionSharedLibrary;
using MissionSharedLibrary.Utilities;
using TaleWorlds.Core;
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
            Module.CurrentModule.GlobalTextManager.LoadGameTexts();
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);

            game.GameTextManager.LoadGameTexts();
        }

        private void Initialize()
        {
            if (!Initializer.Initialize(ModuleId))
                return;
        }

        protected override void OnApplicationTick(float dt)
        {
            base.OnApplicationTick(dt);

            Initializer.OnApplicationTick(dt);
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

            CinematicCameraGameKeyCategory.RegisterGameKeyCategory();
            Global.GetInstance<AMissionStartingManager>().AddHandler(new MissionStartingHandler());
            var menuClassCollection = AMenuManager.Get().MenuClassCollection;
            menuClassCollection.RegisterItem(CinematicCameraOptionClassFactory.CreateOptionClassProvider(menuClassCollection));
            AMenuManager.Get().OnMenuClosedEvent += ModifyCameraHelper.UpdateDepthOfFieldParameters;
            AMenuManager.Get().OnMenuClosedEvent += ModifyCameraHelper.UpdateDepthOfFieldDistance;
            return true;
        }

        public override void OnMissionBehaviorInitialize(Mission mission)
        {
            base.OnMissionBehaviorInitialize(mission);

            ModifyCameraHelper.OnBehaviorInitialize();
        }
    }
}
