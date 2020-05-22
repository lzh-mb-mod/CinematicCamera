using RTSCamera;
using TaleWorlds.MountAndBlade;

namespace CinematicCamera
{
    public class ModifyCameraLogic : MissionLogic
    {
        private FlyCameraMissionView _flyCameraMissionView;
        private readonly CinematicCameraConfig _config = CinematicCameraConfig.Get();

        public override void OnBehaviourInitialize()
        {
            base.OnBehaviourInitialize();

            _flyCameraMissionView = Mission.GetMissionBehaviour<FlyCameraMissionView>();
            _flyCameraMissionView.CameraViewAngle = _config.CameraFov;
            _flyCameraMissionView.CameraRotateSmoothMode = _config.RotateSmoothMode;
            UpdateDepthOfFieldParameters();
            UpdateDepthOfFieldDistance();
        }

        public void UpdateFov()
        {
            if (_flyCameraMissionView == null)
                return;
            _flyCameraMissionView.CameraViewAngle = _config.CameraFov;
        }

        public void UpdateRotateSmoothMode()
        {
            if (_flyCameraMissionView == null)
                return;
            _flyCameraMissionView.CameraRotateSmoothMode = _config.RotateSmoothMode;
        }

        public void UpdateSpeed()
        {
            if (_flyCameraMissionView == null)
                return;
            _flyCameraMissionView.CameraSpeedFactor = _config.SpeedFactor;
            _flyCameraMissionView.CameraVerticalSpeedFactor = _config.VerticalSpeedFactor;
        }

        public void UpdateDepthOfFieldDistance()
        {
            _flyCameraMissionView.DepthOfFieldDistance = _config.DepthOfFieldDistance;
        }

        public void UpdateDepthOfFieldParameters()
        {
            _flyCameraMissionView.DepthOfFieldStart = _config.DepthOfFieldStart;
            _flyCameraMissionView.DepthOfFieldEnd = _config.DepthOfFieldEnd;
        }

        //public void UpdateZoom()
        //{
        //    if (_flyCameraMissionView == null)
        //        return;
        //    _flyCameraMissionView.Zoom = _config.Zoom;
        //}
    }
}
