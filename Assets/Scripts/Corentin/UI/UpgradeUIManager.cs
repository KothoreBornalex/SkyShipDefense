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

    private bool _stopSliding;

    [SerializeField] private float _slideSpeed;

    // Properties



    // Methods
    public void UpgradeSkillsPanelSlide()
    {
        //_stopSliding = true;
        if( _isOpen)
        {
            Debug.Log("Close");
            StartCoroutine(CloseUpgradePanel());
        }
        else
        {
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

        while ((_upgradeSkillsPanel.position.y != _openPosition.position.y) && !_stopSliding)
        {
            positionTemp.y = Mathf.Lerp(_upgradeSkillsPanel.position.y, _openPosition.position.y, Time.deltaTime * _slideSpeed);

            _upgradeSkillsPanel.position = positionTemp;
        }

        _stopSliding = false;
        yield return null;
    }
    IEnumerator CloseUpgradePanel()
    {
        Vector3 positionTemp = _upgradeSkillsPanel.position;

        while ((_upgradeSkillsPanel.position.y != _closedPosition.position.y) && !_stopSliding)
        {
            positionTemp.y = Mathf.Lerp(_upgradeSkillsPanel.position.y, _closedPosition.position.y, Time.deltaTime * _slideSpeed);

            _upgradeSkillsPanel.position = positionTemp;
        }

        _stopSliding = false;
        yield return null;
    }
}
