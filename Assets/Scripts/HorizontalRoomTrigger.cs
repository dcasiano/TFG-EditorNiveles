using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalRoomTrigger : MonoBehaviour
{

    //Room1: Izquierda. Room2: Derecha.
    public GameObject leftRoom, rightRoom, cameraPosition;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Compara las distancias entre el jugador y el trigger, para saber el lado del OnTriggerExit. Dependiendo de esto, movera la camara hacia la sala correspondiente.
            if (transform.position.x < collision.transform.position.x)
            {
                GameManager.instance.SetLevelManager().MoveCamera(new Vector2(rightRoom.transform.position.x, rightRoom.transform.position.y));
                //Si se produce OnTriggerExit en la sala que se encuentra la camara, no se produce el respawn.
                if (rightRoom.GetComponentInChildren<RoomResetManager>() != null && (Vector2)cameraPosition.transform.position != (Vector2)rightRoom.transform.position) rightRoom.GetComponentInChildren<RoomResetManager>().RespawnEnemies();
                if (leftRoom.GetComponentInChildren<RoomResetManager>() != null) leftRoom.GetComponentInChildren<RoomResetManager>().DestroyEnemies();
            }
            else
            {
                GameManager.instance.SetLevelManager().MoveCamera(new Vector2(leftRoom.transform.position.x, leftRoom.transform.position.y));
                if (leftRoom.GetComponentInChildren<RoomResetManager>() != null && (Vector2)cameraPosition.transform.position != (Vector2)leftRoom.transform.position) leftRoom.GetComponentInChildren<RoomResetManager>().RespawnEnemies();
                if (rightRoom.GetComponentInChildren<RoomResetManager>() != null) rightRoom.GetComponentInChildren<RoomResetManager>().DestroyEnemies();
            }

        }
    }
}