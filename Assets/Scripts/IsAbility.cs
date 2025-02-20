using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsAbility : MonoBehaviour {

    public string abilityName;
	
	// Update is called once per frame
	void Update () {
		if (GameManager.instance.ReturnAbilityValue(abilityName))
        {
            gameObject.SetActive(true);
        }
	}
}
