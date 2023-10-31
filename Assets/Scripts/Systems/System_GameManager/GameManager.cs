using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static AI_Class;
public class GameManager : MonoBehaviour
{

    #region Setup Instance
    public static GameManager instance;

    private void Awake()
    {
        if(GameManager.instance == null)
        {
            GameManager.instance = this;
        }
        else
        {
            Destroy(GameManager.instance);
        }
    }
    #endregion


    #region Game State Fields
    public enum GameState
    {
        PreWave,
        InWave,
        PostWave,
        Paused
    }

    [SerializeField] private GameState _currentGameState;
    [SerializeField] private Transform[] _objectifs;

    public Transform[] Objectifs { get => _objectifs; set => _objectifs = value; }

    #endregion


    #region Spawn Fields
    [Header("Spawn Waves Fields")]
    [SerializeField, Expandable] private SO_AI_SpawnList _spawnData;
    private int _currentRound;
    private int _currentSpawnCount;
    private int _unitsSpawnedThisRound;
    private int _unitsDeadThisRound;
    private int _unitsDeadAllRounds;
    private float _postWaveTimer;

    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private float _timeBetweenWave;
    private float _currentSpawnerTimer;
    private bool _inWaveHasStarted;
    #endregion


    private void Start()
    {

    }

    private void Update()
    {
        UpdateGameStates();
    }

    private void SpawnUnits()
    {
        foreach(Transform t in _objectifs)
        {
            AISpawner_Manager.instance.Spawn(FactionsEnum.Elf, SoldiersEnum.Larbin_A, t.position);
        }
    }

    public void UpdateGameStates()
    {
        switch(_currentGameState)
        {
            case GameState.PreWave:
                UpdatePreWave();
                break;

            case GameState.InWave:
                if (!_inWaveHasStarted) StartWave();
                UpdateWave();
                break;

            case GameState.PostWave:
                UpdatePostWave();
                break;

            default: 
                break;
        }
    }

    #region PreWave State

    private void UpdatePreWave()
    {
        EndPreWave();
    }

    private void EndPreWave()
    {
        _currentGameState = GameState.InWave;
    }

    #endregion






    #region InWave State
    private void StartWave()
    {
        if(_currentRound == 0)
        {
            _currentSpawnCount = _spawnData.BaseSpawnCount;
        }
        else
        {
            _currentSpawnCount = _spawnData.BaseSpawnCount * (_currentRound * (_spawnData.BaseSpawnCount / 2));
        }

        _currentRound++;
        _inWaveHasStarted = true;
    }

    private void EndWave()
    {
        _currentGameState = GameState.PostWave;
        _inWaveHasStarted = false;
        _unitsSpawnedThisRound = 0;
        _unitsDeadThisRound = 0;
    }
    public void UpdateWave()
    {

        if (_unitsSpawnedThisRound < _currentSpawnCount)
        {
            _currentSpawnerTimer += Time.deltaTime;

            if (_currentSpawnerTimer > _timeBetweenWave)
            {
                AISpawner_Manager.instance.Spawn(FactionsEnum.Elf, SoldiersEnum.Larbin_A, _spawnPoints[Random.Range(0, _spawnPoints.Length)].position);

                _unitsSpawnedThisRound++;
                _currentSpawnerTimer = 0;
            }
        }
        else if(_unitsDeadThisRound == _unitsSpawnedThisRound)
        {
            EndWave();
        }

    }

    #endregion






    #region PostWave State
    private void UpdatePostWave()
    {
        _postWaveTimer += Time.deltaTime;

        if(_postWaveTimer >= 15.0f)
        {
            EndPostWave();
        }
    }
    private void TogglePostWaveTimer(bool activated)
    {
        if(activated)
        {

        }
        else
        {

        }
    }
    private void EndPostWave()
    {
        _postWaveTimer = 0.0f;
        _currentGameState = GameState.PreWave;
    }

    #endregion
}
