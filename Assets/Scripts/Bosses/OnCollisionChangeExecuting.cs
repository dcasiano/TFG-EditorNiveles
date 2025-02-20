using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionChangeExecuting : MonoBehaviour {
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(GameManager.instance.ReturnBossManager().WolfState()!=WolfEnums.idle)
        GameManager.instance.ReturnBossManager().ChangeBossState("wolf","idle");
    }
}
