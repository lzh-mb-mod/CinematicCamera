using HarmonyLib;
using MissionSharedLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews.Order;
using static TaleWorlds.MountAndBlade.MovementOrder;

namespace CinematicCamera.Patch
{
    public class Patch_BattlePowerCalculationLogic
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
                    typeof(BattlePowerCalculationLogic).GetMethod("CalculateTeamPowers",
                        BindingFlags.Instance | BindingFlags.NonPublic),
                    prefix: new HarmonyMethod(
                        typeof(Patch_BattlePowerCalculationLogic).GetMethod(nameof(Prefix_CalculateTeamPowers),
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

        public static bool Prefix_CalculateTeamPowers(BattlePowerCalculationLogic __instance, Dictionary<Team, float>[] ____sidePowerData)
        {

            MissionAgentSpawnLogic missionBehavior = __instance.Mission.GetMissionBehavior<MissionAgentSpawnLogic>();
            if (missionBehavior == null)
            {
                Mission.TeamCollection teams = __instance.Mission.Teams;
                foreach (Team item in teams)
                {
                    ____sidePowerData[(int)item.Side].Add(item, 0f);
                }
                for (int i = 0; i < 2; i++)
                {
                    BattleSideEnum battleSideEnum = (BattleSideEnum)i;
                    IEnumerable<IAgentOriginBase> allTroopsForSide = missionBehavior.GetAllTroopsForSide(battleSideEnum);
                    Dictionary<Team, float> dictionary = ____sidePowerData[i];
                    bool isPlayerSide = __instance.Mission.PlayerTeam != null && __instance.Mission.PlayerTeam.Side == battleSideEnum;
                    var team = __instance.Mission.Teams.Where(team => team.Side == battleSideEnum);
                    foreach (var agent in team.SelectMany(team => team.ActiveAgents))
                    {
                        Team agentTeam = isPlayerSide ? (agent.Team == __instance.Mission.PlayerTeam ? __instance.Mission.PlayerTeam : __instance.Mission.PlayerAllyTeam) : __instance.Mission.PlayerEnemyTeam;
                        BasicCharacterObject troop = agent.Character;
                        dictionary[agentTeam] += troop.GetPower();
                    }
                }

                foreach (Team item3 in teams)
                {
                    item3.QuerySystem.Expire();
                }

                AccessTools.Property(typeof(BattlePowerCalculationLogic), nameof(BattlePowerCalculationLogic.IsTeamPowersCalculated)).SetValue(__instance, true);
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
