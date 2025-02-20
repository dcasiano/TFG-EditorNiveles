using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiagonalKnockBack : MonoBehaviour {
    Rigidbody2D rigidbody2D;
	// Use this for initialization
	void Start () {
        rigidbody2D = transform.parent.GetComponent<Rigidbody2D>();
	}
    // si choca con el jugador y se esta moviendo en X entonces aplica una fuerza diagonal
   /* private void OnCollisionEnter2D(Collision2D collision)
    {if(rigidbody2D.velocity.x!=0&&collision.gameObject.tag.Equals("Player"))
        rigidbody2D.AddForce(new Vector2(-(rigidbody2D.velocity.x / Mathf.Abs(rigidbody2D.velocity.x)), 1)*rigidbody2D.mass*9,ForceMode2D.Impulse);
    }*/
}
