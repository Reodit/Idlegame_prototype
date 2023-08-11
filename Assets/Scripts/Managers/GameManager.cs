using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Character.Hero;
using Character.Monster;
using UnityEngine;
using GameData;
using Utilities;
using Constants;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        #region MonsterRespawn
        // TODO Stage 정보 불러오기
        [SerializeField] private int currentPlayerStageID;
        
        [SerializeField] private Transform monsterRespawnPoint;
        [SerializeField] private float respawnSpacing;
        #endregion

        #region GameDatas
        private Dictionary<int, MonsterData> _monsterDictionary;
        private Dictionary<int, StageData> _stageDictionary;
        private Dictionary<int, HeroData> _heroDictionary; 
        public UserData _userData { get; private set; }

        #endregion

        #region Caching Datas
        public StageData CurrentStage { get; private set; }
        public Queue<Monster> _monsterQueue { get; private set; }
        public Vector3 DefaultMonsterRespawnPoint { get; private set; }

        #endregion

        #region Singleton
        public static GameManager Instance { get; private set; }
        private GameManager() { }
        #endregion

        public Hero[] Heroes { get; private set; }
        public LayerWrapper lw;

        // TODO 테스트를 위한 임시 리소스
        public GameObject GoldPrefab;

        #region UI

        public event Action<UserData> OnUserDataChanged;
        #endregion
        
        public float monsterSpeed = 5f;
        private void Awake()
        {
            Instance = this;
            DefaultMonsterRespawnPoint = monsterRespawnPoint.position;
        }

        private void Start()
        {
            isStageEnd = false;
            Sequence();
        }

        public bool isInfiniteStage;
        public bool isStageEnd;
        private void Update()
        {
            if (isInfiniteStage && isStageEnd)
            {
                isStageEnd = false;
                StartCoroutine(StartBattle());
            }
        }

        public void ChangesUserGold(Monster monster)
        {
            var userData = _userData;
            userData.playerMoney += monster.monsterData.rewardGold;
            _userData = userData;
            OnUserDataChanged?.Invoke(userData);
        }
        
        private IEnumerator StartBattle()
        {
            var monsterQueueCopy = new Queue<Monster>(_monsterQueue);

            while (monsterQueueCopy.TryDequeue(out var monster))
            {
                yield return StartCoroutine(MoveToNextMonster(monster));
            }

            yield return new WaitForSeconds(1f);
            
            ResetStage();
        }
        
        private IEnumerator MoveToNextMonster(Monster ms)
        {
            // Get Target's X Pos --> Move To dst X
            Vector3 currentMonsterPos = ms.transform.position;
            Vector3 currentMonsterSpawnPos = monsterRespawnPoint.position;

            float dstValue = Mathf.Abs(DefaultMonsterRespawnPoint.x - currentMonsterPos.x);
            float dstXPos = currentMonsterSpawnPos.x - dstValue;
            Vector3 target = new Vector3(dstXPos, currentMonsterSpawnPos.y, currentMonsterSpawnPos.z);
            
            // TODO Animation Transition 방법 고민
            // Co-routine 안에서 animation condition을 변경할 경우, 애니메이션 프레임이 밀리는 경우가 발생.. 
            foreach (var e in Heroes)
            {
                e.StateMachine.SetBool("Run", true);
            }

            yield return null;
            
            // TODO Animation 주기 확인 필요
            while (Vector3.Distance(currentMonsterSpawnPos, target) > float.Epsilon)
            {
                lw.OffsetUpdate();
                
                currentMonsterSpawnPos = Vector3.MoveTowards(currentMonsterSpawnPos,
                    target, 
                    monsterSpeed * Time.deltaTime);

                monsterRespawnPoint.position = currentMonsterSpawnPos;
                
                yield return null;
            }

            foreach (var e in Heroes)
            {
                e.StateMachine.SetBool("Run", false);
            }
            
            monsterRespawnPoint.position = target;
            
            // TODO AUTO mode 구분
            
            CombatSystemClient.ms = ms;
            while (ms.currentHp > 0)
            {
                foreach (var e in Heroes)
                {
                    if (e.IsCoolTimeFinished())
                    {
                        e.StateMachine.SetTrigger("Attack");
                        // TODO 계산은 따로 Task로 분리 (DB 클래스 작성 후 분리)
                    }
                }

                yield return null;
            }

            yield return null;
        }
        
        // 스테이지 정보 갱신 
        private void LoadStage(int stageID)
        {
            // TODO 이전 플레이 기록이 남아 있다면, 플레이어의 스테이지 불러오기 

            CurrentStage = _stageDictionary[stageID];
            
            // TODO Initialize Stage ex) UI.. Background.. etc..
        }

        public SkillData skilldata = new SkillData();
        private void Sequence()
        {
            // 1. 스테이지 & 몬스터 생성 페이즈
            // json 파일 (Stage Data & Monster Data) 처음에 한번 읽어오기
            // 캐릭터가 현재 어느 스테이지인지 알아야 함.
            // 스테이지 생성 --> 몬스터 생성 (몬스터 정보, 다음 몬스터와 간격을 이용해서 처음 한번 Instantiate)
            // 생성한 몬스터 Queue에 Enqueue
            
            // 2. 전투 페이즈
            // 몬스터 --> 전투 지점으로 이동
            // 플레이어가 공격할 때마다 HP 계산
            // 몬스터 사망 시 -- 애니메이션 재생 -- 다음 몬스터 전투지점으로 이동 -- 애니메이션이 끝난 후 SetActive false --> 끝으로 이동 (스테이지가 끝난 후 SetActive true)

            // Data Load
            DataUtility.LoadData(Path.Combine(PathConstants.GameDataFolder, PathConstants.StageDataFolder), out _stageDictionary);
            DataUtility.LoadData(Path.Combine(PathConstants.GameDataFolder, PathConstants.MonsterDataFolder), out _monsterDictionary);
            DataUtility.LoadData(Path.Combine(PathConstants.GameDataFolder, PathConstants.HeroDataFolder), out _heroDictionary);
            _userData = DataUtility.LoadData<UserData>(Path.Combine(PathConstants.GameDataFolder, PathConstants.UserDataFolder));

            // Stage Setup
            LoadStage(currentPlayerStageID);
            
            // TODO UserData는 Dictionary일 필요 없음 
            HeroInstancing(_userData);
            MonsterGeneration(currentPlayerStageID);
            
            // TODO UI 초기화 (임시처리)
            HpBar hpBar = FindObjectOfType<HpBar>();
            hpBar.Init();
            hpBar.CreateHpBar();

            // AutoBattle
            StartCoroutine(StartBattle());
            
            // 스테이지 안에 남아있는 적이 없다면 스테이지 다시 시작
            // StageReset
        }
        
        private void HeroInstancing(UserData userData)
        {
            // TODO 영웅 배치 UI 작성 전 임시 배치 처리
            for (int i = 0; i < userData.userHeroes.Count; i++)
            {
                int heroIdx = _heroDictionary[userData.userHeroes[i]].id;
                string heropath = Path.Combine(PathConstants.PrefabFolder, _heroDictionary[heroIdx].prefabName);

                GameObject hero = Instantiate(
                    Resources.Load<GameObject>(heropath),
                    GameObject.Find($"Character Spot_{i}").transform,
                    true);

                string heroTypeString = "Character.Hero." + _heroDictionary[heroIdx].prefabName;
                
                Type heroType = Type.GetType(heroTypeString);
                
                hero.transform.localPosition = Vector3.zero;
                hero.name = _heroDictionary[heroIdx].heroName;
                Hero heroComponent = hero.AddComponent(heroType) as Hero;
                
                System.Diagnostics.Debug.Assert(heroComponent != null, nameof(heroComponent) + " != null");
                heroComponent.heroData = new HeroData(_heroDictionary[heroIdx]);
                heroComponent.cooldownTest = _heroDictionary[heroIdx].skills[0].cooldownDuration;
                heroComponent.Init();
            }
            
            Heroes = FindObjectsOfType<Hero>();
        }
        
        // 스테이지 안에 남아있는 적이 없다면 스테이지 다시 시작
        private void ResetStage()
        {
            // Reset background ==> 할 필요 있나?
            // Reset Positions (Characters, Monsters)
            monsterRespawnPoint.position = DefaultMonsterRespawnPoint;
            
            // Regen Monsters 
            // Reset MonsterStats
            foreach (var e in _monsterQueue)
            {
                e.StateMachine.SetBool("Die", false);
            }

            isStageEnd = true;
        }
        
        private void MonsterGeneration(int stageID)
        {
            StageData stageData = _stageDictionary[stageID];

            // Queue Init
            _monsterQueue ??= new Queue<Monster>();

            if (_monsterQueue.Count > 0)
            {
                _monsterQueue.Clear();
            }

            Vector3 relativeCoordinates = monsterRespawnPoint.position; 
            
            for (int i = 0; i < stageData.monsterSequence.Length; i++)
            {
                int monsterID = stageData.monsterSequence[i].monsterID;
                MonsterData monsterData = _monsterDictionary[monsterID];
                string monsterPrefabPath = Path.Combine(PathConstants.PrefabFolder, _monsterDictionary[monsterID].prefabName);

                string monsterTypeString = "Character.Monster." + _monsterDictionary[monsterID].prefabName;
                Type monsterType = Type.GetType(monsterTypeString);

                // monster Instantiate
                GameObject monsterObject = Instantiate(Resources.Load<GameObject>(monsterPrefabPath), monsterRespawnPoint, true);
                Monster monsterComponent = monsterObject.AddComponent(monsterType) as Monster;
                monsterComponent.monsterData = monsterData;
                monsterComponent.Init();
                // Enqueue Data
                _monsterQueue.Enqueue(monsterComponent);
                
                // Reposition monsters
                monsterObject.transform.position = relativeCoordinates + new Vector3(stageData.monsterSequence[i].monsterSpacing * respawnSpacing, 0f, 0f);
                relativeCoordinates.x = monsterObject.transform.position.x;
            }
        }
    }
}

