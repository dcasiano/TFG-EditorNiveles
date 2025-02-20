using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternativePlatformMovement : MonoBehaviour {

    public float speedMovement;
    Transform[] points; // trazado que realizará el movimiento
    Transform currentPoint; // punto actual al que se moverá la plataforma
    public int selector; //variable que indica a qué plataforma se moverá

	// Use this for initialization
	void Start ()
    {
        int numberOfPoints = 0;

        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (transform.parent.GetChild(i).name.Contains("Point")) numberOfPoints++;
        }

        points = new Transform[numberOfPoints];
        int j = 0;

        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (transform.parent.GetChild(i).name.Contains("Point"))
            {
                points[j] = transform.parent.GetChild(i).transform;
                j++;
            }
        }

        currentPoint = points[selector];    //se indica el punto de inicio del movimiento
	}
	
	// Update is called once per frame
	void Update () {
        //La plataforma se moverá hasta el siguiente punto del trazado que corresponda
        transform.position = Vector2.MoveTowards(transform.position, currentPoint.transform.position, speedMovement * Time.deltaTime);  

        //comprueba si la plataforma ha llegado al punto
        if ( new Vector2 (transform.position.x, transform.position.y) == new Vector2 (currentPoint.transform.position.x, currentPoint.transform.position.y))
        {
            //si es así, cambia al siguiente punto (la fórmula hace que si llega al máximo de puntos vuelve al primero)
            selector = (selector + 1) % points.Length;
            currentPoint = points[selector];

        }

	}

    public float ReturnSpeed()
    {
        return speedMovement;
    }

    public void UpdateSpeed(float updatedSpeed)
    {
        speedMovement = updatedSpeed;
    }
}
