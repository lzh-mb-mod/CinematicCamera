using CinematicCamera.Config.HotKey;
using CinematicCamera.Patch;
using HarmonyLib;
using MissionLibrary;
using MissionLibrary.Controller;
using MissionLibrary.View;
using MissionSharedLibrary;
using MissionSharedLibrary.Utilities;
using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace CinematicCamera
{
    public class CinematicCameraSubModule : MBSubModuleBase
    {
        public static string ModuleId = "CinematicCamera";

        private readonly Harmony _harmony = new Harmony("CinematicCameraPatch");
        private bool _successPatch;
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            try
            {
                Initialize();
                Utility.ShouldDisplayMessage = true;
                _successPatch = true;
                _successPatch &= Patch_MissionState.Patch(_harmony);
                _successPatch &= Patch_BattlePowerCalculationLogic.Patch(_harmony);
                _successPatch &= Patch_AgentNavigator.Patch(_harmony);
                _successPatch &= Patch_VisualOrderProvider.Patch(_harmony);
            }
            catch (Exception e)
            {
                _successPatch = false;
                MBDebug.ConsolePrint(e.ToString());
            }
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

            if (!ThirdInitialize())
                return;
            if (!_successPatch)
            {
                InformationManager.DisplayMessage(new InformationMessage("Cinematic Camera: patch failed"));
            }

            try
            {
                Module.CurrentModule.GlobalTextManager.LoadGameTexts();
            }
            catch (Exception e)
            {
                MBDebug.ConsolePrint(e.ToString());
                InformationManager.DisplayMessage(new InformationMessage($"CinematicCamera: failed to load game texts: {e}"));
            }
        }

        private bool ThirdInitialize()
        {
            if (!Initializer.ThirdInitialize())
                return false;

            CinematicCameraGameKeyCategory.RegisterGameKeyCategory();
            Global.GetInstance<AMissionStartingManager>().AddHandler(new MissionStartingHandler());
            var menuClassCollection = AMenuManager.Get().MenuClassCollection;
            menuClassCollection.RegisterItem(CinematicCameraOptionClassFactory.CreateOptionClassProvider(menuClassCollection));
            AMenuManager.Get().OnMenuClosedEvent += CinematicCameraConfig.OnMenuClosed;
            AMenuManager.Get().OnMenuClosedEvent += ModifyCameraHelper.UpdateDepthOfFieldParameters;
            AMenuManager.Get().OnMenuClosedEvent += ModifyCameraHelper.UpdateDepthOfFieldDistance;
            return true;
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);

            game.GameTextManager.LoadGameTexts();
        }

        public override void OnMissionBehaviorInitialize(Mission mission)
        {
            base.OnMissionBehaviorInitialize(mission);

            ModifyCameraHelper.OnBehaviorInitialize();
        }
    }
}
