using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class DiceRoll : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Sprite resultSprites;
    
    protected Rigidbody2D _rb; 
    protected Animator _animator;
    protected RectTransform _rctTransform;
    protected TextMeshProUGUI _text;
    protected int _countHitWall;
    protected int _maxCountHitWall;
    protected int _value;

    protected bool _isRolling;
    protected bool _isStopRolling;
    public delegate void RollEndAction(int value);
    public static event RollEndAction OnRollComplete;
    
    public delegate void AllBuffComplete(int value, bool isCritical);
    public static event AllBuffComplete OnAllBuffComplete;

    void Awake()
    {
        InitializeProperty();
        InitializeEvent();
    }

    private void InitializeProperty()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _countHitWall = 0;
        _rctTransform = GetComponent<RectTransform>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _isRolling = false;
    }

    protected virtual void InitializeEvent()
    {
        Buff.OnTextMoveComplete += BuffTextMoveComplete;
        BuffsList.OnAllBuffComplete += ShowResult;
        DifficultyClass.OnNeedStartAnimationDice += StartAnimationDice;
    }

    protected void ShowResult(bool isCritical)
    {
        _isRolling = false;
        OnAllBuffComplete.Invoke(_value, isCritical);
    }

    private void BuffTextMoveComplete(int intValue)
    {
        _value += intValue;
        _text.text = _value.ToString();
    }

    void Update()
    {
        GetComponent<RectTransform>().rotation = Quaternion.identity;
        if (Input.GetKeyDown(KeyCode.Space) && !_isRolling)
        {
            StartRollingRandomDirection();
        }
    }

    private void PrepareToStartRoll()
    {
        _isRolling = true;
        _countHitWall = 0;
        _maxCountHitWall = Random.Range(5, 9);
        _text.enabled = false;
        _animator.enabled = true;
        _text.color = Color.white;
        _value = -1;
        _isStopRolling = false;
    }
    
    private void StartRollingRandomDirection()
    {
        PrepareToStartRoll();
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        _rb.velocity = randomDirection * moveSpeed;
        _animator.Play("Rolling");
    }

    protected virtual void StopRolling()
    {
        if (!_isStopRolling)
        {
            _isStopRolling = true;
            StartCoroutine(MoveToPosition(_rctTransform.position, new Vector3(0,1,90f), 1.5f));
        }
    }
    
    protected virtual IEnumerator MoveToPosition(Vector3 startPosition, Vector3 targetPosition, float timeToMove)
    {
        _rb.velocity = Vector2.zero;
        _rb.rotation = 0;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / timeToMove;
            _rctTransform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        _animator.enabled = false;
        
        _rctTransform.position = targetPosition;

        SetValueOnEndRoll();
        GetComponentInChildren<Image>().sprite = resultSprites;
        if (_value != 20 && _value != 1)
        {
            InvokeRollComplete(_value);
        }
        else
        {
            ShowResult(true);
        }
    }
    public static void InvokeRollComplete(int value)
    {
        OnRollComplete?.Invoke(value);
    }
    protected void SetValueOnEndRoll()
    {
        _value = Random.Range(1, 21);
        _text.enabled = true;
        _text.text = _value.ToString();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            _countHitWall++;
            if (_countHitWall < _maxCountHitWall)
            {
                Vector2 bounceDirection = (transform.position - collision.transform.position).normalized;
            
                _rb.velocity = bounceDirection * moveSpeed;
            }
            else
            {
                StopRolling();
            }
        }
    }


    private void StartAnimationDice(bool isSuccess)
    {
        _animator.enabled = true;
        _animator.Play("Dice" +  (isSuccess ? "Success" : "Fail"), -1, 0f);
        _text.color = isSuccess ? new Color(1f, 0.886f, 0.62f) : new Color(0.604f, 0.149f, 0.149f);
    }

    protected virtual void OnDestroy()
    {
        Buff.OnTextMoveComplete -= BuffTextMoveComplete;
        BuffsList.OnAllBuffComplete -= ShowResult;
        DifficultyClass.OnNeedStartAnimationDice -= StartAnimationDice;
    }
}
