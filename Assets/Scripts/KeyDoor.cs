using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour {
    bool locked = true;
    public string dialogueName;

    [TextArea(3, 10)]
    public string[] sentences;

    Collider2D coll;
    public void Unlock()
    {
        locked = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        coll = collision.collider;
        if(coll == coll.GetComponent<PolygonCollider2D>())
        {
            //Si el método Unlock ha sido llamado por la llave y se cambió el booleano locked, la puerta se podrá abrir en colisión.
            if (!locked)
            {
                Destroy(gameObject);
            }
            //si está cerrada, indica al jugador mediante un cuadro de texto que necesita la llave para pasar por esa puerta.
            else
            {
                GameManager.instance.ReturnUIManager().EnableDialogueBox(dialogueName, sentences);
            }
        }
    }
}
