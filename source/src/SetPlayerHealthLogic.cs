using RTSCamera;
using TaleWorlds.MountAndBlade;

namespace CinematicCamera
{
    public class SetPlayerHealthLogic : MissionLogic
    {
        private CinematicCameraConfig _config = CinematicCameraConfig.Get();
        private ControlTroopLogic _controlTroopLogic;

        public override void OnBehaviourInitialize()
        {
            base.OnBehaviourInitialize();

            Mission.OnMainAgentChanged += Mission_OnMainAgentChanged;
            _controlTroopLogic = Mission.GetMissionBehaviour<ControlTroopLogic>();
            _controlTroopLogic.MainAgentWillBeChangedToAnotherOne += MainAgentWillBeChangedToAnotherOne;
        }

        public override void OnRemoveBehaviour()
        {
            base.OnRemoveBehaviour();

            Mission.OnMainAgentChanged -= Mission_OnMainAgentChanged;
        }

        private void MainAgentWillBeChangedToAnotherOne()
        {
            if (_config.PlayerInvulnerable)
                UpdateHealth(false);
        }

        private void Mission_OnMainAgentChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (Mission.MainAgent != null)
            {
                if (_config.PlayerInvulnerable)
                    UpdateHealth(true);
            }
        }

        public void UpdateHealth(bool invulnerable)
        {
            if (Mission.MainAgent == null)
                return;
            var agent = Mission.MainAgent;
            if (invulnerable)
            {
                agent.HealthLimit = 100000;
                agent.Health = 100000;
            }
            else
            {
                agent.HealthLimit = agent.Character.HitPoints;
                agent.Health = agent.HealthLimit;
                if (agent.AgentDrivenProperties != null)
                {
                    MissionGameModels.Current.AgentStatCalculateModel.InitializeAgentStats(agent, agent.SpawnEquipment, agent.AgentDrivenProperties, (AgentBuildData)null);
                }
            }
            agent.UpdateAgentProperties();
        }
    }
}
