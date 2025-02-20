using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour {
    float posInicial;
    public float speed;
    public float frecuency; //tiempo que tarda en cambiar de dirección
    bool eje;  //indica si la plataforma se moverá horizontal o verticalmente
	// Use this for initialization
	void Start () {
        if (gameObject.tag == "Horizontal")
        {
            eje = false;
            posInicial = gameObject.transform.position.x;
        }
        else if (gameObject.tag == "Vertical")
        {
            eje = true;
            posInicial = gameObject.transform.position.y;
        }
        InvokeRepeating("ChangeDirection",0 , frecuency);

    }
	
	// Update is called once per frame
	void Update () {
        if (!eje)
            transform.Translate(new Vector2(1, 0) * Time.deltaTime * speed, Space.World);
        else
            transform.Translate(new Vector2(0, 1) * Time.deltaTime * speed, Space.World);
    }
    private void FixedUpdate()
    {

    }
     void ChangeDirection()
    {
        speed = -speed;
    }
   
   
}
