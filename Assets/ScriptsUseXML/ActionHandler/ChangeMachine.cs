using XMLib.AM;

namespace XMLibGame
{
    
    [System.Serializable]
    [ActionConfig(typeof(ChangeMachine))]
    public class ChangeMachineConfig : HoldFrames
    {
        public int actionMachineIndex;
    }
    public class ChangeMachine : IActionHandler
    {
        public void Enter(ActionNode node)
        {
            var controller = (ActionMachineController)node.actionMachine.controller;
            var config = (ChangeMachineConfig)node.config;
            
            controller.ChangeActionMachine(config.actionMachineIndex);
        }

        public void Exit(ActionNode node)
        {
            
        }

        public void Update(ActionNode node, float deltaTime)
        {
            
        }
    }
}