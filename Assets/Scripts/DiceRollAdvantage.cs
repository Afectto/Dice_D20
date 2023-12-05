using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DiceRollAdvantage : DiceRoll
{
    public delegate void IsNeedStopRoll();
    public static event IsNeedStopRoll OnIsNeedStopRoll;
    
    public delegate void IsStopRoll(int value, bool isSecond);
    public static event IsStopRoll OnIsStopRoll;
    
    public delegate void IsSetActive(bool isSecond, bool isActive);
    public static event IsSetActive OnIsSetActive;
    private bool _isSecond = false;

    public void SetIsSecond()
    {
        _isSecond = true;
    }

    protected override void InitializeEvent()
    {
        base.InitializeEvent();
        OnIsNeedStopRoll += StopRolling;
        OnIsStopRoll += OnStopRoll;
        OnIsSetActive += OnSetActive;
    }

    public void RefreshDice()
    {
        if(!isActiveAndEnabled) return;
        IsRolling = false;
        OnIsSetActive?.Invoke(true, true);
        OnIsSetActive?.Invoke(false, true);;
        RctTransform.position = new Vector3(_isSecond ? -10 : 10, 1, 90f);
        Text.color = Color.white;
        Text.text = "20";
    }
    
    private void OnSetActive(bool isSecond, bool isActive)
    {
        if(isSecond != _isSecond) return;
        
        gameObject.SetActive(isActive);
    }

    private void OnStopRoll(int value, bool isSecond)
    {
        if(!isActiveAndEnabled) return;

        if(Value == -1)
        {
            StartCoroutine(CheckSecondTime(value,isSecond));
            return;
        }
        
        if (_isSecond != isSecond)
        {
            if (Value == 20 || Value == 1)
            {
                OnIsSetActive?.Invoke(isSecond, false);
                ShowResult(true);
                StartCoroutine(MoveToCenter(RctTransform.position, new Vector3(0, 1, 90f), 1f));
                return;
            }

            StartCoroutine(WaitCheckCritical(value, isSecond));
        }
    }

    private IEnumerator WaitCheckCritical(int value, bool isSecond)
    {
        yield return new WaitForSeconds(0.1f);
        if(!isActiveAndEnabled) yield break;
        
        if (Value == value)
        {
            OnIsSetActive?.Invoke(isSecond, false);
        }
            
        if (Value >= value)
        {
            Animator.enabled = true;
            Animator.Play("DiceSuccess", -1, 0f);
            StartCoroutine(MoveToCenter(RctTransform.position, new Vector3(0, 1, 90f), 1f));
            InvokeRollComplete(Value);
        }
        else
        {
            StartCoroutine(RemoveDiceAfterSecond(0.2f));
        }
    }
    
    private IEnumerator RemoveDiceAfterSecond(float sec)
    {
        yield return new WaitForSeconds(sec);
        IsRolling = false;
        gameObject.SetActive(false);
    }

    private IEnumerator CheckSecondTime(int value, bool isSecond)
    {
        yield return new WaitForSeconds(0.1f);
        
        OnIsStopRoll?.Invoke(value, isSecond);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
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
                OnIsNeedStopRoll?.Invoke();
            }
        }
    }

    protected override void StopRolling()
    {
        if(!isActiveAndEnabled) return;
        if (!IsStopRolling)
        {
            IsStopRolling = true;
            StartCoroutine(MoveToPosition(RctTransform.position, new Vector3(_isSecond ? -10 : 10, 1, 90f), 1.5f));
        }
    }

    private IEnumerator MoveToCenter(Vector3 startPosition, Vector3 targetPosition, float timeToMove)
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
    }

    protected override IEnumerator MoveToPosition(Vector3 startPosition, Vector3 targetPosition, float timeToMove)
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

        OnIsStopRoll?.Invoke(Value, _isSecond);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        OnIsNeedStopRoll -= StopRolling;
        OnIsStopRoll -= OnStopRoll;
    }
}
