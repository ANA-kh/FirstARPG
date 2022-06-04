using System;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class FsmTestMono :MonoBehaviour
    {
        private Fsm<FsmTest.Hero> _heroFsm;
        private void Start()
        {
            FsmTest.Hero superHero = new FsmTest.Hero();
            List<FsmState<FsmTest.Hero>> heroStateBases = new List<FsmState<FsmTest.Hero>>();
            heroStateBases.Add(new FsmTest.HeroIdle());
            heroStateBases.Add(new FsmTest.HeroJump());
            heroStateBases.Add(new FsmTest.HeroSmashDown());
            _heroFsm = Fsm<FsmTest.Hero>.Create("heroFsm", superHero, heroStateBases);
            _heroFsm.Start<FsmTest.HeroIdle>();
        }

        private void Update()
        {
            _heroFsm.Update(Time.deltaTime,Time.unscaledDeltaTime);
        }
    }
}