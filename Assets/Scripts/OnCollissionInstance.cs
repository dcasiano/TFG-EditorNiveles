using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollissionInstance : MonoBehaviour {

    public GameObject gameObjectToInstance;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D[] contacts = new ContactPoint2D[1];
        collision.GetContacts(contacts);

        GameObject newGOgbject = Instantiate(gameObjectToInstance, new Vector3 (contacts[0].point.x, contacts[0].point.y + 5f, 0), Quaternion.identity, null);
    }
}
