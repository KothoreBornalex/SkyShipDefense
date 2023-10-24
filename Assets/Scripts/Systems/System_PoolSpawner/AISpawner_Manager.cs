using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using static AI_Class;

public class AISpawner_Manager : MonoBehaviour
{

    #region Classes & Enums Set Up
    
    [System.Serializable]
    public class PoolClass
    {
        public SoldiersEnum _poolType;
        public ObjectPool<PooledObject> _pool;
    }

    [System.Serializable]   
    public class PoolList
    {
        public FactionsEnum _poolsFaction;
        public List<PoolClass> _poolsList = new List<PoolClass>();
    }
    #endregion

    #region Instance Set Up
    public static AISpawner_Manager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion


    [SerializeField, Range(0, 100)] private int spawnCount;
    private float _timer;

    [Expandable]
    [SerializeField] private SO_AI_SpawnList _spawnListData;
    private List<PoolList> _factionPoolsList = new List<PoolList>();

    private void Start()
    {
        StartPoolsInitialization();
        StartPoolUnitInitialization();
    }

    private void Update()
    {
        
        _timer += Time.deltaTime;

        if(_timer >= 5.0f)
        {

            for (int i = 0; i < spawnCount / 10; i++)
            {
                Vector3 spawnPoint = transform.position + new Vector3(UnityEngine.Random.Range(-15, 15), UnityEngine.Random.Range(-15, 15));
                Spawn(FactionsEnum.Elf, SoldiersEnum.Larbin_A, spawnPoint);
            }
            _timer = 0;
        }
        
    }










    #region UseFull Function
    public SoldiersEnum GetSoldierEnumFromIndex(int index)
    {
        return (SoldiersEnum)index;
    }

    public FactionsEnum GetFactionEnumFromIndex(int index)
    {
        return (FactionsEnum)index;
    }


    public void Spawn(FactionsEnum faction, SoldiersEnum soldier, Vector3 spawnPoint)
    {
        GameObject soldierOject = _factionPoolsList[(int)faction]._poolsList[(int)soldier]._pool.Get().gameObject;
        soldierOject.transform.position = spawnPoint;
    }


    #endregion



    #region Initialization Functions
    public void StartPoolsInitialization()
    {
        for(int i = 0; i < _spawnListData.FactionsList.Count; i++)
        {
            Debug.Log("Start it�ration I = " + i);

            PoolList newPoolList = new PoolList();
            newPoolList._poolsFaction = GetFactionEnumFromIndex(i);

            _factionPoolsList.Add(newPoolList);

            for(int x = 0; x < _spawnListData.FactionsList[i].list.Count; x++)
            {
                Debug.Log("iteration I = " + i + " it�ration X = " + x);

               PoolClass newPoolClassList = new PoolClass();

                newPoolClassList._poolType = GetSoldierEnumFromIndex(x);
                newPoolClassList._pool = PoolInitialization(newPoolClassList._pool, i, x);

                _factionPoolsList[i]._poolsList.Add(newPoolClassList);
            }
        }

    }


    public ObjectPool<PooledObject> PoolInitialization(ObjectPool<PooledObject> objectPool, int factionIndex, int soldierIndex)
    {
        objectPool = new ObjectPool<PooledObject>(() =>
        {
            Debug.Log("Just Intantiate Unit");
            PooledObject pooledObject = Instantiate(_spawnListData.FactionsList[factionIndex].list[soldierIndex]).GetComponent<PooledObject>();
            pooledObject.InitalizedAction(UnSpawn, factionIndex, soldierIndex);

            return pooledObject;

        }, aiObject =>
        {
            aiObject.gameObject.SetActive(true);
        }, aiObject =>
        {
            aiObject.gameObject.SetActive(false);

        }, aiObject =>
        {
            Destroy(aiObject.gameObject);
        }, false, spawnCount, spawnCount * 2);

        
        return objectPool;
    }

    public void StartPoolUnitInitialization()
    {
        for (int i = 0; i < _spawnListData.FactionsList.Count; i++)
        {
            for (int x = 0; x < _spawnListData.FactionsList[i].list.Count; x++)
            {
                FactionsEnum currentFactionEnum = (FactionsEnum)i;
                SoldiersEnum currentSoldierEnum = (SoldiersEnum)x;

                List<PooledObject> initializedObjectsList = new List<PooledObject>();

                for (int y = 0; y < spawnCount; y++)
                {
                    initializedObjectsList.Add(InstantiatePoolUnit(currentFactionEnum, currentSoldierEnum));
                }

                foreach(PooledObject obj in initializedObjectsList)
                {
                    UnSpawn(obj);
                }

            }
        }
    }

    public PooledObject InstantiatePoolUnit(FactionsEnum faction, SoldiersEnum soldier)
    {
        return _factionPoolsList[(int)faction]._poolsList[(int)soldier]._pool.Get();
    }


    private void UnSpawn(PooledObject pooledObject)
    {
        _factionPoolsList[pooledObject.FactionIndex]._poolsList[pooledObject.SoldierIndex]._pool.Release(pooledObject);
    }

    #endregion
}
