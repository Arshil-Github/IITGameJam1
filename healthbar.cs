using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthbar : MonoBehaviour
{
    public Slider slider;
    public Gradient colorgradient;
    public Image fill;
    public void SetHealth(float health)
    {
        slider.value = health;

        fill.color = colorgradient.Evaluate(slider.normalizedValue);
    }
    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        fill.color = colorgradient.Evaluate(1f);
    }
}
