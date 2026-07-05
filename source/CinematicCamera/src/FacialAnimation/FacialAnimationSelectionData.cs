using MissionSharedLibrary.View.ViewModelCollection;
using MissionSharedLibrary.View.ViewModelCollection.Options.Selection;
using System.Collections.Generic;
using System.Linq;

namespace CinematicCamera.FacialAnimation
{
    public class FacialAnimationSelectionData
    {
        public SelectionOptionData SelectionOptionData;
        private bool _isFavoriteList;
        private OptionCategory _optionCategory;

        public static List<string> GetDefaultFacialAnimationList()
        {
            return new List<string>()
            {
                "male_custom",
                "male_facegen",
                "female_facegen",
                "female_custom",
                "facegen_teeth",
                "portraits",
                "archers",
                "cavalry",
                "attack",
                "everyone",
                "followme",
                "infantry",
                "move",
                "retreat",
                "test",
                "test1",
                "test2",
                "talking_happy",
                "talking_angry",
                "talking_sad",
                "talking_mean",
                "talking_normal",
                "talking_engaged",
                "grunt",
                "death",
                "death2",
                "lift",
                "yell",
                "victory",
                "hit",
                "scream",
                "pain",
                "wow",
                "yeah",
                "hitfaceright",
                "hitfaceleft",
                "hitfaceup",
                "hitfacefront",
                "parry",
                "aim",
                "aim_shake",
                "release",
                "shocked",
                "rage",
                "shout",
                "cry",
                "scared",
                "sneer",
                "smirk",
                "happy",
                "smirkcon",
                "angry",
                "suspicious",
                "impressed",
                "terrified",
                "power",
                "confused",
                "hard_laugh",
                "laugh",
                "complimented",
                "arrow_death_1",
                "arrow_death_2",
                "arrow_death_3",
                "arrow_death_4",
                "arrow_death_5",
                "arrow_death_6",
                "arrow_death_7",
                "arrow_death_8",
                "arrow_death_9",
                "idle_angry",
                "idle_furious",
                "idle_insulted",
                "idle_nervous",
                "idle_scared",
                "idle_terrified",
                "idle_bandit",
                "idle_displeased",
                "idle_tired",
                "idle_flirty",
                "idle_concentration",
                "idle_pleased",
                "idle_normal",
                "idle_drunk",
                "idle_noble",
                "idle_guard",
                "idle_sick",
                "idle_despise",
                "idle_repulsed",
                "idle_condescend",
                "idle_cheering1",
                "idle_cheering2",
                "idle_survive",
                "idle_sleep",
                "idle_lookaround",
                "idle_epicwin",
                "idle_old",
                "idle_cheering3",
                "drink",
                "drink2",
                "eat_worried",
                "eat_pleased",
                "eat_weird",
                "eat_happy",
                "eat_all",
                "lookdown",
                "lookup",
                "lookleft",
                "lookright",
                "lookstupid",
                "convo_nervous2",
                "convo_innocent_smile",
                "convo_normal",
                "convo_angry",
                "convo_bared_teeth",
                "convo_dismayed",
                "convo_aggressive",
                "convo_predatory",
                "convo_furious",
                "convo_insulted",
                "convo_annoyed",
                "convo_confused_annoyed",
                "convo_bored",
                "convo_bored2",
                "convo_grave",
                "convo_stern",
                "convo_very_stern",
                "convo_undecided_open",
                "convo_undecided_closed",
                "convo_thinking",
                "convo_astonished",
                "convo_beaten",
                "convo_nervous",
                "convo_shocked",
                "convo_confused_normal",
                "convo_disgusted",
                "convo_uncomfortable",
                "convo_huge_smile",
                "convo_happy",
                "convo_calm_friendly",
                "convo_focused_happy",
                "convo_relaxed_happy",
                "convo_bemused",
                "convo_delighted",
                "convo_nonchalant",
                "convo_merry",
                "convo_approving",
                "convo_excited",
                "convo_contemptuous",
                "convo_mocking_teasing",
                "convo_mocking_revenge",
                "convo_mocking_aristocratic",
                "convo_irritable",
                "convo_grateful",
                "convo_evil_smile",
                "convo_pondering",
                "convo_disbelief",
                "convo_worried",
                "convo_uncomfortable_voice",
                "convo_confused_voice",
                "convo_angry_voice",
                "convo_focused_voice",
                "convo_empathic_voice",
                "convo_snide_voice",
                "stand_ag",
                "start_ag",
                "negative_ag",
                "positive_ag",
                "very_negative_ag",
                "very_positive_ag",
                "unsure_ag",
                "trivial_ag",
                "negative_no",
                "positive_no",
                "very_negative_no",
                "very_positive_no",
                "unsure_no",
                "trivial_no",
                "negative_hi",
                "positive_hi",
                "very_negative_hi",
                "very_positive_hi",
                "unsure_hi",
                "trivial_hi",
                "stand_co2",
                "negative_co2",
                "positive_co2",
                "very_negative_co2",
                "very_positive_co2",
                "unsure_co2",
                "trivial_co2",
                "stand_we",
                "negative_we",
                "positive_we",
                "unsure_we",
                "very_negative_we",
                "very_positive_we",
                "trivial_we",
                "negative_co1",
                "positive_co1",
                "very_negative_co1",
                "very_positive_co1",
                "unsure_co1",
                "trivial_co1",
                "stand_ne",
                "start_ne",
                "negative_ne",
                "positive_ne",
                "unsure_ne",
                "very_negative_ne",
                "very_positive_ne",
                "trivial_ne",
                "pose_1",
                "pose_2",
                "pose_3",
                "pose_4",
                "pose_5",
                "pose_6",
                "pose_7",
                "pose_8",
                "pose_9",
                "lookdirection_9",
                "lookdirection_3",
                "lookdirection_6",
                "lookdirection_12",
                "lookdirection_11",
                "lookdirection_2",
                "lookdirection_5",
                "lookdirection_7",
                "lookdirection_8",
                "lookdirection_4",
                "lookdirection_0",
                "declare_dragon_banner_face_01",
                "declare_dragon_banner_face_02",
                "declare_dragon_banner_face_03",
                "declare_dragon_banner_face_04",
                "declare_dragon_banner_face_05",
                "declare_dragon_banner_face_06",
                "declare_dragon_banner_face_07",
                "declare_dragon_banner_face_08",
                "declare_dragon_banner_face_09",
                "declare_dragon_banner_face_10",
                "declare_dragon_banner_face_11",
                "declare_dragon_banner_face_12",
                "cutscene_expression_1",
                "cutscene_expression_2",
                "sad_face_cutscene",
            };
        }

        public static List<string> GetFavoriteFacialAnimationList()
        {
            return CinematicCameraConfig.Get().FavoriteFacialAnimations;
        }

        private List<string> GetList()
        {
            if (_isFavoriteList)
            {
                return GetFavoriteFacialAnimationList();
            }
            else
            {
                return GetDefaultFacialAnimationList();
            }
        }

        public FacialAnimationSelectionData(bool isFavoriteList, OptionCategory optionCategory)
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

                    var newFacialAnimation = list[i];
                    if (CinematicCameraConfig.Get().FacialAnimation != newFacialAnimation)
                    {
                        CinematicCameraConfig.Get().FacialAnimation = newFacialAnimation;
                        _optionCategory.UpdateAllOptions();
                    }
                },
                () =>
                {
                    var list = GetList();
                    var index = list.FindIndex(facialAnimation =>
                    {
                        return facialAnimation == CinematicCameraConfig.Get().FacialAnimation;
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
                    return list.Select(facialAnimation =>
                    {
                        return new SelectionItem(false, facialAnimation);
                    });
                });
        }
    }
}
