using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FirstARPG.StateMachine.Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        public List<EnemyStateMachine> Enemies = new List<EnemyStateMachine>();
        public float AttackInterval = 5;
        private float _attackTimer = 0;

        private void Update()
        {
            _attackTimer += Time.deltaTime;
            if (_attackTimer > AttackInterval)
            {
                var machines = Enemies.Where(e =>
                    e.CheckState(typeof(EnemyChasingState)) || e.CheckState(typeof(EnemyAttackingState)));
                if (!machines.Any())
                {
                    var machine = Enemies[Random.Range(0, Enemies.Count)];
                    machine.ChasingPlayer();
                }
                _attackTimer -= AttackInterval;
            }
        }
    }
}