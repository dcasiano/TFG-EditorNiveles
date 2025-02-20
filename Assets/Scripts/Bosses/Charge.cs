using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : MonoBehaviour {
    public float animationTime=1, maxSpeed=40;
    public int aceleration=8, chargeCDTime=2;
    Rigidbody2D rigidB;
    Animator wolfAnim;
    bool chargeOnCD = false;
    bool onAnimation = false;
    Vector2 charge, starter;
	// Use this for initialization
	void Start () {
        rigidB = GetComponent<Rigidbody2D>();
        wolfAnim = GetComponent<Animator>();
    }
    private void Update()
    {
        wolfAnim.SetFloat("VelocityX",Mathf.Abs(rigidB.velocity.x));
        LookToPlayer(out starter);
        Debug.DrawRay(starter, charge, Color.yellow);
    }
    void FixedUpdate()
    {
        //Realizará la carga siempre que el estado del boss sea charging.
        if(GameManager.instance.ReturnBossManager().WolfState() == WolfEnums.charging && !onAnimation)
        {
            rigidB.AddForce(charge * aceleration * rigidB.mass);
            rigidB.velocity = new Vector2(Mathf.Clamp(rigidB.velocity.x,-maxSpeed,maxSpeed), rigidB.velocity.y);
        }
    }
    //Comprueba hacia que lado esta el jugador para dirigir su mirada hacia él y saber el lado de la carga.
    void LookToPlayer(out Vector2 starter)
    {
        //obtenemos la layerMask del jugador
        int layerMask = 1 << 8;
        //establecemos desde donde saldrá el raycast
    starter = new Vector2(transform.position.x, transform.position.y - transform.localScale.y / 2);
        //comprobamos en que lado esta el jugador y miramos y establecemos el lado de carga
        if (Physics2D.Raycast(starter, Vector2.right, 3000, layerMask))
        {
            transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y));
            charge = Vector2.right;
        }
        else if (Physics2D.Raycast(starter, Vector2.left, 3000, layerMask)) 
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y));
            charge = Vector2.left;
        }
    }
    //Método llamado por el bossManager que inicia el tiempo de enfriamiento.
    public void StartChargeCD()
    {
        //Aqui se inicia la animación.
        onAnimation = true;
        wolfAnim.SetBool("PreCharge", true);
            Invoke("AnimationFinished",animationTime);
            chargeOnCD = true;
            Invoke("ChangeCD", chargeCDTime);
    }
    //No permite realizar la carga hasta que se realice este método dentro de "animationTime" segundos.
    void AnimationFinished()
    {
        onAnimation = false;
        wolfAnim.SetBool("PreCharge", false);
    }
    //Cambia el CD para que la habilidad vuelva a estar disponible tras el tiempo "chargeCDTime".
    void ChangeCD()
    {
        chargeOnCD = false;
    }
    //Este método permite ver al bossManager si la carga está en cooldown, para no empezar otra al mismo tiempo.
    public bool ChargeCD()
    {
        return chargeOnCD;
    }
}
