using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    public GameObject room;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.instance.ChangeCurrentCheckpoint(transform, room.transform);
    }
}
