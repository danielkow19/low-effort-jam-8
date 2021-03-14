using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thermometer : MonoBehaviour
{

    public Slider slider;

    public void SetTemperature(float temperature)
    {
        slider.value = temperature;
    }
}
