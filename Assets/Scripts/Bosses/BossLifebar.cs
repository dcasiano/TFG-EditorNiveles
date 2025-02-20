using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLifebar : MonoBehaviour {

    float sliderValue = 1;

    public void UpdateLifebar(int lifePoints, int currentLife)
    {
        sliderValue =  (float)currentLife / lifePoints;
        GameManager.instance.ReturnUIManager().SetSliderValue(sliderValue, "Boss");
    }
}
