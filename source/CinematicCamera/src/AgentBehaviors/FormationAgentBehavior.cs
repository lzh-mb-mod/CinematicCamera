using SandBox.Missions.AgentBehaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinematicCamera.src.AgentBehaviors
{
    public class FormationAgentBehavior : AgentBehavior
    {
        public FormationAgentBehavior(AgentBehaviorGroup behaviorGroup) : base(behaviorGroup)
        {
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            OwnerAgent.DisableScriptedMovement();
            OwnerAgent.ClearTargetFrame();
            OwnerAgent.SetMaximumSpeedLimit(-1, false);
        }

        public override void Tick(float dt, bool isSimulation)
        {
            base.Tick(dt, isSimulation);

            //if (OwnerAgent != null)
            //{
            //    if (OwnerAgent.WalkMode)
            //    {
            //        OwnerAgent.SetMaximumSpeedLimit(OwnerAgent.CrouchMode ? OwnerAgent.Monster.CrouchWalkingSpeedLimit : OwnerAgent.Monster.WalkingSpeedLimit, false);
            //    }
            //}
        }

        protected override void OnDeactivate()
        {
            base.OnDeactivate();

            if (OwnerAgent.Formation != null)
                CinematicCameraLogic.SetAgentFormation(OwnerAgent, null);
        }
        public override string GetDebugInfo()
        {
            return "FormationAgentBehavior";
        }
    }
}
