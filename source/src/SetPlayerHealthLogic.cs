using RTSCamera.Event;
using TaleWorlds.MountAndBlade;

namespace CinematicCamera
{
    public class SetPlayerHealthLogic : MissionLogic
    {
        private readonly CinematicCameraConfig _config = CinematicCameraConfig.Get();

        public override void OnBehaviourInitialize()
        {
            base.OnBehaviourInitialize();

            Mission.OnMainAgentChanged += Mission_OnMainAgentChanged;
            MissionEvent.MainAgentWillBeChangedToAnotherOne += MainAgentWillBeChangedToAnotherOne;
        }

        public override void OnRemoveBehaviour()
        {
            base.OnRemoveBehaviour();

            Mission.OnMainAgentChanged -= Mission_OnMainAgentChanged;
            MissionEvent.MainAgentWillBeChangedToAnotherOne -= MainAgentWillBeChangedToAnotherOne;
        }

        private void MainAgentWillBeChangedToAnotherOne(Agent newAgent)
        {
            if (_config.PlayerInvulnerable)
                UpdateInvulnerable(false);
        }

        private void Mission_OnMainAgentChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (Mission.MainAgent != null)
            {
                if (_config.PlayerInvulnerable)
                    UpdateInvulnerable(true);
            }
        }

        public void UpdateInvulnerable(bool invulnerable)
        {
            if (Mission.MainAgent == null)
                return;
            var agent = Mission.MainAgent;
            agent.SetInvulnerable(invulnerable);
        }
    }
}
