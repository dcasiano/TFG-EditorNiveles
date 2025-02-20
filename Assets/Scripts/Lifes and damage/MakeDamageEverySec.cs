using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeDamageEverySec : MonoBehaviour {

    public int damage;
    public float seconds;
    Collider2D triggerCollider;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other == other.GetComponent<PolygonCollider2D>())
        {
            GetCollider(other);
            InvokeRepeating("Damage", 0, seconds);
        }
            
    }
    //Método para realizar daño.
    void Damage()
    {
        if(triggerCollider!=null)
        if (triggerCollider.GetComponent<Life>() != null && !GameManager.instance.GetInvulnerablePlayer())
        {
            triggerCollider.GetComponent<Life>().LoseLife(damage);
        }
    }
    //Se guarda other detectado por el trigger para que Damage lo pueda usar, ya que en Invoke no se puede pasar parámetros a los métodos.
    void GetCollider(Collider2D collider) {
        triggerCollider = collider;
    }
    //Al salir se detiene el invoke, permitiendo que se inicie otro si se entra de nuevo (de esta forma no se superponen).
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == collision.GetComponent<PolygonCollider2D>())
        {
            CancelInvoke("Damage");
        }
        
    }
}