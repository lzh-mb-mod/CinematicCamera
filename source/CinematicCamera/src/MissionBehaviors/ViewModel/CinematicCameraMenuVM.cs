using MissionSharedLibrary.View;
using MissionSharedLibrary.View.ViewModelCollection.Basic;
using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace CinematicCamera
{
    public class CinematicCameraMenuVM : MissionMenuVMBase
    {
        private readonly CinematicCameraConfig _config = CinematicCameraConfig.Get();

        private readonly CinematicCameraLogic _setPlayerHealthLogic =
            Mission.Current.GetMissionBehavior<CinematicCameraLogic>();

        private NumericVM _verticalFov;
        //private NumericVM _zoom;

        private NumericVM _speedFactor;

        private NumericVM _verticalSpeedFactor;

        private NumericVM _depthOfFieldDistance, _depthOfFieldStart, _depthOfFieldEnd;

        private NumericVM _cameraSpeedLow, _cameraSpeedMiddle, _cameraSpeedHigh;

        private SelectorVM<SelectorItemVM> _colorGradeSelector;

        private SelectorVM<SelectorItemVM> _overlaySelector;

        public string PlayerInvulnerableString { get; } = GameTexts.FindText("str_cinematic_camera_player_invulnerable").ToString();
        public string ResetString { get; } = GameTexts.FindText("str_cinematic_camera_reset").ToString();

        public string ZoomString { get; } = GameTexts.FindText("str_cinematic_camera_zoom").ToString();

        public string RotateSmoothModeString { get; } = GameTexts.FindText("str_cinematic_camera_rotate_smooth_mode").ToString();

        public bool PlayerInvulnerable
        {
            get => _config.PlayerInvulnerable;
            set
            {
                if (_config.PlayerInvulnerable == value)
                    return;
                _config.PlayerInvulnerable = value;
                _setPlayerHealthLogic?.UpdateInvulnerable(_config.PlayerInvulnerable);
                OnPropertyChanged(nameof(PlayerInvulnerable));
            }
        }

        [DataSourceProperty]
        public SelectorVM<SelectorItemVM> ColorGradeSelector
        {
            get => this._colorGradeSelector;
            set
            {
                if (value == this._colorGradeSelector)
                    return;
                this._colorGradeSelector = value;
                this.OnPropertyChangedWithValue<SelectorVM<SelectorItemVM>>(value, nameof(ColorGradeSelector));
            }
        }

        [DataSourceProperty]
        public SelectorVM<SelectorItemVM> OverlaySelector
        {
            get => this._overlaySelector;
            set
            {
                if (value == this._overlaySelector)
                    return;
                this._overlaySelector = value;
                this.OnPropertyChangedWithValue<SelectorVM<SelectorItemVM>>(value, nameof(OverlaySelector));
            }
        }

        public NumericVM VerticalFov
        {
            get => _verticalFov;
            set
            {
                if (_verticalFov == value)
                    return;
                _verticalFov = value;
                OnPropertyChanged(nameof(VerticalFov));
            }
        }

        public void ResetFov()
        {
            VerticalFov.OptionValue = 65.0f;
        }

        //public NumericVM Zoom
        //{
        //    get => _zoom;
        //    set
        //    {
        //        if (_zoom == value)
        //            return;
        //        _zoom = value;
        //        OnPropertyChanged(nameof(Zoom));
        //    }
        //}

        //public void ResetZoom()
        //{
        //    Zoom.OptionValue = 1;
        //}

        public bool RotateSmoothMode
        {
            get => _config.RotateSmoothMode;
            set
            {
                if (_config.RotateSmoothMode == value)
                    return;
                _config.RotateSmoothMode = value;
                ModifyCameraHelper.UpdateRotateSmoothMode();
                OnPropertyChanged(nameof(RotateSmoothMode));
            }
        }

        public NumericVM SpeedFactor
        {
            get => _speedFactor;
            set
            {
                if (_speedFactor == value)
                    return;
                _speedFactor = value;
                OnPropertyChanged(nameof(SpeedFactor));
            }
        }

        public void ResetSpeedFactor()
        {
            SpeedFactor.OptionValue = 1.0f;
        }

        public NumericVM VerticalSpeedFactor
        {
            get => _verticalSpeedFactor;
            set
            {
                if (_verticalSpeedFactor == value)
                    return;
                _verticalSpeedFactor = value;
                OnPropertyChanged(nameof(VerticalSpeedFactor));
            }
        }

        public void ResetVerticalSpeedFactor()
        {
            VerticalSpeedFactor.OptionValue = 1.0f;
        }

        public NumericVM DepthOfFieldDistance
        {
            get => _depthOfFieldDistance;
            set
            {
                if (_depthOfFieldDistance == value)
                    return;
                _depthOfFieldDistance = value;
                OnPropertyChanged(nameof(DepthOfFieldDistance));
            }
        }

        public NumericVM DepthOfFieldStart
        {
            get => _depthOfFieldStart;
            set
            {
                if (_depthOfFieldStart == value)
                    return;
                _depthOfFieldStart = value;
                OnPropertyChanged(nameof(DepthOfFieldStart));
            }
        }

        public NumericVM DepthOfFieldEnd
        {
            get => _depthOfFieldEnd;
            set
            {
                if (_depthOfFieldEnd == value)
                    return;
                _depthOfFieldEnd = value;
                OnPropertyChanged(nameof(DepthOfFieldEnd));
            }
        }

        public NumericVM CameraSpeedLow
        {
            get => _cameraSpeedLow;
            set
            {
                if (_cameraSpeedLow == value)
                    return;
                _cameraSpeedLow = value;
                OnPropertyChanged(nameof(CameraSpeedLow));
            }
        }

        public NumericVM CameraSpeedMiddle
        {
            get => _cameraSpeedMiddle;
            set
            {
                if (_cameraSpeedMiddle == value)
                    return;
                _cameraSpeedMiddle = value;
                OnPropertyChanged(nameof(CameraSpeedMiddle));
            }
        }

        public NumericVM CameraSpeedHigh
        {
            get => _cameraSpeedHigh;
            set
            {
                if (_cameraSpeedHigh == value)
                    return;
                _cameraSpeedHigh = value;
                OnPropertyChanged(nameof(CameraSpeedHigh));
            }
        }

        public CinematicCameraMenuVM(Action closeMenu) : base(closeMenu)
        {
            VerticalFov = new NumericVM(GameTexts.FindText("str_cinematic_camera_vertical_fov").ToString(), _config.CameraFov, 1, 179, true,
                fov =>
                {
                    _config.CameraFov = fov;
                    ModifyCameraHelper.UpdateFov();
                });
            //Zoom = new NumericVM(GameTexts.FindText("str_cinematic_camera_zoom").ToString(), _config.Zoom, 0.01f, 10, false,
            //    zoom =>
            //    {
            //        _config.Zoom = zoom;
            //        ModifyCameraHelper.UpdateZoom();
            //    });
            SpeedFactor = new NumericVM(GameTexts.FindText("str_cinematic_camera_speed_factor").ToString(), _config.SpeedFactor, 0.01f,9.99f, false,
                factor =>
                {
                    _config.SpeedFactor = factor;
                    ModifyCameraHelper.UpdateSpeed();
                });
            VerticalSpeedFactor = new NumericVM(GameTexts.FindText("str_cinematic_camera_vertical_speed_factor").ToString(), _config.VerticalSpeedFactor, 0.01f, 9.99f, false,
                factor =>
                {
                    _config.VerticalSpeedFactor = factor;
                    ModifyCameraHelper.UpdateSpeed();
                });

            var scene = Mission.Current.Scene;
            DepthOfFieldDistance = new NumericVM(GameTexts.FindText("str_cinematic_camera_depth_of_field_distance").ToString(), _config.DepthOfFieldDistance, 0, 100f, false,
                v =>
                {
                    _config.DepthOfFieldDistance = v;
                    ModifyCameraHelper.UpdateDepthOfFieldDistance();
                    ModifyCameraHelper.UpdateDepthOfFieldParameters();
                });
            DepthOfFieldStart = new NumericVM(GameTexts.FindText("str_cinematic_camera_depth_of_field_start").ToString(), _config.DepthOfFieldStart, 0, 100f, false,
                v =>
                {
                    _config.DepthOfFieldStart = v;
                    ModifyCameraHelper.UpdateDepthOfFieldParameters();
                });
            DepthOfFieldEnd = new NumericVM(GameTexts.FindText("str_cinematic_camera_depth_of_field_End").ToString(), _config.DepthOfFieldEnd, 0, 1000f, false,
                v =>
                {
                    _config.DepthOfFieldEnd = v;
                    ModifyCameraHelper.UpdateDepthOfFieldParameters();
                });
            CameraSpeedLow = new NumericVM(GameTexts.FindText("str_cinematic_camera_camera_speed_low").ToString(), _config.CameraSpeedLow, 0.01f, 9.99f, false,
                v =>
                {
                    _config.CameraSpeedLow = v;
                });
            CameraSpeedMiddle = new NumericVM(GameTexts.FindText("str_cinematic_camera_camera_speed_medium").ToString(), _config.CameraSpeedMedium, 0.01f, 9.99f, false,
                v =>
                {
                    _config.CameraSpeedMedium = v;
                });
            CameraSpeedHigh = new NumericVM(GameTexts.FindText("str_cinematic_camera_camera_speed_high").ToString(), _config.CameraSpeedHigh, 0.01f, 9.99f, false,
                v =>
                {
                    _config.CameraSpeedHigh = v;
                });
            RefreshValues();
            Mission.Current.Scene.SetPhotoModeOn(true);
        }

        public override void RefreshValues()
        {
            List<string> list1 = new List<string>();
            string allColorGradeNames = Mission.Current.Scene.GetAllColorGradeNames();
            string[] separator1 = new string[1] { "*/*" };
            foreach (string variation in allColorGradeNames.Split(separator1, StringSplitOptions.RemoveEmptyEntries))
            {
                string str = GameTexts.FindText("str_photo_mode_color_grade", variation).ToString();
                list1.Add(str);
            }
            if (list1.Count == 0)
                list1.Add("Photo Mode Not Active");
            this.ColorGradeSelector = new SelectorVM<SelectorItemVM>((IEnumerable<string>)list1, Mission.Current.Scene.GetSceneColorGradeIndex(), new Action<SelectorVM<SelectorItemVM>>(this.OnColorGradeSelectionChanged));
            
            List<string> list2 = new List<string>();
            string allFilterNames = Mission.Current.Scene.GetAllFilterNames();
            string[] separator2 = new string[1] { "*/*" };
            foreach (string variation in allFilterNames.Split(separator2, StringSplitOptions.RemoveEmptyEntries))
            {
                string str = GameTexts.FindText("str_photo_mode_overlay", variation).ToString();
                list2.Add(str);
            }
            if (list2.Count == 0)
                list2.Add("Photo Mode Not Active");
            this.OverlaySelector = new SelectorVM<SelectorItemVM>((IEnumerable<string>)list2, Mission.Current.Scene.GetSceneFilterIndex(), new Action<SelectorVM<SelectorItemVM>>(this.OnOverlaySelectionChanged));
            OnPropertyChanged(nameof(PlayerInvulnerable));
            OnPropertyChanged(nameof(RotateSmoothMode));
            _verticalFov.OptionValue = _config.CameraFov;
            _speedFactor.OptionValue = _config.SpeedFactor;
            _verticalSpeedFactor.OptionValue = _config.VerticalSpeedFactor;
            _depthOfFieldDistance.OptionValue = _config.DepthOfFieldDistance;
            _depthOfFieldStart.OptionValue = _config.DepthOfFieldStart;
            _depthOfFieldEnd.OptionValue = _config.DepthOfFieldEnd;
        }

        public override void CloseMenu()
        {
            _config.Serialize();

            if (OverlaySelector.SelectedIndex == 0)
            {
                Mission.Current.Scene.SetPhotoModeOn(false);
            }
            base.CloseMenu();
        }
        private void OnColorGradeSelectionChanged(SelectorVM<SelectorItemVM> obj)
        {
            if (Mission.Current.Scene.GetSceneColorGradeIndex() == obj.SelectedIndex)
                return;
            Mission.Current.Scene.SetSceneColorGradeIndex(obj.SelectedIndex);
        }
        private void OnOverlaySelectionChanged(SelectorVM<SelectorItemVM> obj)
        {
            if (Mission.Current.Scene.GetSceneFilterIndex() == obj.SelectedIndex)
                return;
            int num = Mission.Current.Scene.SetSceneFilterIndex(obj.SelectedIndex);
            if (num < 0)
                return;
            this.ColorGradeSelector.SelectedIndex = num;
        }
    }
}
