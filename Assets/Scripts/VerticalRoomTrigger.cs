using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalRoomTrigger : MonoBehaviour
{

    //Room1: Izquierda. Room2: Derecha.
    public GameObject upRoom, downRoom, cameraPosition;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Compara las distancias entre el jugador y el trigger, para saber el lado del OnTriggerExit. Dependiendo de esto, movera la camara hacia la sala correspondiente.
            if (transform.position.y > collision.transform.position.y)
            {
                GameManager.instance.SetLevelManager().MoveCamera(new Vector2(downRoom.transform.position.x, downRoom.transform.position.y));
                //Si se produce OnTriggerExit en la sala que se encuentra la camara, no se produce el respawn.
                if (downRoom.GetComponentInChildren<RoomResetManager>() != null && (Vector2)cameraPosition.transform.position != (Vector2)downRoom.transform.position) downRoom.GetComponentInChildren<RoomResetManager>().RespawnEnemies();
                if (upRoom.GetComponentInChildren<RoomResetManager>() != null) upRoom.GetComponentInChildren<RoomResetManager>().DestroyEnemies();
            }
            else
            {
                GameManager.instance.SetLevelManager().MoveCamera(new Vector2(upRoom.transform.position.x, upRoom.transform.position.y));
                if (upRoom.GetComponentInChildren<RoomResetManager>() != null && (Vector2)cameraPosition.transform.position != (Vector2)upRoom.transform.position) upRoom.GetComponentInChildren<RoomResetManager>().RespawnEnemies();
                if (downRoom.GetComponentInChildren<RoomResetManager>() != null) downRoom.GetComponentInChildren<RoomResetManager>().DestroyEnemies();
            }

        }
    }
}