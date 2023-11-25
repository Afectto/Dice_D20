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
    
    private Rigidbody2D _rb; 
    private Animator _animator;
    private RectTransform _rctTransform;
    private TextMeshProUGUI _text;
    private int _countHitWall;
    private int _maxCountHitWall;
    private int _value;

    private bool _isRolling;
    
    public delegate void RollEndAction();
    public static event RollEndAction OnRollComplete;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _countHitWall = 0;
        _rctTransform = GetComponent<RectTransform>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _isRolling = false;
        Buff.OnTextMoveComplete += BuffTextMoveComplete;
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

    private  void PrepareToStartRoll()
    {
        _isRolling = true;
        _countHitWall = 0;
        _maxCountHitWall = Random.Range(5, 9);
        _text.enabled = false;
        _animator.enabled = true;
    }
    
    private void StartRollingRandomDirection()
    {
        PrepareToStartRoll();
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        _rb.velocity = randomDirection * moveSpeed;
        _animator.Play("Rolling");
    }

    private void StopRolling()
    {
        StartCoroutine(MoveToPosition(_rctTransform.position, new Vector3(0,1,90f), 1.5f));
    }
    
    IEnumerator MoveToPosition(Vector3 startPosition, Vector3 targetPosition, float timeToMove)
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
            OnRollComplete.Invoke();
        }
    }

    private void SetValueOnEndRoll()
    {
        _isRolling = false;
        _value = Random.Range(1, 21);
        _text.enabled = true;
        _text.text = _value.ToString();
    }

    void OnCollisionEnter2D(Collision2D collision)
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

    private void OnDestroy()
    {
        Buff.OnTextMoveComplete -= BuffTextMoveComplete;
    }
}
