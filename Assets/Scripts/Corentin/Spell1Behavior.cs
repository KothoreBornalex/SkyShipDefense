using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell1Behavior : AttackBehavior
{

    [SerializeField] private float _duration;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpellTimeCast());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpellTimeCast()
    {
        float _currentDuration = _duration;
        while (_currentDuration <= 0f)
        {
            _currentDuration -= Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
        yield return null;
    }
}
