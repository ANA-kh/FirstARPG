using System;
using System.Diagnostics;
using FirstARPG.Miscs;
using FirstARPG.Saving;
using FirstARPG.Stats;
using FirstARPG.Utils;
using UnityEngine;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;

namespace FirstARPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenerationPercentage = 70;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
        }

        LazyValue<float> healthPoints;

        private bool _wasDeadLastFrame = false;
        private bool _isGod = false;
        public Action OnTakeDamage;
        public Action OnDie;

        private void Awake() {
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Start()
        {
            healthPoints.ForceInit();
        }

        private void OnEnable() {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        private void OnDisable() {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }

        public bool IsDead()
        {
            return healthPoints.value <= 0;
        }
        public void SetGod(bool isGod)
        {
            _isGod = isGod;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            if (_isGod)
            {
                return;
            }
            if (healthPoints.value <= 0)
            {
                return;
            }
            Debug.Log($"{gameObject.name} takeDamage: {damage}");
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);

            if(IsDead())
            {
                OnDie?.Invoke();
                AwardExperience(instigator);
            } 
            else
            {
                OnTakeDamage?.Invoke();
            }
            UpdateState();
        }

        public void Heal(float healthToRestore)
        {
            healthPoints.value = Mathf.Min(healthPoints.value + healthToRestore, GetMaxHealthPoints());
            UpdateState();
        }

        public float GetHealthPoints()
        {
            return healthPoints.value;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetPercentage()
        {
            return 100 * GetFraction();
        }

        public float GetFraction()
        {
            return healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void UpdateState()
        {
            //Animator animator = GetComponent<Animator>();
            if (!_wasDeadLastFrame && IsDead())
            {
                //animator.SetTrigger("die");
                GetComponent<ActionScheduler>()?.CancelCurrentAction();
            }

            if (_wasDeadLastFrame && !IsDead())
            {
                //animator.Rebind();
            }

            _wasDeadLastFrame = IsDead();
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        private void RegenerateHealth()
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
            healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
        }

        public object CaptureState()
        {
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float) state;
            
            UpdateState();
        }
    }
}