using FSM;
using GameData;
using UnityEngine;

namespace Character.Monster
{
    public class Monster : CharacterBase
    {
        public Animator Animator;
        public MonsterData monsterData;
        public int currentHp;
        public StateMachine<Monster> StateMachine;
        public int InstanceId;

        public void Die()
        {
            gameObject.SetActive(false);
        }

        public void Damaged()
        {
            Animator.SetTrigger("damage");
        }

        public void WrapperFunction()
        {
            Invoke("Damaged", 0.3f);
        }
        
        public override void Init()
        {
            this.currentHp = monsterData.maxHp;
            Animator = GetComponent<Animator>();
            gameObject.SetActive(true);
            StateMachineInit();
        }

        public virtual void StateMachineInit() { }
    }
}
