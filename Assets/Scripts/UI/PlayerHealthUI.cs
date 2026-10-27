
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Slider healthSlider;

    private void Start()
    {
        healthSlider.value = playerHealth.GetHealthPercentage();
    }

    private void Update()
    {
        healthSlider.value = playerHealth.GetHealthPercentage();
    }
}
