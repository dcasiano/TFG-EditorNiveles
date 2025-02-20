using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesKnockback : MonoBehaviour {

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Bounce>() != null && !GameManager.instance.GetInvulnerablePlayer()) collision.GetComponent<Bounce>().BounceTo(0,1);
    }

}
