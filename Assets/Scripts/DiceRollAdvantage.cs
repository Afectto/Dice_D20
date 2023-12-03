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

    private void OnSetActive(bool isSecond, bool isActive)
    {
        if(isSecond != _isSecond) return;
        
        gameObject.SetActive(isActive);
    }

    private void OnStopRoll(int value, bool isSecond)
    {
        if(!isActiveAndEnabled) return;

        if(_value == -1)
        {
            StartCoroutine(CheckSecondTime(value,isSecond));
            return;
        }
        if (_isSecond != isSecond)
        {
            if (_value == 20 || _value == 1)
            {
                OnIsSetActive.Invoke(isSecond, false);
                ShowResult(true);
                StartCoroutine(MoveToCenter(_rctTransform.position, new Vector3(0, 1, 90f), 1f));
                return;
            }

            if (_value == value)
            {
                OnIsSetActive.Invoke(isSecond, false);
            }
            
            if (_value >= value)
            {
                _animator.enabled = true;
                _animator.Play("DiceSuccess", -1, 0f);
                StartCoroutine(MoveToCenter(_rctTransform.position, new Vector3(0, 1, 90f), 1f));
                InvokeRollComplete(_value);
            }
            else
            {
                StartCoroutine(RemoveDiceAfterSecond(0.2f));
            }
        }
    }

    private IEnumerator RemoveDiceAfterSecond(float sec)
    {
        yield return new WaitForSeconds(sec);
        gameObject.SetActive(false);
    }

    private IEnumerator CheckSecondTime(int value, bool isSecond)
    {
        yield return new WaitForSeconds(0.1f);
        
        OnIsStopRoll.Invoke(value, isSecond);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
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
                OnIsNeedStopRoll.Invoke();
            }
        }
    }

    protected override void StopRolling()
    {
        if(!isActiveAndEnabled) return;
        if (!_isStopRolling)
        {
            _isStopRolling = true;
            StartCoroutine(MoveToPosition(_rctTransform.position, new Vector3(_isSecond ? -10 : 10, 1, 90f), 1.5f));
        }
    }

    private IEnumerator MoveToCenter(Vector3 startPosition, Vector3 targetPosition, float timeToMove)
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
    }

    protected override IEnumerator MoveToPosition(Vector3 startPosition, Vector3 targetPosition, float timeToMove)
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

        OnIsStopRoll.Invoke(_value, _isSecond);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        OnIsNeedStopRoll -= StopRolling;
        OnIsStopRoll -= OnStopRoll;
    }
}
