using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider slider;
    public Camera camera;
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
        if (camera == null)
        {
            FindCamera();
            return;
        }
        transform.rotation = camera.transform.rotation;
    }
    public void FindCamera()
    {
        camera = Playermovement.instance.playerCameraForReference;

    }
}
