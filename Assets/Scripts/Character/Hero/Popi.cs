using FSM;
using FSM.States;
using Managers;

namespace Character.Hero
{
    public class Popi : Hero
    {
        protected override void InitStateMachine()
        {
            StateMachine = new StateMachine<Hero>(this);
            var idle = StateMachine.CreateState(new HeroIdle("Idle", true));
            var run = StateMachine.CreateState(new HeroRun("Run", true));
            var attack = StateMachine.CreateState(new HeroAttack("Attack", false));

            var idleToRunTransition = StateMachine.CreateTransition("IdleToRun", idle, run);
            var runToIdleTransition = StateMachine.CreateTransition("RunToIdle", run, idle);
            var idleToAttackTransition = StateMachine.CreateTransition("IdleToAttack", idle, attack);

            StateMachine.CurrentState = idle;

            TransitionParameter runParam = new TransitionParameter("Run", ParameterType.Bool);
            StateMachine.AddTransitionCondition(idleToRunTransition,
                runParam, targetValue => (bool)targetValue);
            StateMachine.AddTransitionCondition(runToIdleTransition,
                runParam, targetValue => !(bool)targetValue);

            TransitionParameter attackParam = new TransitionParameter("Attack", ParameterType.Trigger);
            StateMachine.AddTransitionCondition(idleToAttackTransition, attackParam);
            InstanceId = gameObject.GetInstanceID();
            StateMachineManager.Instance.Register(InstanceId, StateMachine);
        }

        // TODO 애니메이션 이벤트 함수 코드 더 좋은 방법이 있는지 고민해보기
        #region AnimationEventFunctions
        // 효과음
        // 이펙트 (데미지 이펙트)
        public void PopiAttack() { }
        public void PopiMove() { }
        public void PopiIdle() { }
        #endregion
    }
}
