using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

    public float speed;
    Rigidbody2D ratRigidbody;

	// Use this for initialization
	void Start () {

        ratRigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
    private void FixedUpdate()
    {
        ratRigidbody.velocity = new Vector2(speed,ratRigidbody.velocity.y);
    }

    public void ChangeDirection()
    {
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        speed = -speed;
    }

    public float ReturnSpeed()
    {
        return speed;
    }

    public void UpdateSpeed(float updatedSpeed)
    {
        speed = updatedSpeed;
    }
}
