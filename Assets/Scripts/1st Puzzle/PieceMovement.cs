using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMovement : MonoBehaviour
{

    // Cada pieza tiene asignada su coordenada correcta
    public Vector2 correctPosition;

    // Metodo para mover la pieza a la posicion newPos
    public void MovePieceTo(Vector2 newPos)
    {
        transform.position = newPos;
    }

    // Metodo que devuelve informacion sobre la posicion correcta de esta pieza a la hora de realizar CheckSolved()
    public bool CorrectPosition()
    {
        return (Vector2)transform.position == correctPosition;
    }
}
