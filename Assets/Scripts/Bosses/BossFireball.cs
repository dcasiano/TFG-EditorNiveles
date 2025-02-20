using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFireball : MonoBehaviour {
    public GameObject fireball;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Create()
    {
        GameObject newFireball = Instantiate(fireball,transform.position+(Vector3.down*fireball.transform.localScale.x/2),Quaternion.identity);
        newFireball.transform.GetChild(0).GetComponent<FireBall>().ChangeDirection(transform.right * transform.localScale.x);
    }
    public void StartCreate()
    {
        InvokeRepeating("Create", 1, 2);
    }
    public void StopCreate()
    {
        CancelInvoke("Create");
    }
}
