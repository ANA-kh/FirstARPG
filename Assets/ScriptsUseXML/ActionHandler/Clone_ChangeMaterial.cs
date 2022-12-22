using UnityEngine;
using XMLib.AM;

namespace XMLibGame
{
    [System.Serializable]
    [ActionConfig(typeof(Clone_ChangeMaterial))]
    public class Clone_ChangeMaterialConfig : HoldFrames
    {
        [Newtonsoft.Json.JsonIgnore] public GameObject Clone{get; set; }
    }
    
    public class Clone_ChangeMaterial : IActionHandler
    {
        public void Enter(ActionNode node)
        {
            var controller = (ActionMachineController)node.actionMachine.controller;
            var config = (Clone_ChangeMaterialConfig)node.config;
            config.Clone = GameObject.Instantiate(controller.Model, controller.Model.transform);
            config.Clone.transform.SetParent(null,true);
            foreach (var skinnedMeshRenderer in config.Clone.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                skinnedMeshRenderer.enabled = true;
                var materials = new Material[3]
                {
                    controller.GlowMaterial,
                    controller.GlowMaterial,
                    controller.GlowMaterial
                };
                skinnedMeshRenderer.materials = materials;
            }
        }

        public void Exit(ActionNode node)
        {
            var config = (Clone_ChangeMaterialConfig)node.config;
            GameObject.Destroy(config.Clone);
        }

        public void Update(ActionNode node, float deltaTime)
        {
        }
    }
}