﻿using MissionSharedLibrary.Config;
using MissionSharedLibrary.Config.HotKey;
using MissionSharedLibrary.Utilities;
using System;
using System.IO;
using TaleWorlds.MountAndBlade;

namespace CinematicCamera.Config.HotKey
{
    public class GameKeyConfig : GameKeyConfigBase<GameKeyConfig>
    {
        protected override string SaveName { get; } =
            Path.Combine(ConfigPath.ConfigDir, CinematicCameraSubModule.ModuleId, nameof(GameKeyConfig) + ".xml");
        protected static Version BinaryVersion => new Version(1, 1);

        public string ConfigVersion = BinaryVersion.ToString(2);

        protected override void CopyFrom(GameKeyConfig other)
        {
            base.CopyFrom(other);
            ConfigVersion = other.ConfigVersion;
        }

        protected override void UpgradeToCurrentVersion()
        {
            switch (ConfigVersion)
            {
                default:
                    Utility.DisplayMessage(Module.CurrentModule.GlobalTextManager.FindText("str_mission_library_hotkey_config_incompatible").ToString(), new TaleWorlds.Library.Color(1, 0, 0));
                    ResetToDefault();
                    Serialize();
                    goto case "1.1";
                case "1.1":
                    break;
            }

            ConfigVersion = BinaryVersion.ToString(2);
        }
    }
}
