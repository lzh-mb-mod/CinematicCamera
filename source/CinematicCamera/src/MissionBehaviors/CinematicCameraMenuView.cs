using MissionSharedLibrary.View;

namespace CinematicCamera
{
    public class CinematicCameraMenuView : MissionMenuViewBase
    {

        public CinematicCameraMenuView()
            : base(25, nameof(CinematicCameraMenuView))
        {
        }

        protected override MissionMenuVMBase GetDataSource()
        {
            return  new CinematicCameraMenuVM(this.OnCloseMenu);
        }
    }
}
