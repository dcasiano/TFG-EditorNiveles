using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawn : MonoBehaviour {

    public GameObject pointPrefab;

    //El método respawn recibe la información concerniente a este objeto almacenada en el array del script que resetea las salas y la aplica a esta nueva instancia
    public void Respawn(Vector3[] wayPointPositions, Vector2 scale, Vector2 speed)
    {
        if (name.Contains("Rat")) transform.GetChild(0).localScale = new Vector3(scale.x, scale.y, transform.localScale.z);

        else transform.localScale = new Vector3(scale.x, scale.y, transform.localScale.z);
        UpdateSpeed(speed);

        int currentWayPoint = 0;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name.Contains("Point") || transform.GetChild(i).name.Contains("Rotation"))
            {
                transform.GetChild(i).gameObject.transform.position = wayPointPositions[currentWayPoint];
                currentWayPoint++;
            }
        }

        for (int i = currentWayPoint; i < wayPointPositions.Length; i++)
        {
            if (pointPrefab != null)
            {
                GameObject newPoint = Instantiate(pointPrefab, wayPointPositions[currentWayPoint], Quaternion.identity, transform);
                currentWayPoint++;
            }
        }
    }

    //Este metodo recibe la velocidad guardada al cargarse la escena y la aplica al objeto que se acaba de instanciar
    void UpdateSpeed(Vector2 speed)
    {
        if (gameObject.GetComponentInChildren<AlternativePlatformMovement>() != null) gameObject.GetComponentInChildren<AlternativePlatformMovement>().UpdateSpeed(speed.x);

        else if (gameObject.GetComponentInChildren<Move>() != null) gameObject.GetComponentInChildren<Move>().UpdateSpeed(speed.x);

        else if (gameObject.GetComponentInChildren<MoveAroundPlatforms>() != null) gameObject.GetComponentInChildren<MoveAroundPlatforms>().UpdateSpeed(speed.x);

        else if (gameObject.GetComponentInChildren<MoveFromAtoB>() != null) gameObject.GetComponentInChildren<MoveFromAtoB>().UpdateSpeed(speed.x, speed.y);
    }
}
 