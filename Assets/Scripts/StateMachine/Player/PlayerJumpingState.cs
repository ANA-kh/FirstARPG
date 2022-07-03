using System;
using FirstARPG.Player;

namespace FirstARPG.StateMachine
{
    internal class PlayerJumpingState : PlayerBaseState
    {
        private ParkourController _parkourController;
        public PlayerJumpingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
            _parkourController = stateMachine.GetComponent<ParkourController>();
            _parkourController.RotationDamping = stateMachine.RotationDamping;
        }

        public override void Enter()
        {
            _parkourController.PerformAction();
        }

        public override void Tick(float deltaTime)
        {
            if (!_parkourController.InAction)
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            }
        }

        public override void Exit()
        {
            
        }
    }
}