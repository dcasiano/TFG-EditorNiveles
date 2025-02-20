using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadWizard : MonoBehaviour
{
    public float distance;
    public int Cooldown;
    public Transform projectilePool;
    public GameObject fireBall;
    bool FBOnCD =false;
    Vector2 origin, direction;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        LookToPlayer(out origin, distance);
    }

    //Comprueba hacia que lado esta el jugador para dirigir su mirada hacia él y saber el lado de disparo.
    void LookToPlayer(out Vector2 origin, float distance)
    {
        //obtenemos la layerMask del jugador
        int layerMask = 1 << 8;
        //establecemos desde donde saldrá el raycast
        origin = new Vector2(transform.position.x,transform.position.y);
        //comprobamos en que lado esta el jugador y miramos y establecemos el lado de carga
        if (Physics2D.Raycast(origin, Vector2.right, distance, layerMask) || Physics2D.Raycast(origin, Vector2.left, distance, layerMask))
        {
            if (Physics2D.Raycast(origin, Vector2.right, distance, layerMask))
            {
                //Cambia la direccion del mago y guarda el lado en direction para indicarselo a la bola.
                transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y));
                direction = Vector2.right;
            }

            else if (Physics2D.Raycast(origin, Vector2.left, distance, layerMask))
            {
                transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y));
                direction = Vector2.left;
            }

            //Si la bola no está en CD, dispara.
            if (!FBOnCD)
            {
                anim.Play("EnemyWizardAttack");
                InstantiateFireBall();
                FBOnCD = true;
                Invoke("FireBallCD", Cooldown);
            }
        }
    }

    //Control de CD de la bola.
    void FireBallCD()
    {
        FBOnCD = !FBOnCD;
    }

    void InstantiateFireBall()
    {
        GameObject newFireBall = Instantiate(fireBall, transform.position, Quaternion.identity, projectilePool);
        //Asigna el tag y layer Enemy a la bola.
        newFireBall.tag = "Enemy";
        newFireBall.layer = 9;
        //Se cambia su dirección.
        newFireBall.GetComponent<FireBall>().ChangeDirection(direction);
    }
}