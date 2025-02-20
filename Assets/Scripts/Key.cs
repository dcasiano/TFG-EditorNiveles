using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {
    KeyDoor door;

    private void Start()
    {
        door = GetComponentInParent<KeyDoor>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Desbloquea la puerta y se destruye.
        door.Unlock();
        Destroy(gameObject);
    }
}
