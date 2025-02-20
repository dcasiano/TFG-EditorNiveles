using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public float speed;
    Vector2 room;

    private void Start()
    {
       // GameManager.instance.SetLevelManager().GetCamera(gameObject);
    }

    private void LateUpdate()
    {
        if (room != null) transform.position = Vector3.MoveTowards(transform.position, new Vector3(room.x, room.y, transform.position.z), speed);
    }

    public void MoveCamera(Vector2 thisRoom)
    {
        room = thisRoom;
    }

    public void TeleportCameraToRoom(Vector2 thisRoom)
    {
        transform.position = new Vector3(room.x, room.y, transform.position.z);
    }
}
