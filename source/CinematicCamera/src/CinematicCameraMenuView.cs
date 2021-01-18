using MissionSharedLibrary.View;

namespace CinematicCamera
{
    public class CinematicCameraMenuView : MissionMenuViewBase
    {

        public CinematicCameraMenuView()
            : base(25, nameof(CinematicCameraMenuView))
        {
            this.GetDataSource = () => new CinematicCameraMenuVM(this.OnCloseMenu);
        }
    }
}
