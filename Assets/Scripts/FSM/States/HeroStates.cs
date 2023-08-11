using Character.Hero;
using UnityEngine;

namespace FSM.States
{
    public class HeroIdle : IState<Hero>
    {
        public string StateName { get; set; }
        public bool RequiresUpdate { get; set; }

        public HeroIdle(string stateName, bool requiresUpdate)
        {
            StateName = stateName;
            RequiresUpdate = requiresUpdate;
        }
        public void Enter(Hero owner)
        {
            { Debug.Log("Entering state : Idle"); }
        }

        public void Execute(Hero owner)
        {
            { Debug.Log("Execute state : Idle"); }
        }

        public void Exit(Hero owner)
        {
            { Debug.Log("Exit state : Idle"); }
        }
    }

    public class HeroRun : IState<Hero>
    {
        public string StateName { get; set; }
        public bool RequiresUpdate { get; set; }

        public HeroRun(string stateName, bool requiresUpdate)
        {
            StateName = stateName;
            RequiresUpdate = requiresUpdate;
        }
        
        public void Enter(Hero owner)
        {
            owner.Move();
            { Debug.Log("Entering state : Run"); }
        }

        public void Execute(Hero owner)
        {
            { Debug.Log("Execute state : Run"); }
        }

        public void Exit(Hero owner)
        {
            owner.MoveOff();
            { Debug.Log("Exit state : Run"); }
        }
    }

    public class HeroAttack : IState<Hero>
    {
        public string StateName { get; set; }
        public bool RequiresUpdate { get; set; }

        public HeroAttack(string stateName, bool requiresUpdate)
        {
            StateName = stateName;
            RequiresUpdate = requiresUpdate;
        }
        
        public void Enter(Hero owner)
        {
            owner.Attack();
            { Debug.Log($"Entering state : Attack : "); }
        }

        public void Execute(Hero owner)
        {
            
            
            { Debug.Log($"Execute state : Attack : "); }
        }

        public void Exit(Hero owner)
        {
            { Debug.Log($"Exit state : Attack : "); }
        }
    }
}