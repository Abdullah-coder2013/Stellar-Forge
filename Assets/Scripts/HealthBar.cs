using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxHealth(int health)
    {
        if (SaveSystem.LoadUpgrade("Health") != null) {
            slider.maxValue += health + int.Parse(SaveSystem.LoadUpgrade("Health").level);
        }
        else{
            slider.maxValue += health;
        }
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        slider.value = health;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
