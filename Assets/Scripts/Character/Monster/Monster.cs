using System;
using System.Collections;
using System.Collections.Generic;
using FSM;
using GameData;
using UI;
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

        // 데미지 계산 --> 서버에 검증요청
        public void HpVerifyToServer(List<int> damages)
        {
            for (int i = 0; i < damages.Count; i++)
            {
                currentHp -= damages[i];
            }

            // TODO 만약 서버에서 검증에 성공하면 값 저장
            List<string> damagesToString = new List<string>();
            for (int i = 0; i < damages.Count; i++)
            {
                damagesToString.Add(damages[i].ToString());
            }

            _verifiedDamage.Enqueue(damagesToString);
        }

        private Queue<List<string>> _verifiedDamage;
        
        public void Damaged()
        {
            Animator.SetTrigger("damage");
            if (DamageEffectManager.Instance.GetDamageEffect()
                .TryGetComponent<DefaultDamageEffect>(out var damageEffect))
            {
                damageEffect.PositionUpdate(this.transform.position);
            }

            if (_verifiedDamage.Count > 0)
            {
                IEnumerator damageEffectCoroutine = damageEffect.DisplayDamage(
                    _verifiedDamage.Dequeue());
                DamageEffectManager.Instance.StartDamageDisplayCoroutine(damageEffectCoroutine);
            }

            else
            {
                throw new Exception("Cannot Verified Player Damage");
            }
        }

        public void WrapperFunction()
        {
            Invoke("Damaged", 0.3f);
        }
        
        public override void Init()
        {
            this.currentHp = monsterData.maxHp;
            _verifiedDamage = new Queue<List<string>>();
            Animator = GetComponent<Animator>();
            gameObject.SetActive(true);
            StateMachineInit();
        }

        public virtual void StateMachineInit() { }
    }
}
