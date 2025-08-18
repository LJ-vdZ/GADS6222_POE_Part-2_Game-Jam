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
<<<<<<< Updated upstream:GADS6222_GameJam/Assets/EnemyHealthBar.cs
        transform.rotation = camera.transform.rotation;
=======

        transform.LookAt(cam.transform);
        transform.Rotate(0f, 180f, 0f);
>>>>>>> Stashed changes:GADS6222_GameJam/Assets/Scripts/EnemyHealthBar.cs
    }
    public void FindCamera()
    {
        camera = Playermovement.instance.playerCameraForReference;

    }
}
