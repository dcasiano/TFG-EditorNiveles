using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioToPlay : MonoBehaviour {

    public string name;

    // Método para modificar el nombre del audio a enviar.
    public void SendThisAudioToPlay(string newName)
    {
        name = newName;
        SendAudioToPlay();
    } 

    // Método para enviar el audio con nombre name al AudioManager (enviará el indicado en el editor si no se ha modificado).
    public void SendAudioToPlay()
    {
        GameManager.instance.ReturnAudioManager().PlayAudio(name);
    }
}
