using System.Collections.Generic;
using UnityEngine;

public class BuffFactory : MonoBehaviour
{
    float distanceBetweenCenters = 120f;
    
    public List<GameObject> CreateBuffGroup(List<GameObject> buffPrefab, Transform parentTransform)
    {
        List<GameObject> result = new List<GameObject>();
        var numberOfObjects = buffPrefab.Count;

        float totalWidth = numberOfObjects + (numberOfObjects - 1) * distanceBetweenCenters;
        
        for (int i = 0; i < numberOfObjects; i++)
        {
            float xPos = i * distanceBetweenCenters - totalWidth / 2.0f;
            result.Add(CreateBuff(buffPrefab[i], new Vector3(xPos, 0, 0), parentTransform));
        }

        return result;
    }

    public GameObject CreateBuff(GameObject prefab, Vector3 pos, Transform parentTransform)
    {
        GameObject obj = Instantiate(prefab, pos, Quaternion.identity);

        obj.transform.SetParent(parentTransform, false); 
        return obj;
    }
    
}
