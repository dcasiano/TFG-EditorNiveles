using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParent : MonoBehaviour {

public void DestroyP()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }
}
