using System.Collections.Generic;
using UnityEngine;

public class Buffs : MonoBehaviour
{
    public List<GameObject> prefabs;
    public BuffFactory BuffFactory;
    void Start()
    {
        BuffFactory.CreateBuffGroup(prefabs,transform);
    }

}
