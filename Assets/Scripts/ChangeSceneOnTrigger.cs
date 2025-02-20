using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneOnTrigger : MonoBehaviour {

    public string sceneToChange, power;
    private void Start()
    {
        if (GameManager.instance.ReturnAbilityValue(power))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.instance.ChangeScene(sceneToChange);  
    }
    public void ForceScenceChange()
    {
        GameManager.instance.ChangeScene(sceneToChange);
    }
}
