using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomResetManager : MonoBehaviour
{
    public bool loadedOnStart = false;

    public GameObject[] prefabs;

    [System.Serializable]
    struct ResettableObject
    {
        public GameObject enemyObject;
        public Vector2 enemyScale;
        public Vector2 enemySpeed;
        public Vector3 spawnPosition;
        public Vector3[] wayPointPositions;
    }

    [SerializeField]
    ResettableObject[] enemyArray;

    //Al cargarse la escena este script almacena toda la información importante de los objetos a resetear en la sala y los destruye
    void Start()
    {
        StoreInfo();
        DestroyEnemies();

        if (loadedOnStart) RespawnEnemies();
    }

    //Este metodo almacena en un array de structs la información importante de los objetos a resetear: el prefab, la escala, la velocidad, la posición en la que spawnean y los puntos por los que se guia su movimiento
    void StoreInfo()
    {
        enemyArray = new ResettableObject[transform.childCount];

        for (int i = 0; i < enemyArray.Length; i++)
        {
            for (int j = 0; j < prefabs.Length; j++)
            {
                if (transform.GetChild(i).gameObject.name.Contains(prefabs[j].name)) enemyArray[i].enemyObject = prefabs[j];
            }

            StoreWaypoints(i, "Point");
            StoreScale(i);
            StoreSpeed(i);
            enemyArray[i].spawnPosition = transform.GetChild(i).transform.position;
        }
    }

    //Este metodo almacena los puntos por los que se guia el movimiento de ciertos objetos
    void StoreWaypoints(int i, string pointName)
    {
        int j = 0;

        for (int k = 0; k < transform.GetChild(i).transform.childCount; k++)
        {
            if (transform.GetChild(i).transform.GetChild(k).name.Contains(pointName)) j++;
        }

        enemyArray[i].wayPointPositions = new Vector3[j];
        j = 0;

        for (int k = 0; k < transform.GetChild(i).transform.childCount; k++)
        {
            if (transform.GetChild(i).transform.GetChild(k).name.Contains(pointName))
            {
                enemyArray[i].wayPointPositions[j] = transform.GetChild(i).transform.GetChild(k).transform.position;
                j++;
            }
        }
    }

    //Este metodo almacena la escala de los objetos. En ciertos casos será la del mismo objeto y en otros la de uno de sus hijos
    void StoreScale(int i)
    {
        if (transform.GetChild(i).name.Contains("Wizard") || transform.GetChild(i).name.Contains("Platform"))
        {
            enemyArray[i].enemyScale.x = transform.GetChild(i).transform.localScale.x;
            enemyArray[i].enemyScale.y = transform.GetChild(i).transform.localScale.y;
        }

        else for (int j = 0; j < transform.GetChild(i).childCount; j++)
            {
                for (int k = 0; k < prefabs.Length; k++)
                {
                    if (transform.GetChild(i).transform.GetChild(j).name.Contains(prefabs[k].name) && !transform.GetChild(i).name.Contains("Wizard") && !transform.GetChild(i).name.Contains("Platform"))
                    {
                        enemyArray[i].enemyScale.x = transform.GetChild(i).transform.GetChild(j).transform.localScale.x;
                        enemyArray[i].enemyScale.y = transform.GetChild(i).transform.GetChild(j).transform.localScale.y;
                    }
                }
            }
    }

    //Este método almacena la velocidad de los objetos dependiendo del tipo de movimiento que tengan
    void StoreSpeed(int i)
    {
        if (transform.GetChild(i).GetComponentInChildren<AlternativePlatformMovement>() != null)
        {
            enemyArray[i].enemySpeed.x = transform.GetChild(i).GetComponentInChildren<AlternativePlatformMovement>().ReturnSpeed();
            enemyArray[i].enemySpeed.y = 0f;
        }

        else if (transform.GetChild(i).GetComponentInChildren<Move>() != null)
        {
            enemyArray[i].enemySpeed.x = transform.GetChild(i).GetComponentInChildren<Move>().ReturnSpeed();
            enemyArray[i].enemySpeed.y = 0f;
        }

        else if (transform.GetChild(i).GetComponentInChildren<MoveAroundPlatforms>() != null)
        {
            enemyArray[i].enemySpeed.x = transform.GetChild(i).GetComponentInChildren<MoveAroundPlatforms>().ReturnSpeed();
            enemyArray[i].enemySpeed.y = 0f;
        }

        else if (transform.GetChild(i).GetComponentInChildren<MoveFromAtoB>() != null)
        {
            enemyArray[i].enemySpeed.x = transform.GetChild(i).GetComponentInChildren<MoveFromAtoB>().ReturnSpeed().x;
            enemyArray[i].enemySpeed.y = transform.GetChild(i).GetComponentInChildren<MoveFromAtoB>().ReturnSpeed().y;
        }
    }

    //Este método destruye a los objetos ya existentes (para evitar que se creen más de la cuenta), y los vuelve a hacer respawnear, haciendo que cada uno de ellos ajuste sus variables dependiendo de la información almacenada
    public void RespawnEnemies()
    {
        DestroyEnemies();

        for (int i = 0; i < enemyArray.Length; i++)
        {
            GameObject enemySpawned = Instantiate(enemyArray[i].enemyObject, enemyArray[i].spawnPosition, Quaternion.identity, transform);
            enemySpawned.GetComponent<EnemyRespawn>().Respawn(enemyArray[i].wayPointPositions, enemyArray[i].enemyScale, enemyArray[i].enemySpeed);
        }
    }

    //Este método destruye a todos los objetos hijos de el objeto que almacena la información
    public void DestroyEnemies()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
