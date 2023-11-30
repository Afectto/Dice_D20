using System;
using System.Collections.Generic;
using UnityEngine;

public class CanAddBuffsList : MonoBehaviour
{    
    public List<GameObject> prefabs;
    private List<GameObject> buffs;
    private List<GameObject> CurrentShowBuffs;
    public BuffFactory BuffFactory;
    
    [SerializeField]private BuffsList _buffsList;
    
    void Start()
    {
        buffs = new List<GameObject>();
        AddBuffByName("BuffDex");
        AddBuffByName("BuffInt");
        AddBuffByName("BuffStr");
        AddBuffByName("BuffD4");
        CurrentShowBuffs = BuffFactory.CreateBuffGroup(buffs, transform);
    }
    
    private void AddBuffByName(string name)
    {
        var buff = prefabs.Find(buffInfo => buffInfo.name == name);
        if (buff is not null)
        {
            buffs.Add(buff);
        }
    }
    
    private void ClearShowBuff()
    {
        foreach (var myBuff in CurrentShowBuffs)
        {
            Destroy(myBuff.gameObject);
        }
    }
    
    public void RemoveToBuffList(GameObject buff)
    {
        var replace = buff.name.Replace("(Clone)", "");
        var obj = buffs.Find(buffs => replace == buffs.name);
        buffs.RemoveAll(obj => obj.name == replace);
        Destroy(obj.gameObject);
        ClearShowBuff();
        CurrentShowBuffs = BuffFactory.CreateBuffGroup(buffs, transform);
    }
    
    public void AddToBuffList(string name)
    {
        _buffsList.AddBuffByName(name);
    }
}
