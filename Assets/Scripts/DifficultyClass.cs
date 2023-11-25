using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class DifficultyClass : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI Value;

    private int _value;

    [SerializeField] private GameObject ResultPrefabsSuccess;
    [SerializeField] private GameObject ResultPrefabsFail;
    [SerializeField] private GameObject ResultPrefabsCriticalSuccess;
    [SerializeField] private GameObject ResultPrefabsCriticalFail;

    private GameObject currentResult;
    private void Start()
    {
        _value = Random.Range(5, 31);
        Value.text = _value.ToString();
        DiceRoll.OnAllBuffComplete += ShowResult;
    }

    private void Update()
    {
        if (currentResult)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Destroy(currentResult.gameObject);
            }
        }
    }

    private void ShowResult(int value, bool isCritical)
    {
        bool isSuccess = false;
        if (isCritical)
        {
            isSuccess = value != 1;
            currentResult = Instantiate( isSuccess ? ResultPrefabsCriticalSuccess : ResultPrefabsCriticalFail);
        }
        else
        {
            isSuccess = value >= _value;
            currentResult = Instantiate(isSuccess ? ResultPrefabsSuccess : ResultPrefabsFail);
        }

        if (currentResult)
        {
            currentResult.transform.SetParent(transform,false);
        }

        Value.color = isSuccess ? new Color(1f, 0.886f, 0.62f) : new Color(0.604f, 0.149f, 0.149f);
        if (!isSuccess)
        {
            StartSuccessAnimation();
        }
        else
        {
            StartFailAnimation();
        }
    }

    private void StartSuccessAnimation()
    {
        
    }
    
    private void StartFailAnimation()
    {
        
    }

    private void OnDestroy()
    {
        DiceRoll.OnAllBuffComplete -= ShowResult;
    }
}
