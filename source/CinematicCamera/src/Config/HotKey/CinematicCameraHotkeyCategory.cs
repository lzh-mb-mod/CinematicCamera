using MissionLibrary.HotKey;
using MissionSharedLibrary.Config.HotKey;
using MissionSharedLibrary.HotKey;
using MissionSharedLibrary.Usage;
using System;
using System.Collections.Generic;
using TaleWorlds.InputSystem;

namespace CinematicCamera.Config.HotKey
{
    public enum GameKeyEnum
    {
        OpenCinematicCameraMenu,
        TogglePlayerInvulnerable,
        IncreaseDepthOfFieldDistance,
        DecreaseDepthOfFieldDistance,
        IncreaseDepthOfFieldStart,
        DecreaseDepthOfFieldStart,
        IncreaseDepthOfFieldEnd,
        DecreaseDepthOfFieldEnd,
        IncreaseFieldOfView,
        DecreaseFieldOfView,
        ResetFieldOfView,
        NumberOfGameKeyEnums
    }
    public class CinematicCameraGameKeyCategory
    {
        public const string CategoryId = "CinematicCameraHotKey";

        public static AGameKeyCategory Category => AGameKeyCategoryManager.Get().GetItem(CategoryId);

        public static void RegisterGameKeyCategory()
        {
            AGameKeyCategoryManager.Get()?.RegisterGameKeyCategory(CreateCategory, CategoryId, new Version(1, 0));
        }
        
        public static GameKeyCategory CreateCategory()
        {
            var result = new GameKeyCategory(CategoryId,
                (int)GameKeyEnum.NumberOfGameKeyEnums, GameKeyConfig.Get());
            result.AddGameKeySequence(new GameKeySequence((int)GameKeyEnum.OpenCinematicCameraMenu, nameof(GameKeyEnum.OpenCinematicCameraMenu),
                CategoryId, new List<GameKeySequenceAlternative>
                {
                    new GameKeySequenceAlternative(
                        new List<InputKey> () {
                            InputKey.RightControl,
                            InputKey.Comma
                        }
                    )
                }));
            result.AddGameKeySequence(new GameKeySequence((int)GameKeyEnum.TogglePlayerInvulnerable, nameof(GameKeyEnum.TogglePlayerInvulnerable),
                CategoryId, new List<GameKeySequenceAlternative>
                {
                    new GameKeySequenceAlternative(
                        new List<InputKey> () {
                            InputKey.LeftControl,
                            InputKey.H
                        }
                    )
                }));
            result.AddGameKeySequence(new GameKeySequence((int)GameKeyEnum.IncreaseDepthOfFieldDistance, nameof(GameKeyEnum.IncreaseDepthOfFieldDistance),
                CategoryId, new List<GameKeySequenceAlternative>
                {
                    new GameKeySequenceAlternative(
                        new List<InputKey> () {
                            InputKey.Comma,
                            InputKey.Equals
                        }
                    )
                }));
            result.AddGameKeySequence(new GameKeySequence((int)GameKeyEnum.DecreaseDepthOfFieldDistance, nameof(GameKeyEnum.DecreaseDepthOfFieldDistance),
                CategoryId, new List<GameKeySequenceAlternative>
                {
                    new GameKeySequenceAlternative(
                        new List<InputKey>
                        {
                            InputKey.Comma,
                            InputKey.Minus
                        }
                    )
                }));
            result.AddGameKeySequence(new GameKeySequence((int)GameKeyEnum.IncreaseDepthOfFieldStart, nameof(GameKeyEnum.IncreaseDepthOfFieldStart),
                CategoryId, new List<GameKeySequenceAlternative>
                {
                    new GameKeySequenceAlternative(
                        new List<InputKey>
                        {
                            InputKey.Period,
                            InputKey.Equals
                        }
                    )
                }));
            result.AddGameKeySequence(new GameKeySequence((int)GameKeyEnum.DecreaseDepthOfFieldStart, nameof(GameKeyEnum.DecreaseDepthOfFieldStart),
                CategoryId, new List<GameKeySequenceAlternative>
                {
                    new GameKeySequenceAlternative(
                        new List<InputKey>
                        {
                            InputKey.Period,
                            InputKey.Minus
                        }
                    )
                }));
            result.AddGameKeySequence(new GameKeySequence((int)GameKeyEnum.IncreaseDepthOfFieldEnd, nameof(GameKeyEnum.IncreaseDepthOfFieldEnd),
                CategoryId, new List<GameKeySequenceAlternative>
                {
                    new GameKeySequenceAlternative(
                        new List<InputKey>
                        {
                            InputKey.Slash,
                            InputKey.Equals
                        }
                    )
                }));
            result.AddGameKeySequence(new GameKeySequence((int)GameKeyEnum.DecreaseDepthOfFieldEnd, nameof(GameKeyEnum.DecreaseDepthOfFieldEnd),
                CategoryId, new List<GameKeySequenceAlternative>
                {
                    new GameKeySequenceAlternative(
                        new List<InputKey>
                        {
                            InputKey.Slash,
                            InputKey.Minus
                        }
                    )
                }));
            result.AddGameKeySequence(new GameKeySequence((int)GameKeyEnum.IncreaseFieldOfView, nameof(GameKeyEnum.IncreaseFieldOfView),
                CategoryId, new List<GameKeySequenceAlternative>
                {
                    new GameKeySequenceAlternative(
                        new List<InputKey>
                        {
                            InputKey.BackSlash,
                            InputKey.Equals
                        }
                    )
                }));
            result.AddGameKeySequence(new GameKeySequence((int)GameKeyEnum.DecreaseFieldOfView, nameof(GameKeyEnum.DecreaseFieldOfView),
                CategoryId, new List<GameKeySequenceAlternative>
                {
                    new GameKeySequenceAlternative(
                        new List<InputKey>
                {
                    InputKey.BackSlash,
                    InputKey.Minus
                        }
                    )
                }));
            result.AddGameKeySequence(new GameKeySequence((int)GameKeyEnum.ResetFieldOfView, nameof(GameKeyEnum.ResetFieldOfView),
                CategoryId, new List<GameKeySequenceAlternative>
                {
                    new GameKeySequenceAlternative(
                        new List<InputKey>
                    {
                        InputKey.BackSlash,
                        InputKey.OpenBraces
                        }
                    )
                }));
            return result;
        }

        public static IGameKeySequence GetKey(GameKeyEnum key)
        {
            return Category?.GetGameKeySequence((int)key);
        }
    }
}
