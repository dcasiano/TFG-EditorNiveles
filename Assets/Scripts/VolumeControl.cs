using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour {

	// se entrega la referencia de este objeto al GM.
	void Start () {
        Slider volume = GetComponent<Slider>();
        GameManager.instance.SetVolumeSlider(volume);
        volume.value = GameManager.instance.GetCurrentVolume();
	}
}
