using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emitter : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    public Material end;
    LineRenderer lineRen;
    bool stop;

    //Usamos la componente de unity lineRenderer y marcamos el punto inicial.
    void Start()
    {
        lineRen = GetComponent<LineRenderer>(); //Pinta la línea formada por el rayo.        
        lineRen.SetPosition(0, transform.position);
    }

    //Bucle del rayo.
    void Update()
    {
        //Creamos el rayo (raycast) en la misma posición inicial y con una dirección asignada (right).
        stop = false;
        ray = new Ray(transform.position, Vector3.up);
        //Comienza 
        lineRen.positionCount = 1;
        //Bucle para reflexion de "reflections" veces.
        while (!stop)
        {
            //Se detecta si hay colision en el raycast con "hit".
            if (Physics.Raycast(ray.origin, ray.direction, out hit, 100))
            {
                //Golpe detectado, actualiza el renderizado de la línea.
                lineRen.positionCount++;
                //Se crea la linea desde el punto anterior al nuevo punto de colisión.
                lineRen.SetPosition(lineRen.positionCount - 1, hit.point);
                //Se crea el vector de reflexión con el rayo incidente de ray y la normal del punto de colisión.
                Vector3 reflection = Vector3.Reflect(ray.direction, hit.normal);
                //Se asigna a ray el rayo reflejado para detectar las siguientes colisiones.
                ray = new Ray(hit.point, reflection);
                //Si no tocamos un objeto con tag "Mirror", detenemos bucle.
                if (hit.collider.tag != "Mirror")
                {
                    stop = true;
                    //Comprueba si ha finalizado.
                    if (hit.transform.tag == "EndMirror")
                    {
                        GetComponentInChildren<SelectMirror>().StopInput();
                        MeshRenderer mesh = hit.transform.GetComponent<MeshRenderer>();
                        mesh.material = end;
                        GameManager.instance.SetAbilityTrue("Shield");
                        GameManager.instance.ChangeScene("Zona2");

                    }
                }

            }
            //Golpe no detectado, actualiza el renderizado de la línea hasta la longitud actual.
            else
            {
                lineRen.positionCount++;
                lineRen.SetPosition(lineRen.positionCount - 1, ray.origin + ray.direction * 20);
                stop = true;
            }
        }
    }
}
