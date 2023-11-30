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
        CurrentShowBuffs = new List<GameObject>();
        buffs = new List<GameObject>();
        AddBuffByName("BuffDex");
        AddBuffByName("BuffInt");
        AddBuffByName("BuffStr");
        AddBuffByName("BuffD4");
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
    
    public void RemoveToBuffList(GameObject buff)
    {
        ClearShowBuff();
        var replace = buff.name.Replace("(Clone)", "");
        buffs.RemoveAll(obj => obj.name == replace);
        var obj = CurrentShowBuffs.Find(myBuff => buff.name == myBuff.name);
        Destroy(obj.gameObject);
        CurrentShowBuffs = BuffFactory.CreateBuffGroup(buffs, transform);
    }
    
    public void AddToBuffList(string name)
    {
        _buffsList.AddBuffByName(name);
    }
}
