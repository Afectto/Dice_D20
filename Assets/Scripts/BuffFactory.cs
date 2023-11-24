using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffFactory : MonoBehaviour
{
    float distanceBetweenCenters = 120f;
    private float totalWidth;
    
    public void CreateBuffGroup(List<GameObject> buffPrefab, Transform parentTransform)
    {
        var numberOfObjects = buffPrefab.Count;

        totalWidth = numberOfObjects + (numberOfObjects - 1) * distanceBetweenCenters;
        for (int i = 0; i < numberOfObjects; i++)
        {
            float xPos = i * distanceBetweenCenters - totalWidth / 2.0f;
            CreateBuff(buffPrefab[i], new Vector3(xPos, 0, 0), parentTransform);
        }
    }

    public void CreateBuff(GameObject prefab, Vector3 pos, Transform parentTransform)
    {
        GameObject obj = Instantiate(prefab, pos, Quaternion.identity);

        Vector3 localPosition = obj.transform.localPosition;
        Vector3 localScale = obj.transform.localScale;

        obj.transform.parent = parentTransform;

        obj.transform.localPosition = localPosition;
        obj.transform.localScale = localScale;
    }
    
}
