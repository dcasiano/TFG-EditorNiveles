using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFromAtoB : MonoBehaviour {

    Rigidbody2D bat;
    public Transform pointA, pointB;
    Animator anime;

    [System.Serializable]
    public struct Speed
    {
        public float x, y;
    }

    [SerializeField]
    public Speed speed;

    // Use this for initialization
    void Start()
    {
        bat = GetComponent<Rigidbody2D>();
        transform.position = pointA.position;
        anime = GetComponent<Animator>();
    }

    private void Update()
    {
        ChangeXSpeed();
        ChangeYspeed();
        ChangeScale();
    }

    //fisicas
    private void FixedUpdate()
    {
        bat.velocity = new Vector2(speed.x, speed.y);
    }

    //si collisiona con algo !=jugador o != de su caca cambia la velocidad en X e Y
    public void ChangeBouthSpeed()
    {
   
            speed.x = -speed.x;
            speed.y = -speed.y;
    }

    //cambiamos la velocidad en X
    void ChangeXSpeed()
    {
            if (this.transform.position.x >= pointB.position.x)
            {
                speed.x = -Mathf.Abs(speed.x);
            }

            else if (this.transform.position.x <= pointA.position.x)
            {
                speed.x = Mathf.Abs(speed.x);
            }
    }

    //cambaimos la velocidad en Y
    void ChangeYspeed()
    {
        if (transform.position.y <= pointA.position.y )
        {
            speed.y = Mathf.Abs(speed.y);
        }
        else if (transform.position.y >= pointB.position.y)
        {
            speed.y = -Mathf.Abs(speed.y);
        }
    }

    //cambiamos la escala
    void ChangeScale()
    {
        float       scaleX = transform.localScale.x;
        if (speed.x > 0) scaleX = Mathf.Abs(scaleX);
        else if (speed.x < 0) scaleX = -Mathf.Abs(scaleX);
        transform.localScale = new Vector2(scaleX, transform.localScale.y);
    }

    public Speed ReturnSpeed()
    {
        return speed;
    }

    public void UpdateSpeed(float updatedSpeedX, float updatedSpeedY)
    {
        speed.x = updatedSpeedX;
        speed.y = updatedSpeedY;
    }
}
