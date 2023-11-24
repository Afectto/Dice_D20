using System.Collections.Generic;
using UnityEngine;

public class Buffs : MonoBehaviour
{
    public List<GameObject> prefabs;
    public BuffFactory BuffFactory;

    private List<GameObject> buffs;

    void Start()
    {
        buffs = new List<GameObject>();
        AddBuffByName("BuffDex");
        AddBuffByName("BuffDex");
        AddBuffByName("BuffInt");
        AddBuffByName("BuffStr");
        BuffFactory.CreateBuffGroup(buffs, transform);
    }

    private void AddBuffByName(string name)
    {
        var buff = prefabs.Find(buffInfo => buffInfo.name == name);
        if (buff is not null)
        {
            buffs.Add(buff);
        }
    }

}
