using MissionSharedLibrary.View.ViewModelCollection;
using MissionSharedLibrary.View.ViewModelCollection.Options.Selection;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.MountAndBlade;

namespace CinematicCamera.Actions
{
    public class ActionSelectionData
    {
        public SelectionOptionData SelectionOptionData;
        private bool _isFavoriteList;
        private OptionCategory _optionCategory;

        public static List<ActionIndexCache> GetDefaultActionList()
        {
            return new List<ActionIndexCache>()
            {
                ActionIndexCache.act_none,
                ActionIndexCache.act_pickup_down_begin,
                ActionIndexCache.act_pickup_down_end,
                ActionIndexCache.act_pickup_down_begin_left_stance,
                ActionIndexCache.act_pickup_down_end_left_stance,
                ActionIndexCache.act_pickup_down_left_begin,
                ActionIndexCache.act_pickup_down_left_end,
                ActionIndexCache.act_pickup_down_left_begin_left_stance,
                ActionIndexCache.act_pickup_down_left_end_left_stance,
                ActionIndexCache.act_pickup_middle_begin,
                ActionIndexCache.act_pickup_middle_end,
                ActionIndexCache.act_pickup_middle_begin_left_stance,
                ActionIndexCache.act_pickup_middle_end_left_stance,
                ActionIndexCache.act_pickup_middle_left_begin,
                ActionIndexCache.act_pickup_middle_left_end,
                ActionIndexCache.act_pickup_middle_left_begin_left_stance,
                ActionIndexCache.act_pickup_middle_left_end_left_stance,
                ActionIndexCache.act_pickup_up_begin,
                ActionIndexCache.act_pickup_up_end,
                ActionIndexCache.act_pickup_up_begin_left_stance,
                ActionIndexCache.act_pickup_up_end_left_stance,
                ActionIndexCache.act_pickup_up_left_begin,
                ActionIndexCache.act_pickup_up_left_end,
                ActionIndexCache.act_pickup_up_left_begin_left_stance,
                ActionIndexCache.act_pickup_up_left_end_left_stance,
                ActionIndexCache.act_pickup_from_right_down_horseback_begin,
                ActionIndexCache.act_pickup_from_right_down_horseback_end,
                ActionIndexCache.act_pickup_from_right_down_horseback_left_begin,
                ActionIndexCache.act_pickup_from_right_down_horseback_left_end,
                ActionIndexCache.act_pickup_from_right_middle_horseback_begin,
                ActionIndexCache.act_pickup_from_right_middle_horseback_end,
                ActionIndexCache.act_pickup_from_right_middle_horseback_left_begin,
                ActionIndexCache.act_pickup_from_right_middle_horseback_left_end,
                ActionIndexCache.act_pickup_from_right_up_horseback_begin,
                ActionIndexCache.act_pickup_from_right_up_horseback_end,
                ActionIndexCache.act_pickup_from_right_up_horseback_left_begin,
                ActionIndexCache.act_pickup_from_right_up_horseback_left_end,
                ActionIndexCache.act_pickup_from_left_down_horseback_begin,
                ActionIndexCache.act_pickup_from_left_down_horseback_end,
                ActionIndexCache.act_pickup_from_left_down_horseback_left_begin,
                ActionIndexCache.act_pickup_from_left_down_horseback_left_end,
                ActionIndexCache.act_pickup_from_left_middle_horseback_begin,
                ActionIndexCache.act_pickup_from_left_middle_horseback_end,
                ActionIndexCache.act_pickup_from_left_middle_horseback_left_begin,
                ActionIndexCache.act_pickup_from_left_middle_horseback_left_end,
                ActionIndexCache.act_pickup_from_left_up_horseback_begin,
                ActionIndexCache.act_pickup_from_left_up_horseback_end,
                ActionIndexCache.act_pickup_from_left_up_horseback_left_begin,
                ActionIndexCache.act_pickup_from_left_up_horseback_left_end,
                ActionIndexCache.act_pickup_boulder_begin,
                ActionIndexCache.act_pickup_boulder_end,
                ActionIndexCache.act_usage_trebuchet_idle,
                ActionIndexCache.act_usage_trebuchet_reload,
                ActionIndexCache.act_usage_trebuchet_reload_2,
                ActionIndexCache.act_usage_trebuchet_reload_idle,
                ActionIndexCache.act_usage_trebuchet_reload_2_idle,
                ActionIndexCache.act_usage_trebuchet_load_ammo,
                ActionIndexCache.act_usage_trebuchet_shoot,
                ActionIndexCache.act_usage_siege_machine_push,
                ActionIndexCache.act_usage_ladder_lift_from_left_1_start,
                ActionIndexCache.act_usage_ladder_lift_from_left_2_start,
                ActionIndexCache.act_usage_ladder_lift_from_right_1_start,
                ActionIndexCache.act_usage_ladder_lift_from_right_2_start,
                ActionIndexCache.act_usage_ladder_pick_up_fork_begin,
                ActionIndexCache.act_usage_ladder_pick_up_fork_end,
                ActionIndexCache.act_usage_ladder_push_back,
                ActionIndexCache.act_usage_ladder_push_back_stopped,
                ActionIndexCache.act_usage_batteringram_left,
                ActionIndexCache.act_usage_batteringram_left_slower,
                ActionIndexCache.act_usage_batteringram_left_slowest,
                ActionIndexCache.act_usage_batteringram_right,
                ActionIndexCache.act_usage_batteringram_right_slower,
                ActionIndexCache.act_usage_batteringram_right_slowest,
                ActionIndexCache.act_strike_bent_over,
                ActionIndexCache.act_strike_fall_back_back_rise,
                ActionIndexCache.act_row_strike,
                ActionIndexCache.act_stagger_forward,
                ActionIndexCache.act_stagger_backward,
                ActionIndexCache.act_stagger_right,
                ActionIndexCache.act_stagger_left,
                ActionIndexCache.act_stagger_forward_2,
                ActionIndexCache.act_stagger_backward_2,
                ActionIndexCache.act_stagger_right_2,
                ActionIndexCache.act_stagger_left_2,
                ActionIndexCache.act_stagger_forward_3,
                ActionIndexCache.act_stagger_backward_3,
                ActionIndexCache.act_stagger_right_3,
                ActionIndexCache.act_stagger_left_3,
                ActionIndexCache.act_command,
                ActionIndexCache.act_command_leftstance,
                ActionIndexCache.act_command_unarmed,
                ActionIndexCache.act_command_unarmed_leftstance,
                ActionIndexCache.act_command_2h,
                ActionIndexCache.act_command_2h_leftstance,
                ActionIndexCache.act_command_bow,
                ActionIndexCache.act_command_follow,
                ActionIndexCache.act_command_follow_leftstance,
                ActionIndexCache.act_command_follow_unarmed,
                ActionIndexCache.act_command_follow_unarmed_leftstance,
                ActionIndexCache.act_command_follow_2h,
                ActionIndexCache.act_command_follow_2h_leftstance,
                ActionIndexCache.act_command_follow_bow,
                ActionIndexCache.act_horse_command,
                ActionIndexCache.act_horse_command_unarmed,
                ActionIndexCache.act_horse_command_2h,
                ActionIndexCache.act_horse_command_bow,
                ActionIndexCache.act_horse_command_follow,
                ActionIndexCache.act_horse_command_follow_unarmed,
                ActionIndexCache.act_horse_command_follow_2h,
                ActionIndexCache.act_horse_command_follow_bow,
                ActionIndexCache.act_ship_connection_break,
                ActionIndexCache.act_usage_hook_ready,
                ActionIndexCache.act_usage_hook_release,
                ActionIndexCache.act_usage_row_idle_no_hold,
                ActionIndexCache.act_t_pose,
                ActionIndexCache.act_jump_loop,
                ActionIndexCache.act_stand_1,
                ActionIndexCache.act_idle_unarmed_1,
                ActionIndexCache.act_walk_idle_1h_with_shield_left_stance,
                ActionIndexCache.act_crouch_walk_idle_unarmed,
                ActionIndexCache.act_beggar_idle,
                ActionIndexCache.act_walk_idle_unarmed,
                ActionIndexCache.act_horse_stand_1,
                ActionIndexCache.act_hero_mount_idle_camel,
                ActionIndexCache.act_camel_idle_1,
                ActionIndexCache.act_tableau_hand_armor_pose,
                ActionIndexCache.act_inventory_idle_start,
                ActionIndexCache.act_inventory_idle,
                ActionIndexCache.act_inventory_glove_equip,
                ActionIndexCache.act_inventory_cloth_equip,
                ActionIndexCache.act_conversation_normal_loop,
                ActionIndexCache.act_conversation_warrior_loop,
                ActionIndexCache.act_conversation_hip_loop,
                ActionIndexCache.act_conversation_closed_loop,
                ActionIndexCache.act_conversation_demure_loop,
                ActionIndexCache.act_scared_reaction_1,
                ActionIndexCache.act_scared_idle_1,
                ActionIndexCache.act_greeting_front_1,
                ActionIndexCache.act_greeting_front_2,
                ActionIndexCache.act_greeting_front_3,
                ActionIndexCache.act_greeting_front_4,
                ActionIndexCache.act_greeting_right_1,
                ActionIndexCache.act_greeting_right_2,
                ActionIndexCache.act_greeting_right_3,
                ActionIndexCache.act_greeting_right_4,
                ActionIndexCache.act_greeting_left_1,
                ActionIndexCache.act_greeting_left_2,
                ActionIndexCache.act_greeting_left_3,
                ActionIndexCache.act_greeting_left_4,
                ActionIndexCache.act_guard_cautious_look_around_1,
                ActionIndexCache.act_guard_patrolling_cautious_look_around_1,
                ActionIndexCache.act_use_smithing_machine_ready,
                ActionIndexCache.act_use_smithing_machine_loop,
                ActionIndexCache.act_smithing_machine_anvil_start,
                ActionIndexCache.act_smithing_machine_anvil_part_2,
                ActionIndexCache.act_smithing_machine_anvil_part_4,
                ActionIndexCache.act_smithing_machine_anvil_part_5,
                ActionIndexCache.act_childhood_schooled,
                ActionIndexCache.act_arena_spectator,
                ActionIndexCache.act_argue_trio_middle,
                ActionIndexCache.act_argue_trio_middle_2,
                ActionIndexCache.act_argue_trio_left,
                ActionIndexCache.act_argue_trio_right,
                ActionIndexCache.act_taunt_cheer_1,
                ActionIndexCache.act_taunt_cheer_2,
                ActionIndexCache.act_taunt_cheer_3,
                ActionIndexCache.act_taunt_cheer_4,
                ActionIndexCache.act_cheering_low_01,
                ActionIndexCache.act_cheering_low_02,
                ActionIndexCache.act_cheering_low_03,
                ActionIndexCache.act_cheering_low_04,
                ActionIndexCache.act_cheering_low_05,
                ActionIndexCache.act_cheering_low_06,
                ActionIndexCache.act_cheering_low_07,
                ActionIndexCache.act_cheering_low_08,
                ActionIndexCache.act_cheering_low_09,
                ActionIndexCache.act_cheering_low_10,
                ActionIndexCache.act_cheer_1,
                ActionIndexCache.act_cheer_2,
                ActionIndexCache.act_cheer_3,
                ActionIndexCache.act_cheer_4,
                ActionIndexCache.act_cheering_high_01,
                ActionIndexCache.act_cheering_high_02,
                ActionIndexCache.act_cheering_high_03,
                ActionIndexCache.act_cheering_high_04,
                ActionIndexCache.act_cheering_high_05,
                ActionIndexCache.act_cheering_high_06,
                ActionIndexCache.act_cheering_high_07,
                ActionIndexCache.act_cheering_high_08,
                ActionIndexCache.act_map_raid,
                ActionIndexCache.act_map_rider_camel_attack_1h,
                ActionIndexCache.act_map_rider_camel_attack_1h_spear,
                ActionIndexCache.act_map_rider_camel_attack_1h_swing,
                ActionIndexCache.act_map_rider_camel_attack_2h_swing,
                ActionIndexCache.act_map_rider_camel_attack_unarmed,
                ActionIndexCache.act_map_rider_horse_attack_1h,
                ActionIndexCache.act_map_rider_horse_attack_1h_spear,
                ActionIndexCache.act_map_rider_horse_attack_1h_swing,
                ActionIndexCache.act_map_rider_horse_attack_2h_swing,
                ActionIndexCache.act_map_rider_horse_attack_unarmed,
                ActionIndexCache.act_map_mount_attack_1h,
                ActionIndexCache.act_map_mount_attack_spear,
                ActionIndexCache.act_map_mount_attack_swing,
                ActionIndexCache.act_map_mount_attack_unarmed,
                ActionIndexCache.act_map_attack_1h,
                ActionIndexCache.act_map_attack_2h,
                ActionIndexCache.act_map_attack_spear_1h_or_2h,
                ActionIndexCache.act_map_attack_unarmed,
                ActionIndexCache.act_conversation_naval_start,
                ActionIndexCache.act_conversation_naval_idle_loop,
                ActionIndexCache.act_death_by_arrow_pelvis,
                ActionIndexCache.act_horse_fall_right,
                ActionIndexCache.act_cutscene_npc_argue_player_1,
                ActionIndexCache.act_escape_jump,
            };
        }

