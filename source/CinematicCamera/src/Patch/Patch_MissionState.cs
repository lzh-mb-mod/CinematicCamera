using HarmonyLib;
using MissionSharedLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.TroopSuppliers;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews.Order;
using static TaleWorlds.MountAndBlade.Mission;

namespace CinematicCamera.Patch
{
    public class Patch_MissionState
    {
        private static bool _patched;
        public static bool Patch(Harmony harmony)
        {
            try
            {
                if (_patched)
                    return false;
                _patched = true;

                // recover player formation from general formation
                harmony.Patch(
                    typeof(MissionState).GetMethod("HandleOpenNew",
                        BindingFlags.Instance | BindingFlags.NonPublic),
                    postfix: new HarmonyMethod(
                        typeof(Patch_MissionState).GetMethod(nameof(Postfix_HandleOpenNew),
                            BindingFlags.Static | BindingFlags.Public)));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Utility.DisplayMessage(e.ToString());
                return false;
            }

            return true;
        }

        public static void Postfix_HandleOpenNew(MissionState __instance, string missionName)
        {
            if (CinematicCameraLogic.ShouldAddOrderUI(missionName))
            {
                var list = new List<MissionBehavior>
                {
                    new BattlePowerCalculationLogic(),
                    new OrderTroopPlacer(),
                    ViewCreator.CreateMissionOrderUIHandler()
                };
                foreach (var missionBehavior in list)
                {
                    __instance.CurrentMission.AddMissionBehavior(missionBehavior);
                }
            }
        }
    }
}
