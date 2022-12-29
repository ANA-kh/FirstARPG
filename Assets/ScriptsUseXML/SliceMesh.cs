using EzySlice;
using UnityEngine;
using XMLib.AM;

namespace XMLibGame
{
    [System.Serializable]
    [ActionConfig(typeof(SliceMesh))]
    public class SliceMeshConfig : HoldFrames
    {
        public float power;
    }
    
    public class SliceMesh : IActionHandler
    {
        public void Enter(ActionNode node)
        {
            var controller = (ActionMachineController)node.actionMachine.controller;
            var CutPlane = controller.CutPlane;
            var hits = Physics.OverlapBox(CutPlane.position, new Vector3(5, 0.1f, 5), CutPlane.rotation, LayerMask.GetMask("Enemy"));
                
            foreach (var item in hits)
            {
                var meshGameObject = GetMeshGameObject(item);
                if (meshGameObject== null)
                {
                    continue;
                }
                var hull = SliceObject(meshGameObject,CutPlane);
                if (hull != null)
                {
                    GameObject bottom = hull.CreateLowerHull(meshGameObject);
                    GameObject top = hull.CreateUpperHull(meshGameObject);
                    AddHullComponents(bottom);
                    AddHullComponents(top);
                    GameObject.Destroy(meshGameObject);
                }
            }
            
            foreach (var particle in controller.SliceParticles)
            {
                particle.Play();
            }
        }

        public void Exit(ActionNode node)
        {
        }

        public void Update(ActionNode node, float deltaTime)
        {
        }
        
        private static GameObject GetMeshGameObject(Collider item)
        {
            if (item.GetComponent<MeshFilter>() == null)
            {
                var skinnedMesh = item.GetComponent<SkinnedMeshRenderer>();
                if (skinnedMesh)
                {
                    Mesh staticMesh = new Mesh();
                    GameObject goNewMesh = new GameObject();

                    skinnedMesh.BakeMesh(staticMesh);
                    goNewMesh.transform.position = skinnedMesh.transform.position;
                    goNewMesh.AddComponent<MeshFilter>().sharedMesh = staticMesh;
                    goNewMesh.AddComponent<MeshRenderer>().sharedMaterials = skinnedMesh.materials;
                    // goNewMesh.AddComponent<Rigidbody>().useGravity = true;
                    // goNewMesh.AddComponent<BoxCollider>();
                    GameObject.Destroy(item.transform.parent.gameObject);

                    return goNewMesh;
                }

                return null;
            }

            return item.gameObject;
        }

        public SlicedHull SliceObject(GameObject obj,Transform CutPlane, Material crossSectionMaterial = null)
        {
            // slice the provided object using the transforms of this object
            if (obj.GetComponent<MeshFilter>() == null)
            {
                return null;
            }


            return obj.Slice(CutPlane.position, CutPlane.up, crossSectionMaterial);
        }

        public void AddHullComponents(GameObject go)
        {
            go.layer = 9;
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            MeshCollider collider = go.AddComponent<MeshCollider>();
            collider.convex = true;

            //TODO 对上半部分和下半部的分别施加力
            rb.AddExplosionForce(100, go.transform.position, 20);
        }
    }
}