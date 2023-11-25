using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffsList : MonoBehaviour
{
    public List<GameObject> prefabs;
    public BuffFactory BuffFactory;

    private List<GameObject> buffs;
    [SerializeField]private DiceRoll _diceRoll;

    void Start()
    {
        DiceRoll.OnRollComplete += StartAnimation;
        
        buffs = new List<GameObject>();
        AddBuffByName("BuffDex");
        AddBuffByName("BuffInt");
        AddBuffByName("BuffDex");
        AddBuffByName("BuffStr");
        buffs = BuffFactory.CreateBuffGroup(buffs, transform);
    }

    private void StartAnimation()
    {
        StartCoroutine(StartAnimationBuff());
    }
    
    private void AddBuffByName(string name)
    {
        var buff = prefabs.Find(buffInfo => buffInfo.name == name);
        if (buff is not null)
        {
            buffs.Add(buff);
        }
    }

    private IEnumerator StartAnimationBuff()
    {
        foreach (var buff in buffs)
        {
            yield return new WaitForSeconds(0.5f);
            buff.GetComponent<Buff>().StartAnimationValue();
        }
    }

    private void OnDestroy()
    {
        DiceRoll.OnRollComplete -= StartAnimation;
    }
}
