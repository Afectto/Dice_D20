using System;
using System.Collections.Generic;
using UnityEngine;

public class BuffsList : MonoBehaviour
{    
    public List<GameObject> prefabs;
    public BuffFactory BuffFactory;

    private List<GameObject> buffs;

    public delegate void AllBuffCompleteAction(bool isCritical);
    public static event AllBuffCompleteAction OnAllBuffComplete;

    private int _countBuffCompleteAnimation;
    void Start()
    {
        DiceRoll.OnRollComplete += StartAnimation;
        Buff.OnTextMoveComplete += BuffTextMoveComplete;
        _countBuffCompleteAnimation = 0;
        
        buffs = new List<GameObject>();
        // AddBuffByName("BuffDex");
        AddBuffByName("BuffInt");
        // AddBuffByName("BuffDex");
        // AddBuffByName("BuffStr");
        AddBuffByName("BuffD4");
        // AddBuffByName("Advantage");
        buffs = BuffFactory.CreateBuffGroup(buffs, transform);
    }

    private void Update()
    {
        if (_countBuffCompleteAnimation < buffs.Count && Input.GetKeyDown(KeyCode.Space))
        {
            
        }
    }

    private void AddBuffByName(string name)
    {
        var buff = prefabs.Find(buffInfo => buffInfo.name == name);
        if (buff is not null)
        {
            buffs.Add(buff);
        }
    }
    
    private void StartAnimation(int value)
    {
        _countBuffCompleteAnimation = 0;
        StartAnimationBuff(0);
    }
    
    private void StartAnimationBuff(int index)
    {
        if (index < buffs.Count)
        {
            buffs[index].GetComponent<Buff>().StartAnimationValue();
        }
    }

    private void BuffTextMoveComplete(int value)
    {
        _countBuffCompleteAnimation++;
        StartAnimationBuff(_countBuffCompleteAnimation);
        if (buffs.Count == _countBuffCompleteAnimation)
        {
            OnAllBuffComplete.Invoke(false);;
        }
    }

    private void OnDestroy()
    {
        DiceRoll.OnRollComplete -= StartAnimation;
        Buff.OnTextMoveComplete -= BuffTextMoveComplete;
    }
}
