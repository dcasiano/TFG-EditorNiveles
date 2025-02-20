using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEnemyProjectileVertical : MonoBehaviour {

    public GameObject enemyProjectile;
    PoolManager pools;
    public float time;
    // Use this for initialization
    void Start()
    {
        pools = GameManager.instance.ReturnPoolManager();
        // el 0.3 al iniciar se debe a que si es 0, al darle al play da errores en todos los proyectiles de los murcielagos porque no tienen aun la referencia al PoolManager.
        InvokeRepeating("Instantiate", 0.3f, time);
    }

    // Update is called once per frame
    void Update()
    {

    }
    //metodo que crea la caca
    void Instantiate()
    {
        GameObject newProjectile = Instantiate(enemyProjectile, transform.position, Quaternion.identity, pools.GetProjectilePool());
    }
}
