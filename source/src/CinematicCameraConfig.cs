using RTSCamera;
using RTSCamera.Config.Basic;
using System;
using System.IO;
using System.Xml.Serialization;

namespace CinematicCamera
{
    public class CinematicCameraConfig : RTSCameraConfigBase<CinematicCameraConfig>
    {
        private static CinematicCameraConfig _instance;

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
        private static CinematicCameraConfig CreateDefault()
        {
            return new CinematicCameraConfig();
        }

        public bool PlayerInvulnerable = false;

        public float CameraFov = 65f;

        public bool RotateSmoothMode = false;

        //public float Zoom = 1.0f;

        public float SpeedFactor = 1.0f;

        public float VerticalSpeedFactor = 1.0f;

        public float DepthOfFieldDistance = 0;

        public float DepthOfFieldStart = 0;

        public float DepthOfFieldEnd = 0;


        public static CinematicCameraConfig Get()
        {
            if (_instance == null)
            {
                _instance = CreateDefault();
                _instance.SyncWithSave();
            }

            return _instance;
        }

        protected override XmlSerializer serializer => new XmlSerializer(typeof(CinematicCameraConfig));

        public override void ResetToDefault()
        {
            CopyFrom(CreateDefault());
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
        }
        protected override string SaveName => Path.Combine(SavePath, nameof(CinematicCameraConfig) + ".xml");
        protected override string[] OldNames { get; } = { Path.Combine(OldSavePath, "CinematicCameraConfig.xml") };
    }
}
