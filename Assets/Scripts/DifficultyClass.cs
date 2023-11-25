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
    private Animator _animatorValue;
    private bool _isCanRestart = true;
    
    private void Start()
    {
        _value = Random.Range(5, 31);
        Value.text = _value.ToString();
        DiceRoll.OnAllBuffComplete += ShowResult;
        _animatorValue = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (currentResult)
        {
            if (Input.GetKeyDown(KeyCode.Space) && _isCanRestart)
            {
                _isCanRestart = false;
                ResetValue();
                Destroy(currentResult.gameObject);
            }
        }
    }
    
    private void ResetValue()
    {
        _value = Random.Range(5, 31);
        Value.text = _value.ToString();
        Value.color = Color.white;
        
    }
    

    private void ShowResult(int value, bool isCritical)
    {
        _isCanRestart = true;
        
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
            StartFailAnimation();
        }
        else
        {
            StartSuccessAnimation();
        }
    }

    private void StartSuccessAnimation()
    {
        
    }
    
    private void StartFailAnimation()
    {
        _animatorValue.Play("ValueFail", -1, 0f);
    }

    private void OnDestroy()
    {
        DiceRoll.OnAllBuffComplete -= ShowResult;
    }
}
