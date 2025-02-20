using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpOnTop : MonoBehaviour {
    //Si el jugador colisiona con la parte superior de la rata la aplasta, matandola y rebotando

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Al producirse la colisión se genera un array de puntos de contacto en el que guardaremos la información de la colisión
        ContactPoint2D[] contacts = new ContactPoint2D[1];
        collision.GetContacts(contacts);

        //La variable "angle" es el angulo entre el vector "right" y la normal del punto de contacto
        float angle = Vector2.Angle((Vector2)transform.right, contacts[0].normal);

        //Si el objeto con el que se ha colisionado es el jugador y "angle" es practicamente 0 sabemos que la colisión ha sido por arriba además se comprueba que la posicion del jugador mas1/4 de su escala para que si salta algun frame no de fallos(como pasa con 1/2 de la escala) estan por encima de la posicion del objeto.
         if (collision.gameObject.tag == "Player" && angle < 0.01&&transform.position.y<=(collision.transform.position.y-collision.transform.localScale.y/4))
         {
            //Obtenemos el script "Life" de la rata y restamos a su vida la misma cantidad que tenga actualmente, haciendo que esta caiga a 0
            Life ratLife = transform.parent.GetComponent<Life>();
            ratLife.LoseLife(ratLife.GetActualLife());

            //Comprobamos si el jugador tiene el script "Bounce" y si es asi hacemos que el script "Bounce" lo haga rebotar hacia arriba.
            if (collision.gameObject.GetComponent<Bounce>() != null)
            {
                Bounce player = collision.gameObject.GetComponent<Bounce>();
                player.BounceTo(0,1);
            }
         }
    }

}
