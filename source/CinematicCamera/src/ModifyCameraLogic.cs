using MissionLibrary.Controller.Camera;
using TaleWorlds.MountAndBlade;

namespace CinematicCamera
{
    public class ModifyCameraLogic : MissionLogic
    {
        private readonly CinematicCameraConfig _config = CinematicCameraConfig.Get();

        public override void OnBehaviourInitialize()
        {
            base.OnBehaviourInitialize();

            if (CameraController.Instance != null)
            {
                CameraController.Instance.ViewAngle = _config.CameraFov;
                CameraController.Instance.SmoothRotationMode = _config.RotateSmoothMode;
                UpdateDepthOfFieldParameters();
                UpdateDepthOfFieldDistance();
            }
        }

        public void UpdateFov()
        {
            if (CameraController.Instance == null)
                return;
            CameraController.Instance.ViewAngle = _config.CameraFov;
        }

        public void UpdateRotateSmoothMode()
        {
            if (CameraController.Instance == null)
                return;
            CameraController.Instance.SmoothRotationMode = _config.RotateSmoothMode;
        }

        public void UpdateSpeed()
        {
            if (CameraController.Instance == null)
                return;
            CameraController.Instance.MovementSpeedFactor = _config.SpeedFactor;
            CameraController.Instance.VerticalMovementSpeedFactor = _config.VerticalSpeedFactor;
        }

        public void UpdateDepthOfFieldDistance()
        {
            if (CameraController.Instance == null)
                return;
            CameraController.Instance.DepthOfFieldDistance = _config.DepthOfFieldDistance;
        }

        public void UpdateDepthOfFieldParameters()
        {
            if (CameraController.Instance == null)
                return;
            CameraController.Instance.DepthOfFieldStart = _config.DepthOfFieldStart;
            CameraController.Instance.DepthOfFieldEnd = _config.DepthOfFieldEnd;
        }

        //public void UpdateZoom()
        //{
        //    if (CameraController.Instance == null)
        //        return;
        //    CameraController.Instance.Zoom = _config.Zoom;
        //}
    }
}
