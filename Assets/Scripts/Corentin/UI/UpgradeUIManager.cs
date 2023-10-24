using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeUIManager : MonoBehaviour
{
    // Fields
    [SerializeField] private RectTransform _upgradeSkillsPanel;

    [SerializeField] private RectTransform _openPosition;
    [SerializeField] private RectTransform _closedPosition;

    private bool _isOpen;

    [SerializeField] private float _slideSpeed;

    // Properties



    // Methods
    public void UpgradeSkillsPanelSlide()
    {
        if( _isOpen)
        {
            _isOpen = false;
            Debug.Log("Close");
            StartCoroutine(CloseUpgradePanel());
        }
        else
        {
            _isOpen = true;
            Debug.Log("Open");
            StartCoroutine(OpenUpgradePanel());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator OpenUpgradePanel()
    {
        Vector3 positionTemp = _upgradeSkillsPanel.position;
        
        while ((_upgradeSkillsPanel.position.y != _openPosition.position.y) && _isOpen)
        {

            positionTemp.y = Mathf.Lerp(_upgradeSkillsPanel.position.y, _openPosition.position.y, Time.deltaTime * _slideSpeed);

            _upgradeSkillsPanel.position = positionTemp;

            yield return null;
        }

        yield return null;
    }
    IEnumerator CloseUpgradePanel()
    {
        Vector3 positionTemp = _upgradeSkillsPanel.position;

        while ((_upgradeSkillsPanel.position.y != _closedPosition.position.y) && !_isOpen)
        {
            positionTemp.y = Mathf.Lerp(_upgradeSkillsPanel.position.y, _closedPosition.position.y, Time.deltaTime * _slideSpeed);

            _upgradeSkillsPanel.position = positionTemp;

            yield return null;
        }

        yield return null;
    }
}
