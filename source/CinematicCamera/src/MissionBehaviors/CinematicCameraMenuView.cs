using MissionSharedLibrary.View;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ScreenSystem;
using TaleWorlds.TwoDimension;

namespace CinematicCamera.MissionBehaviors
{
    public class CinematicCameraMenuView : MissionMenuViewBase
    {
        private static bool _willEndDraggingMode;
        private static bool _earlyDraggingMode;
        private static float _beginDraggingOffset;
        private static readonly float _beginDraggingOffsetThreshold = 100;
        private static bool _rightButtonDraggingMode;

        public CinematicCameraMenuView()
            : base(25, nameof(CinematicCameraMenuView))
        {
        }

        protected override MissionMenuVMBase GetDataSource()
        {
            return new CinematicCameraMenuVM(this.OnCloseMenu);
        }

        public override void OnMissionScreenTick(float dt)
        {
            if (IsActivated)
            {
                UpdateDragData();
                UpdateMouseVisibility();
                DataSource.RefreshValues();
            }
            base.OnMissionScreenTick(dt);
        }

        public override void ActivateMenu()
        {
            IsActivated = true;
            DataSource = GetDataSource();
            if (DataSource == null)
                return;
            GauntletLayer = new GauntletLayer(ViewOrderPriority) { IsFocusLayer = false };
            GauntletLayer.InputRestrictions.SetInputRestrictions();
            GauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
            _movie = GauntletLayer.LoadMovie(_movieName, DataSource);
            SpriteData spriteData = UIResourceManager.SpriteData;
            TwoDimensionEngineResourceContext resourceContext = UIResourceManager.ResourceContext;
            ResourceDepot uiResourceDepot = UIResourceManager.UIResourceDepot;
            spriteData.SpriteCategories["ui_saveload"]?.Load(resourceContext, uiResourceDepot);
            MissionScreen.AddLayer(GauntletLayer);
            ScreenManager.TrySetFocus(GauntletLayer);
            MissionState.Current.Paused = true;
        }

        public override void DeactivateMenu()
        {
            if (GauntletLayer.Input.IsKeyReleased(InputKey.RightMouseButton) && !_rightButtonDraggingMode
                || GauntletLayer.Input.IsHotKeyReleased("Exit"))
            {
                base.DeactivateMenu();
            }
        }

        protected override void OnCloseMenu()
        {
            IsActivated = false;
            GauntletLayer.InputRestrictions.ResetInputRestrictions();
            MissionScreen.RemoveLayer(GauntletLayer);
            DataSource.OnFinalize();
            DataSource = null;
            _movie = null;
            GauntletLayer = null;
            MissionState.Current.Paused = false;
        }

        private void UpdateDragData()
        {
            if (_willEndDraggingMode)
            {
                _willEndDraggingMode = false;
                EndDrag();
            }
            else if (GauntletLayer.Input.IsKeyReleased(InputKey.RightMouseButton))
            {
                if (_earlyDraggingMode || _rightButtonDraggingMode)
                    _willEndDraggingMode = true;
            }
            else
            {
                if (ShouldBeginEarlyDragging())
                {
                    BeginEarlyDragging();
                }
                else if (GauntletLayer.Input.IsKeyDown(InputKey.RightMouseButton))
                {
                    if (ShouldBeginDragging())
                    {
                        BeginDrag();
                    }
                    else if (_earlyDraggingMode)
                    {
                        float inputXRaw = MissionScreen.SceneLayer.Input.GetMouseMoveX();
                        float inputYRaw = MissionScreen.SceneLayer.Input.GetMouseMoveY();
                        _beginDraggingOffset += inputYRaw * inputYRaw + inputXRaw * inputXRaw;
                    }
                }
            }
        }
        private void UpdateMouseVisibility()
        {
            bool mouseVisibility = !_rightButtonDraggingMode && !_earlyDraggingMode;
            if (mouseVisibility != GauntletLayer.InputRestrictions.MouseVisibility)
            {
                GauntletLayer.InputRestrictions.SetInputRestrictions(mouseVisibility,
                    mouseVisibility ? InputUsageMask.All : InputUsageMask.Invalid);
            }
        }

        private static bool ShouldBeginDragging()
        {
            return _earlyDraggingMode && _beginDraggingOffset > _beginDraggingOffsetThreshold;
        }

        private bool ShouldBeginEarlyDragging()
        {
            return !_earlyDraggingMode && GauntletLayer.Input.IsKeyDown(InputKey.RightMouseButton);
        }

        private static void BeginEarlyDragging()
        {
            _earlyDraggingMode = true;
            _beginDraggingOffset = 0;
        }

        private static void EndEarlyDragging()
        {
            _earlyDraggingMode = false;
            _beginDraggingOffset = 0;
        }

        private static void BeginDrag()
        {
            EndEarlyDragging();
            _rightButtonDraggingMode = true;
        }

        private static void EndDrag()
        {
            EndEarlyDragging();
            _rightButtonDraggingMode = false;
        }
    }
}
