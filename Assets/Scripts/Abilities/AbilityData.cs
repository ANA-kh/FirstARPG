using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstARPG.Abilities
{
    public class AbilityData : IAction
    {
        private GameObject _user;
        private Vector3 _targetedPoint;
        private IEnumerable<GameObject> _targets;
        private bool _cancelled = false;
    
        public AbilityData(GameObject user)
        {
            this._user = user;
        }
    
        public IEnumerable<GameObject> GetTargets()
        {
            return _targets;
        }
    
        public void SetTargets(IEnumerable<GameObject> targets)
        {
            this._targets = targets; 
        }
    
        public Vector3 GetTargetedPoint()
        {
            return _targetedPoint;
        }
    
        public void SetTargetedPoint(Vector3 targetedPoint)
        {
            this._targetedPoint = targetedPoint;
        }
    
        public GameObject GetUser()
        {
            return _user;
        }
    
        public void StartCoroutine(IEnumerator coroutine)
        {
            _user.GetComponent<MonoBehaviour>().StartCoroutine(coroutine);
        }
    
        public void Cancel()
        {
            _cancelled = true;
        }
    
        public bool IsCancelled()
        {
            return _cancelled;
        }
    }
}
