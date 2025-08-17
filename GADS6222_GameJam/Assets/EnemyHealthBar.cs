using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider slider;

    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        slider.value = currentHealth/maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
