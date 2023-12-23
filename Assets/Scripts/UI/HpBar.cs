using System;
using System.Collections;
using System.Collections.Generic;
using Character.Monster;
using Managers;
using UnityEngine;
using UnityEngine.UI;

// Hp Bar 관리는 캔버스 여러개와 ScreenSpace를 사용
// 각 Hp Bar는 pooling을 통해 재사용할 수 있도록 설계
// --> 한 스크린 안에 최대 몬스터 개수를 제한해야 함 
public class HpBar : MonoBehaviour
{
    private RectTransform rectTransform;

    [SerializeField] private Vector3 HpbarOffset;
    
    // TODO Mapping UI Prefabs
    [SerializeField] private GameObject HpbarPrefab;
    
    private const int MaxMonstersOnScreen = 10;
    private Monster[] _monsters;
    private Image[] HpBars;
    public bool isReadyMonsterGeneration;
    
    public void Init()
    {
        rectTransform = GetComponent<RectTransform>();
        
        //TODO 순서 보장이되는지 확인 필요
        _monsters = GameManager.Instance._monsterQueue.ToArray();
        isReadyMonsterGeneration = false;
        instanceCount = _monsters.Length < MaxMonstersOnScreen ? _monsters.Length : MaxMonstersOnScreen;
    }

    // 처음 HpBar를 만들때 최대 10개의 hp바를 만든다.
    // 앞의 몬스터가 죽으면, hp바의 위치를 11번째 몬스터의 위치로 옮긴다.
    public void CreateHpBar()
    {
        HpBars = new Image[MaxMonstersOnScreen];
        
        for (int i = 0; i < instanceCount; i++)
        {
            GameObject hpBar = Instantiate(HpbarPrefab, this.rectTransform);
            Vector3 screenPos = Camera.main.WorldToScreenPoint(_monsters[i].transform.position + HpbarOffset);
            hpBar.transform.position = screenPos;
            
            // TODO 자식오브젝트 가져오는 Utility 클래스 만들기 
            HpBars[i] = hpBar.transform.GetChild(1).GetComponent<Image>();
        }

        isReadyMonsterGeneration = true;
    }

    private static int instanceCount;
    // 몬스터가 이동하거나, 죽었을 때 Hp bar의 rect-transform update
    private void UpdateHpBar()
    {
        int pedding = 0;
        
        foreach (var e in _monsters)
        {
            if (e.currentHp <= 0)
            {
                pedding++;
            }
        }
        
        pedding = _monsters.Length - instanceCount < pedding ? _monsters.Length - instanceCount : pedding;

        if (instanceCount < MaxMonstersOnScreen)
        {
            pedding = 0;
        }
        
        for (int i = 0; i < instanceCount; i++)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(_monsters[i + pedding].transform.position + HpbarOffset); 
            HpBars[i].transform.parent.position = screenPos;

            HpBars[i].fillAmount = _monsters[i + pedding].monsterData.maxHp == 0
               ? 0 : (float)_monsters[i + pedding].currentHp / _monsters[i + pedding].monsterData.maxHp;

            HpBars[i].transform.parent.gameObject.SetActive(_monsters[i + pedding].gameObject.activeSelf);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isReadyMonsterGeneration)
        {
            UpdateHpBar();
        }
    }
}

