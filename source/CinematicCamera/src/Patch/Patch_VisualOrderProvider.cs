using HarmonyLib;
using MissionSharedLibrary.Utilities;
using System;
using System.Reflection;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.VisualOrders;

namespace CinematicCamera.Patch
{
    public class Patch_VisualOrderProvider
    {
        private static bool _patched;
        public static bool Patch(Harmony harmony)
        {
            try
            {
                if (_patched)
                    return false;
                _patched = true;

                harmony.Patch(
                    typeof(DefaultVisualOrderProvider).GetMethod(nameof(DefaultVisualOrderProvider.IsAvailable),
                        BindingFlags.Instance | BindingFlags.Public),
                    prefix: new HarmonyMethod(
                        typeof(Patch_VisualOrderProvider).GetMethod(nameof(Prefix_IsAvailable),
                            BindingFlags.Static | BindingFlags.Public)));
                var rtsCommandVisualOrderProviderType = AccessTools.TypeByName("RTSCamera.CommandSystem.Orders.RTSCommandVisualOrderProvider");
                if (rtsCommandVisualOrderProviderType != null)
                {
                    harmony.Patch(
                        rtsCommandVisualOrderProviderType.GetMethod(nameof(DefaultVisualOrderProvider.IsAvailable),
                            BindingFlags.Instance | BindingFlags.Public),
                        prefix: new HarmonyMethod(
                            typeof(Patch_VisualOrderProvider).GetMethod(nameof(Prefix_IsAvailable),
                                BindingFlags.Static | BindingFlags.Public)));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Utility.DisplayMessage(e.ToString());
                return false;
            }

            return true;
        }

        public static bool Prefix_IsAvailable(ref bool __result)
        {
            if (Mission.Current != null && Mission.Current.IsFriendlyMission && CinematicCameraLogic.ShouldAddOrderUI(MissionState.Current.MissionName))
            {
                __result = true;
                return false;
            }
            return true;
        }
    }
}
