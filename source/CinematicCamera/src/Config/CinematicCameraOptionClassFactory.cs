using CinematicCamera.MissionBehaviors;
using MissionLibrary.Provider;
using MissionLibrary.View;
using MissionSharedLibrary.Provider;
using MissionSharedLibrary.View;
using MissionSharedLibrary.View.ViewModelCollection;
using MissionSharedLibrary.View.ViewModelCollection.Options;
using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace CinematicCamera
{
    public class CinematicCameraOptionClassFactory
    {
        public static IProvider<AOptionClass> CreateOptionClassProvider(AMenuClassCollection menuClassCollection)
        {
            return ProviderCreator.Create(() =>
            {
                var menuManager = AMenuManager.Get();
                var optionClass = new OptionClass(CinematicCameraSubModule.ModuleId,
                    GameTexts.FindText("str_cinematic_camera_cinematic_camera"), menuClassCollection);
                var cameraOptionCategory =
                    new OptionCategory("Camera", GameTexts.FindText("str_cinematic_camera_cinematic_camera"));

                cameraOptionCategory.AddOption(new ActionOptionViewModel(GameTexts.FindText("str_cinematic_camera_open_menu"), null,
                    () =>
                    {
                        menuManager.RequestToCloseMenu();
                        Mission.Current.GetMissionBehavior<CinematicCameraMenuView>()?.ActivateMenu();
                    }));
                optionClass.AddOptionCategory(0, cameraOptionCategory);

                return optionClass;
            }, CinematicCameraSubModule.ModuleId, new Version(1, 0, 0));
        }
    }
}
