using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipOnCollision : MonoBehaviour {
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Move move = transform.parent.GetComponent<Move>();
        if (move != null ) {
            move.ChangeDirection();
        }
        MoveFromAtoB moveAB= transform.parent.GetComponent<MoveFromAtoB>();
        if (moveAB != null)
        {
            moveAB.ChangeBouthSpeed();
        }
    }
}
