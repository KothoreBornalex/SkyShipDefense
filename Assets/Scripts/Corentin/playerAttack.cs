using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAttack : MonoBehaviour
{
    // Fields

    [SerializeField] private GameObject _spell1Prefab;
    [SerializeField] private GameObject _spell2Prefab;
    [SerializeField] private GameObject _spell3Prefab;

    private int _spellIndex = 1;

    private int _spell1Level = 0;
    private int _spell2Level = 0;
    private int _spell3Level = 0;

    [SerializeField] private PlayerAttacksData _playerAttacksData;

    
    // Properties


    // Methods
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
                case 1:
                    UseSpell1(go, _playerAttacksData.Spell1DamageStats[_spell1Level], _playerAttacksData.Spell1RadiusStats[_spell1Level]);
                    break;
                case 2:
                    UseSpell2(go, _playerAttacksData.Spell2DamageStats[_spell2Level], _playerAttacksData.Spell2RadiusStats[_spell2Level]);
                    break;
                case 3:
                    UseSpell3(go, _playerAttacksData.Spell3SlowStats[_spell3Level], _playerAttacksData.Spell3RadiusStats[_spell3Level]);
                    break;
                default:
                    Debug.LogWarning("Erreur ! Aucune attaque reconnue !");
                    break;

            }

        }
    }

    private void UseSpell1(Vector3 attackOrigin, int damageValue, float radius)
    {
        GameObject att = Instantiate(_spell1Prefab, attackOrigin, Quaternion.identity);
        SphereCollider collider = att.GetComponent<SphereCollider>();
        collider.radius = radius;
    }
    private void UseSpell2(Vector3 attackOrigin, int damageValue, float radius)
    {
        GameObject att = Instantiate(_spell2Prefab, attackOrigin, Quaternion.identity);
        SphereCollider collider = att.GetComponent<SphereCollider>();
        collider.radius = radius;
    }
    private void UseSpell3(Vector3 attackOrigin, int slowValue, float radius)
    {
        GameObject att = Instantiate(_spell3Prefab, attackOrigin, Quaternion.identity);
        SphereCollider collider = att.GetComponent<SphereCollider>();
        collider.radius = radius;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }
    }
}
