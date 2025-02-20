using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterXSeconds : MonoBehaviour {
    public float seconds;
	// Use this for initialization
	void Start () {
        Invoke("Destroy", seconds);

    }
	
	// Update is called once per frame
	void Destroy () {
        Destroy(gameObject);
	}
}
