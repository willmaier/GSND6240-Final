using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelBar : MonoBehaviour
{

    public Slider slider;

    public void SetMaxFuel(float maxFuel)
    {
        slider.maxValue = maxFuel;
        slider.value = maxFuel;

    }

    public void SetFuel (float fuel)
    {
        slider.value = fuel;

    }
}
