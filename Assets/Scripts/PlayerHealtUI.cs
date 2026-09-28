using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField]
    private PlayerHealth playerHealth;

    [SerializeField]
    private Slider hpSlider;

    void Start()
    {
        hpSlider.maxValue = playerHealth.MaxHP;
        hpSlider.value = playerHealth.CurrentHP;
    }

    void Update()
    {
        hpSlider.maxValue = playerHealth.MaxHP;
        hpSlider.value = playerHealth.CurrentHP;
    }
}