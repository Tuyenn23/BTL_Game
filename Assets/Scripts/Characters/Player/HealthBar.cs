using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Text valueText;
    
    public void UpdateBar(int currentValue, int maxHealth)
    {
        valueText.text = currentValue.ToString();
    }
}
