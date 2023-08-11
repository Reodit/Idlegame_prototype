using System;
using System.Collections;
using System.Collections.Generic;
using Character.Monster;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class MonsterPooler : MonoBehaviour
{
    private IObjectPool<Monster> _monsterpool;
    public int maxPoolSize;
    public bool collectionChecks;
    public int defaultCapacity;

    public GameObject MonsterPrefab;
    
    private void Awake()
    {
        _monsterpool = new ObjectPool<Monster>(CreateMonsterPool, OnTakeFromPool, OnReturnedPool, OnDestroyPoolObject, collectionChecks, defaultCapacity, maxPoolSize);
    }

    private Monster CreateMonsterPool()
    {
        GameObject go = Instantiate(MonsterPrefab);
        Monster ms = go.AddComponent<Monster>();
        
        
        return ms;
    }

    private void OnTakeFromPool(Monster ms)
    {
        ms.gameObject.SetActive(true);
    }

    private void OnReturnedPool(Monster ms)
    {
        ms.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObject(Monster ms)
    {
        Destroy(ms.gameObject);
    }
    
}
