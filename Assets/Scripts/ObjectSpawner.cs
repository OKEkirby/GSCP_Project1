using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public int ObjectCount = 10;
    public float ObjectSize = 0.1f;
    public PrimitiveType ObjectType = PrimitiveType.Cube;
    public Material outlineMat;

    [ContextMenu("Spawn Objects")]
    public void SpawnObjects()
    {
        RemoveObjects();

        
        for (var i = 0; i < ObjectCount; i++)
        {
            for (var j = 0; j < i; j++) {
               

                    var newObject = GameObject.CreatePrimitive(ObjectType);
                    newObject.transform.position = new Vector3(i * ObjectSize, j * ObjectSize, 1);

                    newObject.transform.localScale = Vector3.one * ObjectSize;


                    var propertyBlock = new MaterialPropertyBlock();
                    propertyBlock.SetColor("_Color", Random.ColorHSV());
                //newObject.GetComponent<Renderer>().material.color = Random.ColorHSV();
                    newObject.GetComponent<Renderer>().material = outlineMat;
                    newObject.transform.parent = transform;

                
            }

        }


    }

    public void RemoveObjects()
    {
        for(var i = transform.childCount -1; i >= 0; i--)
        {
            if (Application.isPlaying)
            {
                GameObject.Destroy(transform.GetChild(i).gameObject);
            }
            else
            {
                GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }
    }
}
