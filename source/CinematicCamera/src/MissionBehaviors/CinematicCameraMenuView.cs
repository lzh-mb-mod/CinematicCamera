using CinematicCamera.Config.HotKey;
using MissionSharedLibrary.View;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

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
            : base(25, nameof(CinematicCameraMenuView), false, false)
        {
        }

        protected override MissionMenuVMBase GetDataSource()
        {
            return new CinematicCameraMenuVM(this.OnCloseMenu);
        }

        public override void OnMissionScreenTick(float dt)
        {
            if (CinematicCameraGameKeyCategory.GetKey(GameKeyEnum.OpenCinematicCameraMenu).IsKeyPressedInOrder())
            {
                if (IsActivated)
                {
                    DeactivateMenu();
                }
                else
                {
                    ActivateMenu();
                }
            }
            if (IsActivated)
            {
                UpdateDragData();
                UpdateMouseVisibility();
                DataSource.RefreshValues();

                if (GauntletLayer.Input.IsKeyReleased(InputKey.RightMouseButton) && !_rightButtonDraggingMode
                    || GauntletLayer.Input.IsHotKeyReleased("Exit"))
                    DeactivateMenu();
            }
        }
        public override bool OnEscape()
        {
            if (IsActivated)
            {
                DeactivateMenu();
                return true;
            }

            return base.OnEscape();
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
                    mouseVisibility ? InputUsageMask.MouseButtons : InputUsageMask.Invalid);
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
