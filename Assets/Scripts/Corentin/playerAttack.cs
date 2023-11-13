using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static IStatistics;

public class playerAttack : MonoBehaviour
{
    // Fields

    [SerializeField] private LayerMask _damageableLayerMask;

    [SerializeField] private GameObject _spell1Prefab;
    [SerializeField] private GameObject _spell2Prefab;
    [SerializeField] private GameObject _spell3Prefab;

    private int _spellIndex = 1;

    private int _spell1Level = 0;
    private int _spell2Level = 0;
    private int _spell3Level = 0;

    [SerializeField] private PlayerAttacksData _playerAttacksData;



    // Properties
    public int Spell1Level { get => _spell1Level; set => _spell1Level = value; }
    public int Spell2Level { get => _spell2Level; set => _spell2Level = value; }
    public int Spell3Level { get => _spell3Level; set => _spell3Level = value; }


    // Methods
    public void ChangeAttackIndex(int index)
    {
        _spellIndex = index;
    }
    public void ChangeSpell1Level(int index)
    {
        if (index == _spell1Level + 1)
        {
            _spell1Level = index;
        }
    }
    public void ChangeSpell2Level(int index)
    {
        if (index == _spell2Level + 1)
        {
            _spell2Level = index;
        }
    }
    public void ChangeSpell3Level(int index)
    {
        if (index == _spell3Level + 1)
        {
            _spell3Level = index;
        }
    }

    private void Attack()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.yellow);

            Vector3 go = hit.point;

            switch (_spellIndex)
            {
                case 1:     // Si Faible dégats de zone
                    UseSpell1(go, _playerAttacksData.Spell1DamageStats[_spell1Level], _playerAttacksData.Spell1RadiusStats[_spell1Level]);
                    break;

                case 2:     // Si Fort dégats précis
                    UseSpell2(go, _playerAttacksData.Spell2DamageStats[_spell2Level], _playerAttacksData.Spell2RadiusStats[_spell2Level]);
                    break;

                case 3:     // Si Slow
                    UseSpell3(go, _playerAttacksData.Spell3SlowStats[_spell3Level], _playerAttacksData.Spell3RadiusStats[_spell3Level]);
                    break;

                default:        // Si aucune attaque
                    Debug.LogWarning("Erreur ! Aucune attaque reconnue !");
                    break;
            }

        }
    }

    private void UseSpell1(Vector3 attackOrigin, int damageValue, float radius)     // Cast Faible dégats de zone
    {
        GameObject att = Instantiate(_spell1Prefab, attackOrigin, Quaternion.identity);
        Collider[] hitCollider = Physics.OverlapSphere(transform.position, radius, _damageableLayerMask);
        DamageSpell(hitCollider, damageValue);
    }
    private void UseSpell2(Vector3 attackOrigin, int damageValue, float radius)     // Cast Fort dégats précis
    {
        GameObject att = Instantiate(_spell2Prefab, attackOrigin, Quaternion.identity);
        Collider[] hitCollider = Physics.OverlapSphere(transform.position, radius, _damageableLayerMask);
        DamageSpell(hitCollider, damageValue);
    }
    private void UseSpell3(Vector3 attackOrigin, int slowValue, float radius)       // Cast Freeze
    {
        GameObject att = Instantiate(_spell3Prefab, attackOrigin, Quaternion.identity);
        Collider[] hitCollider = Physics.OverlapSphere(transform.position, radius, _damageableLayerMask);
        SlowSpell(hitCollider, slowValue);
    }

    private void DamageSpell(Collider[] colliders, int damageValue)     // Inflige dégats
    {
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Ennemy"))
            {
                collider.gameObject.GetComponent<IStatistics>().DecreaseStat(StatName.Health, damageValue);
            }
        }
    }
    private void SlowSpell(Collider[] colliders, int slowValue)     // Slow ennemis
    {
        foreach(Collider collider in colliders)
        {
            if (collider.CompareTag("Ennemy"))
            {
                // !!!!! Slow ennemis
                //collider.gameObject.GetComponent<IStatistics>().DecreaseStat();
            }
        }
    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }
    }
}
