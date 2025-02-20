using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMirror : MonoBehaviour
{
    int index;
    bool input = false;
    MovMirror[] mirror;
	// Use this for initialization
	void Start ()
    {
        // Se obtiene el componente MovMirror de los hijos en un array.
        mirror = GetComponentsInChildren<MovMirror>();
        index = 0;
        Invoke("iniciar", 0.02f);
    }

    void iniciar()
    {
        mirror[index].changeSelection();
        input = true;
    }
	
	// Cambia el espejo seleccionado.
	void Update ()
    {
        if (Input.GetKeyDown("down") && input) //Selección right.
        {
            int aux = index;
            index++;
            ChangeSelect(aux,ref index);
        }
        else if (Input.GetKeyDown("up") && input) //Selección left.
        {
            int aux = index;
            index--;
            ChangeSelect(aux,ref index);
        }
    }

    private void ChangeSelect(int aux,ref int index)
    {
        // Se vuelve a poner default al anterior.
        mirror[aux].changeSelection();
        // Se selecciona el índice de forma modular.
        if (index < 0) index = (index % mirror.Length) + mirror.Length;
        else index = index % mirror.Length;
        // Se cambia al actual.
        mirror[index].changeSelection();
    }
    // Método para detener el input cuando se termina el puzle.
    public void StopInput()
    {
        input = false;
    }
    // Método auxiliar para mostrar el valor del input a los hijos del objeto.
    public bool ReturnInputValue()
    {
        return input;
    }

}
