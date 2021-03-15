using MissionLibrary.HotKey;
using MissionSharedLibrary.HotKey.Category;
using System;
using TaleWorlds.InputSystem;

namespace CinematicCamera.Config.HotKey
{
    public enum GameKeyEnum
    {
        TogglePlayerInvulnerable,
        ToggleMapLongPress,
        NumberOfGameKeyEnums
    }
    public class CinematicCameraGameKeyCategory
    {
        public const string CategoryId = "CinematicCameraHotKey";

        public static AGameKeyCategory Category => AGameKeyCategoryManager.Get().GetCategory(CategoryId);

        public static void RegisterGameKeyCategory()
        {
            AGameKeyCategoryManager.Get()?.AddCategory(CreateCategory, new Version(1, 0));
        }

        // not enabled
        public static GameKeyCategory CreateCategory()
        {
            var result = new GameKeyCategory(CategoryId,
                (int)GameKeyEnum.NumberOfGameKeyEnums, GameKeyConfig.Get());
            result.AddGameKey(new GameKey((int)GameKeyEnum.TogglePlayerInvulnerable, nameof(GameKeyEnum.TogglePlayerInvulnerable),
                CategoryId, InputKey.M, CategoryId));
            result.AddGameKey(new GameKey((int)GameKeyEnum.ToggleMapLongPress, nameof(GameKeyEnum.ToggleMapLongPress),
                CategoryId, InputKey.LeftAlt, CategoryId));
            return result;
        }

        public static InputKey GetKey(GameKeyEnum key)
        {
            return Category?.GetKey((int)key) ?? InputKey.Invalid;
        }
    }
}
