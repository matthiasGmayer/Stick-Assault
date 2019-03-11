using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmCollision : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objects;
    [SerializeField]
    private GameObject[] objectsToIgnore;

    void Awake()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            for (int j = 0; j < objectsToIgnore.Length; j++)
            {
                Physics2D.IgnoreCollision(objects[i].GetComponent<Collider2D>(), objectsToIgnore[j].GetComponent<Collider2D>());
            }
        }
    }
}
