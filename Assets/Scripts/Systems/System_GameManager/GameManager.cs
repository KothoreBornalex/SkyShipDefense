using System.Collections;
using System.Collections.Generic;
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

    #region Objectifs Fields
    [SerializeField] private Transform[] _objectifs;
    [SerializeField] private Transform[] _spawnPoints;

    public Transform[] Objectifs { get => _objectifs; set => _objectifs = value; }

    #endregion


    private void Start()
    {
        AISpawner_Manager.instance.Spawn(FactionsEnum.Elf, SoldiersEnum.Larbin_A, _spawnPoints[0].position);

    }

}
