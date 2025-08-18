using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider slider;
    public Camera cam;
    public Canvas parent;

    private void Start()
    {
        FindCamera();
    }
    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        slider.value = currentHealth/maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (cam == null)
        {
            FindCamera();
            return;
        }
        transform.rotation = cam.transform.rotation;
    }
    public void FindCamera()
    {
        cam = Playermovement.instance.playerCameraForReference;

    }
}
