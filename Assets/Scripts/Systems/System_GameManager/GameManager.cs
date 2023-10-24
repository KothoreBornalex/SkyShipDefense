using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Transform[] Objectifs { get => _objectifs; set => _objectifs = value; }

    #endregion



}
