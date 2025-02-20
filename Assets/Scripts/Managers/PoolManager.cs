using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour {
    Transform[] pools;
    // Use this for initialization
    void Start () {
        GameManager.instance.ThisPoolManager(this);
        // Se recoge los transform de todos los hijos de PoolManager
        pools = GetComponentsInChildren<Transform>();
	}
	//Metodos para devolver los transfom de los pools.
	public Transform GetProjectilePool()
    {
        return pools[1];
    }
    public Transform GetObjectPool()
    {
        return pools[2];
    }
}
