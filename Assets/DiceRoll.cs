using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceRoll : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D _rb; 
    private Animator _animator;
    public Sprite resultSprites;
    
    private int countHitWall;
    private int MaxCountHitWall;
    private RectTransform _rctTransform;
    private TextMeshProUGUI _text;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        countHitWall = 0;
        MaxCountHitWall = Random.Range(6, 9);
        _rctTransform = GetComponent<RectTransform>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        GetComponent<RectTransform>().rotation = Quaternion.identity;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartMovingRandomDirection();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            StopRolling();
        }
    }

    void StartMovingRandomDirection()
    {
        _text.enabled = false;
        _animator.enabled = true;
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        _rb.velocity = randomDirection * moveSpeed;
        _animator.Play("Rolling");
    }

    private void StopRolling()
    {
        StartCoroutine(MoveToPosition(_rctTransform.position, new Vector3(0,1,90f), 2));
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
        
        var spriteIndex = Random.Range(0, 20);
        _text.enabled = true;
        _text.text = spriteIndex.ToString();
        GetComponentInChildren<Image>().sprite = resultSprites;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            countHitWall++;
            if (countHitWall < MaxCountHitWall)
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
}
