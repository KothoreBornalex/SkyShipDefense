using NaughtyAttributes;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IStatistics;

public class AI_Class : MonoBehaviour, IStatistics
{
    public enum FactionsEnum
    {
        Orc = 0,
        Elf = 1,
        Necromancer = 2,
        Human = 3
    }
    public enum SoldiersEnum
    {
        Larbin_A = 0,
        Larbin_B = 1,
        Larbin_C = 2,
        LittleChief = 3,
        Chief = 4,
        BigChief = 5,
    }


    [Button("Receive Damage")] void Attack() => LoseLP();
    private void LoseLP()
    {
        DecreaseStat(StatName.Health, (int)UnityEngine.Random.Range(1, 3));
    }

    [Header("Global AI Fields")]
    [SerializeField] private SoldiersEnum _unitType;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField, Expandable] private AI_Data _ai_Data;
    [SerializeField] private bool _setChase;
    private int _targetID;
    private Rigidbody _rigidbody;


    [SerializeField] private WeaponsScriptableObject _weaponsList;
    private List<Statistics> _aiStatistics = new List<Statistics>();

    [Header("Pathfinding Fields")]
    private AIPath _aiPath;
    private AIDestinationSetter _destination;
    private Seeker _seeker;

    [Header("Patrols Fields")]
    [SerializeField] private Transform[] patrolWayPoints;
    private int currentPatrolPoint;


    [Header("Attack Fields")]
    private int _currentWeaponIndex;
    [SerializeField, Range(0, 5)] private float _attackFrequency;
    [SerializeField] private Transform _bulletSpawnPoint;
    private float _attackTimer;

    private void Start()
    {
        _targetID = Random.Range(0, GameManager.instance.Objectifs.Length);

        _seeker = GetComponent<Seeker>();
        _destination = GetComponent<AIDestinationSetter>();
        _currentWeaponIndex = GetWeaponIndex(_unitType);
        _rigidbody = GetComponent<Rigidbody>();

        InitializeStats();
        FreezPhysics();

    }


    private void Update()
    {
        HandleChase();

        /*
        if (!_isAlerted)
        {
            HandlePatrol();
        }
        else
        {
            HandleChase();
        }
        */
    }


    #region Patrol & Chases Functions
    public void HandleChase()
    {
        
        if (!_setChase)
        {
            _destination.target = GameManager.instance.Objectifs[_targetID];
            _setChase = true;
        }

        if (Vector3.Distance(transform.position, GameManager.instance.Objectifs[_targetID].position) > _ai_Data.AttackRange && _destination.target != GameManager.instance.Objectifs[_targetID])
        {
            _destination.target = GameManager.instance.Objectifs[_targetID];
        }
        else if(Vector3.Distance(transform.position, GameManager.instance.Objectifs[_targetID].position) <= _ai_Data.AttackRange)
        {
            _destination.target = null;

            _attackTimer += Time.deltaTime;
            //HandleAIAttack();
            Debug.Log("Attack !!");
        }
        
    }

    public void HandlePatrol()
    {
        if(patrolWayPoints.Length == 0)
        {
            return;
        }

        if (_destination.target == null)
        {
            _destination.target = patrolWayPoints[currentPatrolPoint].transform;
        }


        if (Mathf.Round(transform.position.x) == Mathf.Round(patrolWayPoints[currentPatrolPoint].position.x) && Mathf.Round(transform.position.y) == Mathf.Round(patrolWayPoints[currentPatrolPoint].position.y) && currentPatrolPoint != patrolWayPoints.Length)
        {
            currentPatrolPoint++;
            _destination.target = patrolWayPoints[currentPatrolPoint].transform;

        }
        else if(patrolWayPoints.Length == currentPatrolPoint)
        {
            currentPatrolPoint = 0;
            _destination.target = patrolWayPoints[currentPatrolPoint].transform;
        }
    }

    #endregion

    public void FreezPhysics()
    {
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void UnFreezPhysics()
    {
        _rigidbody.constraints = RigidbodyConstraints.None;
    }

    public void Death()
    {
        Instantiate<GameObject>(_ai_Data.DeathObject, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }



    #region Global Attacks Functions
    public void HandleAIAttack()
    {

        switch (_unitType)
        {
            case SoldiersEnum.Larbin_A:
                if (_attackTimer >= _weaponsList.WeaponsList[_currentWeaponIndex].weaponCoolDown)
                    HandleKatanaAttack();
                break;


            case SoldiersEnum.Larbin_B:
                if (_attackTimer >= _weaponsList.WeaponsList[_currentWeaponIndex].weaponCoolDown)
                    HandlePistolAttack();
                break;

            case SoldiersEnum.Larbin_C:
                if (_attackTimer >= _weaponsList.WeaponsList[_currentWeaponIndex].weaponCoolDown)
                    HandleFusilAttack();
                break;

        }

    }


    public void HandleKatanaAttack()
    {
        Debug.Log("Katana Attack !!");
        //AudioManager.instance.PlayOneShot_GlobalSound(FMODEvents.instance.Weapons_KatanaSlash);
        //PlayerStateMachine.instance.DecreaseStat(StatName.Health, _weaponsList.WeaponsList[_currentWeaponIndex].weaonDamage);
        _attackTimer = 0;
    }


    public void HandlePistolAttack()
    {
        int currentWeapon = GetWeaponIndex(_unitType);

        //AudioManager.instance.PlayOneShot_GlobalSound(FMODEvents.instance.Weapons_PistolShot);


        // Calculate the rotation of the pistol in degrees
        float pistolRotation = _bulletSpawnPoint.eulerAngles.z;
        float pistolRotationRad = pistolRotation * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(pistolRotationRad), Mathf.Sin(pistolRotationRad));

        //Instantiation Projectile
        //WeaponContactTrigger projectile = Instantiate<GameObject>(PlayerStateMachine.instance.WeaponsList.WeaponsList[currentWeapon].attackProjectile, transform.position, Quaternion.identity).GetComponent<WeaponContactTrigger>();
        //projectile.direction = direction;
        //projectile.TargetedFaction = Factions.Player;
        _attackTimer = 0;


    }

    public void HandleFusilAttack()
    {
        int currentWeapon = GetWeaponIndex(_unitType);

        //AudioManager.instance.PlayOneShot_GlobalSound(FMODEvents.instance.Weapons_FusilShot);


        // Calculate the rotation of the pistol in degrees
        float pistolRotation = _bulletSpawnPoint.eulerAngles.z;
        float pistolRotationRad = pistolRotation * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(pistolRotationRad), Mathf.Sin(pistolRotationRad));

        //Instantiation Projectile
        //WeaponContactTrigger projectile = Instantiate<GameObject>(PlayerStateMachine.instance.WeaponsList.WeaponsList[currentWeapon].attackProjectile, transform.position, Quaternion.identity).GetComponent<WeaponContactTrigger>();
        //projectile.direction = direction;
        //projectile.TargetedFaction = Factions.Player;

        _attackTimer = 0;

    }

    public int GetWeaponIndex(SoldiersEnum soldierEnum)
    {
        for (int i = 0; i < _weaponsList.WeaponsList.Count; i++)
        {
            if (_weaponsList.WeaponsList[i].soldierType == soldierEnum)
            {
                return i;
            }
        }

        return 0;
    }

    #endregion



    #region IStatistics Functions

    public void InitializeStats()
    {
        foreach (Statistics statistics in _ai_Data.AiStatistics)
        {
            _aiStatistics.Add(new Statistics(statistics._statName, statistics._statCurrentValue, statistics._statMaxValue));
        }
    }


    public void SetStat(StatName statName, float statValue)
    {

        foreach (Statistics stats in _aiStatistics)
        {
            if (stats._statName == statName)
            {
                stats._statCurrentValue = statValue;
            }
        }

    }

    public void DecreaseStat(StatName statName, float decreasingValue)
    {

        foreach (Statistics stats in _aiStatistics)
        {
            if (stats._statName == statName)
            {
                if (stats._statName == StatName.Health)
                {
                    //AudioManager.instance.PlayOneShot_GlobalSound(FMODEvents.instance.Player_Hurt);
                    //_aiSprite.color = Color.red;


                    if(stats._statCurrentValue <= 0)
                    {
                        Death();
                    }
                }

                stats._statCurrentValue -= decreasingValue;
                stats._statCurrentValue = Mathf.Clamp(stats._statCurrentValue, 0, stats._statMaxValue);
                return;
            }
        }

    }

    public void IncreaseStat(StatName statName, float increasingValue)
    {

        foreach (Statistics stats in _aiStatistics)
        {
            if (stats._statName == statName)
            {
                if (stats._statName == StatName.Health)
                {
                    //AudioManager.instance.PlayOneShot_GlobalSound(FMODEvents.instance.Player_Healed);
                    //_aiSprite.color = Color.green;
                }

                stats._statCurrentValue += increasingValue;
                stats._statCurrentValue = Mathf.Clamp(stats._statCurrentValue, 0, stats._statMaxValue);
                return;
            }
        }

    }



    public void ResetStat(StatName statName)
    {
        foreach (Statistics stat in _aiStatistics)
        {
            if (stat._statName == statName)
            {
                stat._statCurrentValue = stat._statMaxValue;
                return;
            }
        }
    }

    public float GetStat(StatName statName)
    {
        foreach (Statistics stats in _aiStatistics)
        {
            if (stats._statName == statName)
            {
                return stats._statCurrentValue;
            }
        }

        return 0.0f;
    }


    public float GetMaxStat(StatName statName)
    {
        foreach (Statistics stats in _aiStatistics)
        {
            if (stats._statName == statName)
            {
                return stats._statMaxValue;
            }
        }

        return 0.0f;
    }
    #endregion
}
