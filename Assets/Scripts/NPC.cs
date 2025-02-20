using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {
    Vector2 starter;
    public string npcName;

    [TextArea(3, 10)]
    public string[] sentences;
    private void Update()
    {
        LookToPlayer(out starter);
        Debug.DrawRay(starter, Vector2.right, Color.yellow);
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision == collision.GetComponent<PolygonCollider2D>())
        {
            GameManager.instance.ReturnUIManager().EnableDialogueBox(npcName, sentences);
        }
    }
    void LookToPlayer(out Vector2 starter)
    {
        //obtenemos la layerMask del jugador
        int layerMask = 1 << 8;
        //establecemos desde donde saldrá el raycast
        starter = new Vector2(transform.position.x, transform.position.y+0.2f);
        //comprobamos en que lado esta el jugador y miramos hacia allo
        if (Physics2D.Raycast(starter, Vector2.right, 3000, layerMask))
        {
            transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y));
        }
        else if (Physics2D.Raycast(starter, Vector2.left, 3000, layerMask))
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y));
        }
    }
}
