using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

    public float shieldDuration;

	void Start () {
        GameManager.instance.InvulnerablePlayer();
        AudioToPlay audio = GetComponent<AudioToPlay>();
        if (audio != null) audio.SendAudioToPlay();
        Invoke("ShieldDuration", shieldDuration);
	}
	//Metodo para destruir el escudo pasados shieldDuration segundos.
	void ShieldDuration()
    {
        GameManager.instance.InvulnerablePlayer();
        Destroy(gameObject);
    }
}
