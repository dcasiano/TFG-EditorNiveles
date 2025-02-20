using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour {

    public int lifePoints;
    public int currentLife;

    private void Start()
    {
        SetLife(lifePoints);
    }

    //Método que quita vida al jugador.
    public void LoseLife(int damage)
    {
        currentLife -= damage;
        if (GetComponent<BlinkAfterDamage>() != null) GetComponent<BlinkAfterDamage>().Blink();

            //Cuando la vida sea 0 o menor, el jugador muere.
            if (currentLife <= 0) Dead();

        if (tag == "Player") {
            GameManager.instance.ReturnUIManager().UpdateLifeUI(); }

        if (CompareTag("Boss") && GetComponent<BossLifebar>() != null) GetComponent<BossLifebar>().UpdateLifebar(lifePoints, currentLife);
    }

    //Método que destruye al gameObject al morir.
    public void Dead()
    {
        DropObjectOnDeath drop = GetComponent<DropObjectOnDeath>();
        if (drop != null) drop.DropObject();
        DestroyParent destroy = GetComponent <DestroyParent>();
        if (destroy != null) destroy.DestroyP();
        AudioToPlay audio = GetComponent<AudioToPlay>();
        if (audio != null) audio.SendAudioToPlay(); 
        if (tag == "Player") GameManager.instance.Respawn();
        else Destroy(gameObject);
    }

    //Aumenta la vida del jugador 
    public void IncreaseLife(int increase)
    {
        if ((currentLife + increase) > lifePoints)
        {
            currentLife = lifePoints;
        }

        else currentLife += increase;

        if (tag == "Player") GameManager.instance.ReturnUIManager().UpdateLifeUI();
    }

    //devuelve la vida actual
    public int GetActualLife()
    {
        return currentLife;
    }

    //set actual life se hace en el GM para que no se ponga full vida siempre que cambie de pantalla.
    public void SetLife(int life)
    {
        currentLife = life;
    }
}
