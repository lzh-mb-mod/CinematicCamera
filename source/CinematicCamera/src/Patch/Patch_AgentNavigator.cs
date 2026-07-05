using HarmonyLib;
using MissionSharedLibrary.Utilities;
using SandBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace CinematicCamera.Patch
{
    public class Patch_AgentNavigator
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
                    typeof(AgentNavigator).GetMethod(nameof(AgentNavigator.SetTarget),
                        BindingFlags.Instance | BindingFlags.Public),
                    prefix: new HarmonyMethod(
                        typeof(Patch_AgentNavigator).GetMethod(nameof(Prefix_SetTarget),
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

        public static void Prefix_SetTarget(AgentNavigator __instance,
            UsableMachine usableMachine,
            bool isInitialTarget,
            Agent.AIScriptedFrameFlags customFlags)
        {
            //if (Mission.Current.IsFriendlyMission && (__instance.TargetUsableMachine != null || usableMachine != null))
            //{
            //    if (__instance.OwnerAgent.Formation != null)
            //    {
            //        CinematicCameraLogic.SetAgentFormation(__instance.OwnerAgent, null);
            //    }
            //}
        }
    }
}
