using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class CoolTimeManager : MonoBehaviour
    {
        public static CoolTimeManager Instance { get; private set; }
        private CoolTimeManager() { }

        private Dictionary<string, float> coolTimes;
        private Dictionary<string, float> lastUsedTimes;
        
        [Serializable]
        public class CoolTimeFinishedEvent : UnityEvent<string> { }
    
        public CoolTimeFinishedEvent OnCoolTimeFinished;
        
        private void Awake()
        {
            Instance = this;
            coolTimes = new Dictionary<string, float>();
            lastUsedTimes = new Dictionary<string, float>();
        }

        public void RegisterCoolTime(string id, float coolTime)
        {
            if (coolTime < 0)
            {
                throw new ArgumentException("Cool time cannot be negative.", nameof(coolTime));
            }

            if (coolTimes.ContainsKey(id))
            {
                throw new ArgumentException($"Cool time item with id '{id}' is already registered.", "id");
            }

            coolTimes.Add(id, coolTime);
            lastUsedTimes.Add(id, 0f);
        }

        public void Use(string id)
        {
            if (!coolTimes.ContainsKey(id))
            {
                throw new ArgumentException($"No cool time item with id '{id}' is registered.", "id");
            }

            lastUsedTimes[id] = Time.time;
        }

        public bool IsCoolTimeFinished(string id)
        {
            if (!coolTimes.ContainsKey(id))
            {
                throw new ArgumentException($"No cool time item with id '{id}' is registered.", "id");
            }

            if (Time.time >= lastUsedTimes[id] + coolTimes[id])
            {
                // 쿨타임이 완료되면 이벤트 발생
                OnCoolTimeFinished?.Invoke(id);
                return true;
            }

            return false;
        }
    }
}
