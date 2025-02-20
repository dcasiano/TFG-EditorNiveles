using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackEverySec : MonoBehaviour {

    public float seconds, knockbackBoostX=1, knockbackBoostY;
    Collider2D triggerCollider;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == other.GetComponent<PolygonCollider2D>())
        {
            GetCollider(other);
        InvokeRepeating("KnockBack", 0, seconds);
        }

    }
    //Método para realizar KnockBack con el componente Bounce del jugador.
    void KnockBack()
    {
        float x = 0;
        //El valor de la x es aleatorio entre -1 y 1. No puede valer 0 para poder hacer un bounce diagonal.
        while( x == 0)
        {
            x = Random.Range(-1, 2); 
           
        }
        if(triggerCollider!=null)
        if (triggerCollider.GetComponent<Bounce>() != null && !GameManager.instance.GetInvulnerablePlayer()) triggerCollider.GetComponent<Bounce>().BounceTo(x*knockbackBoostX/2,knockbackBoostY);
    }
    //Se guarda other detectado por el trigger para que Damage lo pueda usar, ya que en Invoke no se puede pasar parámetros a los métodos.
    void GetCollider(Collider2D collider)
    {
        triggerCollider = collider;
    }
    //Al salir del trigger se detiene el invoke, permitiendo que se inicie otro si se entra de nuevo (de esta forma no se superponen).
    private void OnTriggerExit2D(Collider2D collision)
    {
        CancelInvoke("KnockBack");
    }
}
