using System.Collections;
using UnityEngine;

public class BuffD4 : Buff
{
    private Animator _animator;
    
    public override void Initialize()
    {
        Value = Random.Range(1, 5).ToString();
        base.Initialize();
        valueText.enabled = false;
        _animator = GetComponentInChildren<Animator>();
    }

    public override void StartAnimationValue()
    {
        _animator.Play("RollingD4");
        StartCoroutine(Roll());
    }

    private IEnumerator Roll()
    {
        yield return new WaitForSeconds(0.75f);
        _animator.Play("Stop");
        base.StartAnimationValue();
    }
}
