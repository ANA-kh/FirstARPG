using UnityEngine;

namespace FSM
{
    public class FsmTest
    {
        public static void ContinueState()
        {
            Debug.Log(".");
        }
        public class Hero
        {
            HeroStateBase _heroStateBase = new HeroStateBase();
        }

        public class HeroStateBase : FsmState<Hero>
        {
            protected float Interval = 0.4f;
            protected float TickSecond = 0;
            public override void OnEnter(IFsm<Hero> fsm)
            {
                base.OnEnter(fsm);
                Debug.Log("--------------");
            }
        
            public override void OnLeave(IFsm<Hero> fsm, bool isShutdown)
            {
                base.OnLeave(fsm,isShutdown);
                TickSecond = 0;
                Debug.Log("---------------");
            }
            public override void OnUpdate(IFsm<Hero> fsm, float logicSeconds, float realSeconds)
            {
                base.OnUpdate(fsm, logicSeconds, realSeconds);
                if ((fsm.CurrentStateTime - TickSecond) > Interval)
                {
                    TickSecond = fsm.CurrentStateTime;
                    ContinueState();
                }
            }
        }
        
        public class HeroIdle: HeroStateBase
        {
            public override void OnEnter(IFsm<Hero> fsm)
            {
                base.OnEnter(fsm);
                Debug.Log("站立");
            }

            public override void OnUpdate(IFsm<Hero> fsm, float logicSeconds, float realSeconds)
            {
                base.OnUpdate(fsm, logicSeconds, realSeconds);
                if (Input.GetKey(KeyCode.W))
                {
                    ChangeState<HeroJump>(fsm);
                }
            }
        }
        public class HeroMove: HeroStateBase
        {
            public override void OnEnter(IFsm<Hero> fsm)
            {
                base.OnEnter(fsm);
                Debug.Log("移动");
            }
        }
        public class HeroJump: HeroStateBase
        {
            public float jumpSeconds = 2f;
            public override void OnEnter(IFsm<Hero> fsm)
            {
                base.OnEnter(fsm);
                Debug.Log("跳跃");
            }

            public override void OnUpdate(IFsm<Hero> fsm, float logicSeconds, float realSeconds)
            {
                base.OnUpdate(fsm, logicSeconds, realSeconds);
                if (fsm.CurrentStateTime > jumpSeconds)
                {
                    ChangeState<HeroIdle>(fsm);
                }

                if (Input.GetKey(KeyCode.S))
                {
                    ChangeState<HeroSmashDown>(fsm);
                }
            }
        }
        public class HeroSmashDown: HeroStateBase
        {
            public float hitSeconds = 2f;
            public override void OnEnter(IFsm<Hero> fsm)
            {
                base.OnEnter(fsm);
                Debug.Log("下劈");
            }

            public override void OnUpdate(IFsm<Hero> fsm, float logicSeconds, float realSeconds)
            {
                base.OnUpdate(fsm, logicSeconds, realSeconds);
                if (fsm.CurrentStateTime > hitSeconds)
                {
                    ChangeState<HeroIdle>(fsm);
                }
            }
        }
    }

    
    
}