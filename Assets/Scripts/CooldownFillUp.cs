using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownFillUp : MonoBehaviour {

    public UIManager uIManager;
    float cooldown, sliderValue;
    string name;
    float timePassed = 0;

    void Start()
    {
        ReturnName();
        //Llama al metodo ReturnCooldowns del AbilityManager, el cual devuelve el cooldown especifico de esta habilidad
        cooldown = GameManager.instance.ReturnCooldown(name);
        sliderValue = 1;
        uIManager.SetSliderValue(sliderValue, name);
    }

    void Update()
    {
        //Si el valor del slider de esta habilidad es 0 quiere decir que esta habilidad acaba de usarse, por lo que el icono que muestra el cooldown ha de empezar a llenarse
        if (uIManager.ReturnSliderValue(name) == 0)
        {
            //Paramos la corrutina que hace que se llene el icono para que no se ejecuten varias al mismo tiempo
            StopCoroutine("IncrementSliderValue");
            //Hacemos que comience la corrutina que hace que se llene el icono aumentando el valor del slider correspondiente
            StartCoroutine("IncrementSliderValue");
        }
    }

    IEnumerator IncrementSliderValue()
    {
        //Inicializamos los valores a cero para resetear el estado del slider
        sliderValue = 0;
        timePassed = 0;

        //Esta corrutina se ejecutará hasta que haya pasado un 100% del tiempo de cooldown, lo que significa que el valor del slider sera 1, momento en el que esta parará
        while (sliderValue < 1)
        {
            timePassed += 8; //Aumentamos el valor del tiempo que ha pasado desde el comienzo del bucle
            sliderValue = timePassed / cooldown / 100; //Para que el valor del slider aumente al ritmo que marca el cooldown debemos dividir el tiempo que ha pasado entre el cooldown, obteniendo así el porcentaje de tiempo transcurrido
            uIManager.SetSliderValue(sliderValue, name); //Actualizamos el valor del slider correspondiente en el UIManager y hacemos que se muestre en pantalla
            yield return new WaitForSeconds(8f / 100); //Este bucle se repetira al ritmo que marquemos aqui
        }
    }

    //El metodo ReturnName devuelve el nombre de una de las tres habilidades de combate dependiendo del nombre de este objeto
    void ReturnName()
    {
        if (gameObject.name.Contains("Fireball")) name = "Fireball";

        else if (gameObject.name.Contains("Shield")) name = "Shield";

        else if (gameObject.name.Contains("Lightning")) name = "Lightning";
    }
}
