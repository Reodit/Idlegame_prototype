using Character.Monster;
using UnityEngine;

namespace FSM.States
{
    public class MonsterIdle : IState<Monster>
    {
        public string StateName { get; set; }
        public bool RequiresUpdate { get; set; }

        public MonsterIdle(string stateName, bool requiresUpdate)
        {
            StateName = stateName;
            RequiresUpdate = requiresUpdate;
        }
        
        public void Enter(Monster owner)
        {
            { Debug.Log("Entering state : MonsterIdle"); }
        }

        public void Execute(Monster owner)
        {
            { Debug.Log("Execute state : MonsterIdle"); }
        }

        public void Exit(Monster owner)
        {
            { Debug.Log("Exit state : MonsterIdle"); }
        }
    }

    public class MonsterRun : IState<Monster>
    {
        public string StateName { get; set; }
        public bool RequiresUpdate { get; set; }

        public MonsterRun(string stateName, bool requiresUpdate)
        {
            StateName = stateName;
            RequiresUpdate = requiresUpdate;
        }

        public void Enter(Monster owner)
        {
            { Debug.Log("Entering state : MonsterRun"); }
        }

        public void Execute(Monster owner)
        {
            { Debug.Log("Execute state : MonsterRun"); }
        }

        public void Exit(Monster owner)
        {
            { Debug.Log("Exit state : MonsterRun"); }
        }
    }

    public class MonsterAttack : IState<Monster>
    {
        public string StateName { get; set; }
        public bool RequiresUpdate { get; set; }

        public MonsterAttack(string stateName, bool requiresUpdate)
        {
            StateName = stateName;
            RequiresUpdate = requiresUpdate;
        }

        public void Enter(Monster owner)
        {
            { Debug.Log($"Entering state : MonsterAttack : "); }
        }

        public void Execute(Monster owner)
        {
            { Debug.Log($"Execute state : MonsterAttack : "); }
        }

        public void Exit(Monster owner)
        {
            { Debug.Log($"Exit state : MonsterAttack : "); }
        }
    }
    
    public class MonsterDie : IState<Monster>
    {
        public string StateName { get; set; }
        public bool RequiresUpdate { get; set; }

        private float timeElapsed = 0f;
        private float delay = 0.5f; 
        private bool hasDelayedFunctionExecuted = false;
        
        public MonsterDie(string stateName, bool requiresUpdate)
        {
            StateName = stateName;
            RequiresUpdate = requiresUpdate;
        }

        public void Enter(Monster owner)
        {        
            // 상태 진입 시 변수 초기화
            timeElapsed = 0f;
            hasDelayedFunctionExecuted = false;
            { Debug.Log($"Entering state : MonsterDie : "); }
        }

        public void Execute(Monster owner)
        {
            if (!hasDelayedFunctionExecuted)
            {
                timeElapsed += Time.deltaTime;

                if (timeElapsed >= delay)
                {
                    owner.Die();
                    hasDelayedFunctionExecuted = true;
                }
            }
            
            { Debug.Log($"Execute state : MonsterDie : "); }
        }

        public void Exit(Monster owner)
        {
            owner.currentHp = owner.monsterData.maxHp;
            owner.gameObject.SetActive(true);
            { Debug.Log($"Exit state : MonsterDie : "); }
        }

    }
    
    public class MonsterDamaged : IState<Monster>
    {
        public string StateName { get; set; }
        public bool RequiresUpdate { get; set; }

        public MonsterDamaged(string stateName, bool requiresUpdate)
        {
            StateName = stateName;
            RequiresUpdate = requiresUpdate;
        }

        public void Enter(Monster owner)
        {
            { Debug.Log($"Entering state : MonsterDamaged"); }
        }

        public void Execute(Monster owner)
        {
            owner.WrapperFunction();
            { Debug.Log($"Execute state : MonsterDamaged"); }
        }

        public void Exit(Monster owner)
        {
            { Debug.Log($"Exit state : MonsterDamaged"); }
        }
    }
}
