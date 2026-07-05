using MissionSharedLibrary.Config;
using MissionSharedLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using TaleWorlds.Core;

namespace CinematicCamera
{
    public class CinematicCameraConfig : MissionConfigBase<CinematicCameraConfig>
    {
        protected static Version BinaryVersion => new Version(1, 0);
        public string ConfigVersion { get; set; } = BinaryVersion.ToString();

        protected override void UpgradeToCurrentVersion()
        {
            switch (ConfigVersion?.ToString())
            {
                default:
                    Utility.DisplayLocalizedText("str_config_incompatible");
                    ResetToDefault();
                    Serialize();
                    break;
                case "1.0":
                    break;
            }
        }

        public bool PlayerInvulnerable = false;

        public float CameraFov = 65f;

        public bool RotateSmoothMode = true;

        //public float Zoom = 1.0f;

        public float SpeedFactor = 1.0f;

        public float VerticalSpeedFactor = 1.0f;

        public float DepthOfFieldDistance = 0;

        public float DepthOfFieldStart = 0;

        public float DepthOfFieldEnd = 0;

        public float CameraSpeedLow = 0.5f;

        public float CameraSpeedMedium = 1f;

        public float CameraSpeedHigh = 3f;

        public bool OrderUIInRegularScene = false;

        public string ActionName = "";

        public List<string> FavoriteActions = new List<string>();

        public string FacialAnimation = "";

        public List<string> FavoriteFacialAnimations = new List<string>();

        public bool IsCinematicCameraOptionShown = true;

        public bool IsActionOptionShown = true;

        public bool IsFacialAnimationOptionShown = true;

        public static void OnMenuClosed()
        {
            Get().Serialize();
        }
        
        protected override void CopyFrom(CinematicCameraConfig other)
        {
            ConfigVersion = other.ConfigVersion;
            PlayerInvulnerable = other.PlayerInvulnerable;
            CameraFov = other.CameraFov;
            RotateSmoothMode = other.RotateSmoothMode;
            //Zoom = other.Zoom;
            SpeedFactor = other.SpeedFactor;
            VerticalSpeedFactor = other.VerticalSpeedFactor;
            DepthOfFieldDistance = other.DepthOfFieldDistance;
            DepthOfFieldStart = other.DepthOfFieldStart;
            DepthOfFieldEnd = other.DepthOfFieldEnd;
            CameraSpeedLow = other.CameraSpeedLow;
            CameraSpeedMedium = other.CameraSpeedMedium;
            CameraSpeedHigh = other.CameraSpeedHigh;
            OrderUIInRegularScene = other.OrderUIInRegularScene;
            FavoriteActions = other.FavoriteActions.Distinct().ToList();
            if (FavoriteActions.IsEmpty())
            {
                FavoriteActions = new List<string>()
                {
                    "act_none",
                    "act_walk_idle_unarmed",
                    "act_crouch_walk_idle_unarmed",
                };
            }
            FacialAnimation = other.FacialAnimation;
            FavoriteFacialAnimations = other.FavoriteFacialAnimations.Distinct().ToList();
            IsCinematicCameraOptionShown = other.IsCinematicCameraOptionShown;
            IsActionOptionShown = other.IsActionOptionShown;
        }

        protected static string SavePathStatic { get; } = Path.Combine(ConfigPath.ConfigDir, CinematicCameraSubModule.ModuleId);
        protected static string OldSavePathStatic { get; } = Path.Combine(ConfigPath.ConfigDir, "RTSCamera");

        protected override string SaveName => Path.Combine(SavePathStatic, nameof(CinematicCameraConfig) + ".xml");

        protected override string OldSavePath => OldSavePathStatic;
        protected override string[] OldNames { get; } =
        {
            Path.Combine(OldSavePathStatic, "CinematicCameraConfig.xml"),
            Path.Combine(OldSavePathStatic, "CinematicCameraConfig.xml")
        };
    }
}