        public static List<ActionIndexCache> GetFavoriteActionList()
        {
            var defaultList = GetDefaultActionList();
            return CinematicCameraConfig.Get().FavoriteActions.Select(actionName =>
                defaultList.FirstOrDefault(action => action.GetName() == actionName))
                .Where(action => action != null).ToList();
        }

        private List<ActionIndexCache> GetList()
        {
            if (_isFavoriteList)
            {
                return GetFavoriteActionList();
            }
            else
            {
                return GetDefaultActionList();
            }
        }

        public ActionSelectionData(bool isFavoriteList, OptionCategory optionCategory)
        {
            _isFavoriteList = isFavoriteList;
            _optionCategory = optionCategory;
            SelectionOptionData = new SelectionOptionData(
                i =>
                {
                    var list = GetList();
                    if (i < 0 || i >= list.Count)
                    {
                        return;
                    }

                    var newName = list[i].GetName();
                    if (CinematicCameraConfig.Get().ActionName != newName)
                    {
                        CinematicCameraConfig.Get().ActionName = list[i].GetName();
                        _optionCategory.UpdateAllOptions();
                    }
                },
                () =>
                {
                    var list = GetList();
                    var index = list.FindIndex(actionIndexCache =>
                    {
                        return actionIndexCache.GetName() == CinematicCameraConfig.Get().ActionName;
                    });
                    return index;
                },
                () =>
                {
                    var list = GetList();
                    return list.Count;
                },
                () =>
                {
                    var list = GetList();
                    return list.Select(actionIndexCache =>
                    {
                        return new SelectionItem(false, actionIndexCache.GetName());
                    });
                });
        }
    }
}
