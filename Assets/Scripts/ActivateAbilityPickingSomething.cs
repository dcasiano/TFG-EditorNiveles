using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAbilityPickingSomething : MonoBehaviour {
    public string abilityName;

    [TextArea(3, 10)]
    public string[] sentences;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision == collision.GetComponent<PolygonCollider2D>())
        {
            GameManager.instance.SetAbilityTrue(abilityName);
            GameManager.instance.ReturnUIManager().EnableAbilityIcons();
            GameManager.instance.ReturnUIManager().EnableDialogueBox(abilityName, sentences);
            Destroy(gameObject);
        }
   
    }
}
