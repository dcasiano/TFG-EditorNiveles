using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeDamage : MonoBehaviour {

    // Realiza daño al jugador al colisionar.
    public int damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //En el caso del jugador solo le hará daño si choca con este collider.
            if(collision == collision.GetComponent<PolygonCollider2D>())
            {
                if (transform.parent != null && !transform.parent.name.Contains("Rat") && collision.GetComponent<Life>() != null && !GameManager.instance.GetInvulnerablePlayer()) collision.GetComponent<Life>().LoseLife(damage);

                else if (transform.parent != null && transform.parent.name.Contains("Rat") && collision.GetComponent<Life>() != null && !GameManager.instance.GetInvulnerablePlayer())
                {
                    ContactPoint2D[] contacts = new ContactPoint2D[1];
                    collision.GetContacts(contacts);

                    if (contacts[0].normal.y == 1) collision.GetComponent<Life>().LoseLife(damage);
                }

                else if (transform.parent == null && collision.GetComponent<Life>() != null && !GameManager.instance.GetInvulnerablePlayer()) collision.GetComponent<Life>().LoseLife(damage);
            }
        }

        else if (collision.CompareTag("Boss") && collision.transform.parent.GetComponentInChildren<Life>() != null) collision.transform.parent.GetComponentInChildren<Life>().LoseLife(damage);

        else if (collision.GetComponent<Life>() != null) collision.GetComponent<Life>().LoseLife(damage);
    }
}
