using FSM;
using FSM.States;
using Managers;
using UnityEngine;

namespace Character.Monster
{
    public class Ghost : Monster
    {
        // 월드 캔버스는 최하위에 각각 위치하도록 
        public Canvas monsterWorldCanvas;
        
        public override void StateMachineInit()
        {
            base.StateMachineInit();
            StateMachine = new StateMachine<Monster>(this);
            var idle = StateMachine.CreateState(new MonsterIdle("Idle", true));
            var damaged = StateMachine.CreateState(new MonsterDamaged("Damaged", false));
            var die = StateMachine.CreateState(new MonsterDie("Die", true));

            /*var idleToRunTransition = StateMachine.CreateTransition("IdleToRun", idle, run);
            var runToIdleTransition = StateMachine.CreateTransition("RunToIdle", run, idle);*/
            var idleToDamagedTransition = StateMachine.CreateTransition("MonsterIdleToDamaged", idle, damaged);
            var idleToDieTransition = StateMachine.CreateTransition("MonsterIdleToDie", idle, die);
            var dieToIdleTransition = StateMachine.CreateTransition("MonsterDieToIdle", die, idle);
            
            StateMachine.CurrentState = idle;

            /*TransitionParameter runParam = new TransitionParameter("Run", ParameterType.Bool);
            StateMachine.AddTransitionCondition(idleToRunTransition,
                runParam, true, targetValue => (bool)targetValue);
            StateMachine.AddTransitionCondition(runToIdleTransition,
                runParam, false, targetValue => !(bool)targetValue);*/

            TransitionParameter damagedParam = new TransitionParameter("Damaged", ParameterType.Trigger);
            StateMachine.AddTransitionCondition(idleToDamagedTransition, damagedParam);
            
            TransitionParameter dieParam = new TransitionParameter("Die", ParameterType.Bool);
            StateMachine.AddTransitionCondition(idleToDieTransition, 
                    dieParam, targetValue => (bool)targetValue);
            StateMachine.AddTransitionCondition(dieToIdleTransition, 
                dieParam, targetValue => !(bool)targetValue);      

            InstanceId = gameObject.GetInstanceID();
            StateMachineManager.Instance.Register(InstanceId, StateMachine);
        }
        
        #region AnimationEventFunctions
        // 효과음
        // 이펙트 (데미지 이펙트)
        public void GhostAttack() { }
        public void GhostMove() { }
        public void GhostIdle() { }
        #endregion
    }
}
