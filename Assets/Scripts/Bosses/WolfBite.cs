using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfBite : MonoBehaviour {
    Animator wolfAnim;
    Collider2D triggerCollider;
    public int damage;
    //Tiempo entre mordisco y mordisco aproximado.
    float seconds = 0.6f;
    private void Start()
    {
        //Se obtiene el Animator del padre.
        wolfAnim = GetComponentInParent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Como el layer Enemy también interactua con Rooms, se centra al player por su tag.
        if (other.gameObject.CompareTag("Player"))
        {
            wolfAnim.SetBool("Head", true);
            GetCollider(other);
            InvokeRepeating("Damage", seconds, seconds);
        }
    }
    //Método para realizar daño.
    void Damage()
    {
        int x = 0;
        //El valor de la x es aleatorio entre -1 y 1. No puede valer 0 para poder hacer un bounce diagonal.
        while (x == 0)
        {
            x = Random.Range(-1, 2);
        }
        //En este caso se realiza tanto el daño como el knockback.
        if (triggerCollider.GetComponent<Bounce>() != null && !GameManager.instance.GetInvulnerablePlayer()) triggerCollider.GetComponent<Bounce>().BounceTo(x/2, 1);
        if (triggerCollider.GetComponent<Life>() != null && !GameManager.instance.GetInvulnerablePlayer()) triggerCollider.GetComponent<Life>().LoseLife(damage);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Se detiene la animación y el daño.
            wolfAnim.SetBool("Head", false);
            CancelInvoke("Damage");
        }
    }
    //Se guarda other detectado por el trigger para que Damage lo pueda usar, ya que en Invoke no se puede pasar parámetros a los métodos.
    void GetCollider(Collider2D collider)
    {
        triggerCollider = collider;
    }
}
