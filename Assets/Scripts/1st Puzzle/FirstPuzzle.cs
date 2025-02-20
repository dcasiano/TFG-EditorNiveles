using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPuzzle : MonoBehaviour
{
    // Distancia diagonal entre piezas
    public Vector2 diagonalDistance;
    PieceMovement[] pieces;
    PieceMovement frame;
    bool selected = false;
    public int pieceSelected;

    // Use this for initialization

    void Start()
    {
        frame = transform.GetChild(0).GetComponent<PieceMovement>();
        pieces = transform.GetChild(1).GetComponentsInChildren<PieceMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        if (!GameManager.instance.IsOnMenu())
        {
            // Se escoge el vector inputVec del metodo CheckSpot dependiendo de la flecha pulsada
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                CheckSpot(new Vector2(-Mathf.Abs(diagonalDistance.x), 0), -1);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                CheckSpot(new Vector2(Mathf.Abs(diagonalDistance.x), 0), 1);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                CheckSpot(new Vector2(0, Mathf.Abs(diagonalDistance.y)), -3);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                CheckSpot(new Vector2(0, -Mathf.Abs(diagonalDistance.y)), 3);
            }

            else if (Input.GetKeyDown(KeyCode.Space))
            {
                selected = !selected;
                pieceSelected = FindSelectedPiece();
                ChangeFrameColour();
            }
        }
    }

    // Metodo para analizar si es posible el movimiento de pieza
    void CheckSpot(Vector2 inputVec, int direction)
    {
        int i = 0;
        bool found = false;

        while (i < pieces.Length && !found)
        {

            // Si las coordenadas de la pieza "Empty" + el vector input coinciden con la posicion de una de las piezas del array, intercambia la posicion entre ellas 
            if (selected && (Vector2)frame.transform.position + inputVec == (Vector2)pieces[i].transform.position)
            {
                SwapPosition(i, pieceSelected, inputVec);
                CheckSolved();
                found = true;
            }

            else if (!selected && (Vector2)frame.transform.position + inputVec == (Vector2)pieces[i].transform.position)
            {
                ChangePosition(inputVec);
                found = true;
            }

            i++;
        }
    }

    // Metodo para realizar el cambio de posición del marco

    void ChangePosition(Vector2 inputVec)
    {
        frame.MovePieceTo((Vector2)frame.transform.position + inputVec);
    }

    // Metodo para realizar el cambio de posicion entre dos piezas
    void SwapPosition(int i, int j, Vector2 inputVec)
    {
        pieces[i].MovePieceTo(frame.transform.position);
        frame.MovePieceTo((Vector2)frame.transform.position + inputVec);
        pieces[j].MovePieceTo((Vector2)frame.transform.position);
    }

    int FindSelectedPiece()
    {
        if (selected)
        {
            int i = 0;
            bool found = false;

            while (i < pieces.Length && !found)
            {
                if ((Vector2)frame.transform.position == (Vector2)pieces[i].transform.position)
                {
                    found = true;
                }

                else i++;
            }

            if (found) return i;
            else return -1;
        }

        else return -1;
    }

    void ChangeFrameColour()
    {
        if (selected) frame.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
        else frame.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    // Metodo para revisar si se ha completado
    void CheckSolved()
    {
        int i = 0;

        while (i < pieces.Length && pieces[i].CorrectPosition())
        {
            i++;
        }

        if (i == pieces.Length)
        {
            Debug.Log("Completado!");
            GameManager.instance.SetAbilityTrue("Fireball");
            GameManager.instance.Respawn();
        }
    }
}
