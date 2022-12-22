using System.Collections;
using System.Collections.Generic;
using FirstARPG.Attributes;
using FirstARPG.Miscs;
using UnityEngine;

namespace FirstARPG.Combat
{
    public class WeaponDamage : MonoBehaviour
    {
        private int _damage;
        private float _knockBack;

        private readonly List<Collider> _alreadyCollidedWith = new List<Collider>();

        public GameObject Owner { get; set; }

        private void OnEnable()
        {
            _alreadyCollidedWith.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == Owner)
            {
                return;
            }

            if (_alreadyCollidedWith.Contains(other))
            {
                return;
            }

            _alreadyCollidedWith.Add(other);

            if (other.TryGetComponent<Health>(out Health health))
            {
                health.TakeDamage(Owner,_damage);
                Debug.Log("--------------------TakeDamage");
            }

            if (other.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
            {
                Vector3 direction = (other.transform.position - Owner.transform.position).normalized;
                forceReceiver.AddForce(direction * _knockBack);
            }
        }

        public void SetAttack(int damage, float knockback)
        {
            this._damage = damage;
            this._knockBack = knockback;
        }
    }
}