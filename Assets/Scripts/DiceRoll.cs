using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class DiceRoll : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private bool _isRolling;

    protected Rigidbody2D Rb; 
    protected Animator Animator;
    protected RectTransform RctTransform;
    protected int CountHitWall;
    protected int MAXCountHitWall;
    protected int Value;
    protected bool IsStopRolling;

    public float moveSpeed = 5f;
    public Sprite resultSprites;

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
        Rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        CountHitWall = 0;
        RctTransform = GetComponent<RectTransform>();
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
        if(!isActiveAndEnabled) return;
        var k = this;
        OnAllBuffComplete.Invoke(Value, isCritical);
    }

    private void BuffTextMoveComplete(int intValue)
    {
        if(!isActiveAndEnabled) return;
        Value += intValue;
        _text.text = Value.ToString();
    }

    void Update()
    {
        GetComponent<RectTransform>().rotation = Quaternion.identity;
        if (Input.GetKeyDown(KeyCode.Space) && !_isRolling)
        {
            StartRollingRandomDirection();
        }
    }

    protected virtual void PrepareToStartRoll()
    {
        _isRolling = true;
        CountHitWall = 0;
        MAXCountHitWall = Random.Range(5, 9);
        _text.enabled = false;
        Animator.enabled = true;
        _text.color = Color.white;
        Value = -1;
        IsStopRolling = false;
        RctTransform.localScale = Vector3.one;
    }
    
    private void StartRollingRandomDirection()
    {
        PrepareToStartRoll();
        if(!isActiveAndEnabled) return;
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Rb.velocity = randomDirection * moveSpeed;
        Animator.Play("Rolling");
    }

    protected virtual void StopRolling()
    {
        if(!isActiveAndEnabled) return;
        if (!IsStopRolling)
        {
            IsStopRolling = true;
            StartCoroutine(MoveToPosition(RctTransform.position, new Vector3(0,1,90f), 1.5f));
        }
    }
    
    protected virtual IEnumerator MoveToPosition(Vector3 startPosition, Vector3 targetPosition, float timeToMove)
    {
        Rb.velocity = Vector2.zero;
        Rb.rotation = 0;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / timeToMove;
            RctTransform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        Animator.enabled = false;
        
        RctTransform.position = targetPosition;

        SetValueOnEndRoll();
        GetComponentInChildren<Image>().sprite = resultSprites;
        if (Value != 20 && Value != 1)
        {
            InvokeRollComplete(Value);
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
        if(!isActiveAndEnabled) return;
        Value = Random.Range(1, 21);
        _text.enabled = true;
        _text.text = Value.ToString();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            CountHitWall++;
            if (CountHitWall < MAXCountHitWall)
            {
                Vector2 bounceDirection = (transform.position - collision.transform.position).normalized;
            
                Rb.velocity = bounceDirection * moveSpeed;
            }
            else
            {
                StopRolling();
            }
        }
    }


    private void StartAnimationDice(bool isSuccess)
    {
        if(!isActiveAndEnabled) return;
        
        Animator.enabled = true;
        Animator.Play("Dice" +  (isSuccess ? "Success" : "Fail"), -1, 0f);
        _text.color = isSuccess ? new Color(1f, 0.886f, 0.62f) : new Color(0.604f, 0.149f, 0.149f);
    }

    protected virtual void OnDestroy()
    {
        Buff.OnTextMoveComplete -= BuffTextMoveComplete;
        BuffsList.OnAllBuffComplete -= ShowResult;
        DifficultyClass.OnNeedStartAnimationDice -= StartAnimationDice;
    }
}
