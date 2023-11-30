using System;
using System.Collections.Generic;
using UnityEngine;

public class BuffsList : MonoBehaviour
{    
    public List<GameObject> prefabs;
    public BuffFactory BuffFactory;

    private List<GameObject> buffs;
    private List<GameObject> CurrentShowBuffs;

    public delegate void AllBuffCompleteAction(bool isCritical);
    public static event AllBuffCompleteAction OnAllBuffComplete;

    private int _countBuffCompleteAnimation;
    [SerializeField]private CanAddBuffsList _buffsCanAddList;

    void Start()
    {
        DiceRoll.OnRollComplete += StartAnimation;
        Buff.OnTextMoveComplete += BuffTextMoveComplete;
        CurrentShowBuffs = new List<GameObject>();
        _countBuffCompleteAnimation = 0;
        
        buffs = new List<GameObject>();
        // AddBuffByName("BuffDex");
        AddBuffByName("BuffInt");
        // AddBuffByName("BuffDex");
        // AddBuffByName("BuffStr");
        // AddBuffByName("BuffD4");
        // AddBuffByName("Advantage");
        // CurrentShowBuffs = BuffFactory.CreateBuffGroup(buffs, transform);
    }

    private void Update()
    {
        if (_countBuffCompleteAnimation < buffs.Count && Input.GetKeyDown(KeyCode.Space))
        {
            
        }
    }

    public void AddBuffByName(string name)
    {
        var replace = name.Replace("(Clone)", "");
        ClearShowBuff();
        
        var buff = prefabs.Find(buffInfo => buffInfo.name == replace);
        if (buff is not null)
        {
            buffs.Add(buff);
        }
        CurrentShowBuffs = BuffFactory.CreateBuffGroup(buffs, transform);
    }

    private void ClearShowBuff()
    {
        foreach (var myBuff in CurrentShowBuffs)
        {
            Destroy(myBuff.gameObject);
        }
    }
    
    public void RemoveBuff(GameObject buff)
    {
        ClearShowBuff();
        var replace = buff.name.Replace("(Clone)", "");
        buffs.RemoveAll(obj => obj.name == replace);
        var obj = CurrentShowBuffs.Find(buffs => buff == buffs);
        Destroy(obj.gameObject);
        CurrentShowBuffs = BuffFactory.CreateBuffGroup(buffs, transform);
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
            CurrentShowBuffs[index].GetComponent<Buff>().StartAnimationValue();
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
    
    public void AddToBuffList(string name)
    {
        _buffsCanAddList.AddBuffByName(name);
    }

    private void OnDestroy()
    {
        DiceRoll.OnRollComplete -= StartAnimation;
        Buff.OnTextMoveComplete -= BuffTextMoveComplete;
    }
}
