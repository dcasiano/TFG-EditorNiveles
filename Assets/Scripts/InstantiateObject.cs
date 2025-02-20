using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateObject : MonoBehaviour
{
    public GameObject gameObjectToInstance;

    public void Instantiate(Vector2 vector)
    {
        GameObject newGOgbject = Instantiate(gameObjectToInstance, vector, Quaternion.identity, null);
        newGOgbject.transform.Rotate(Vector3.forward, 90);
    }
}
