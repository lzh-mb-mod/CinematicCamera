using System;
using System.Collections.Generic;
using System.Text;
using EnhancedMission;

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
