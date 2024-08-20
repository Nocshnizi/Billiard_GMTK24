using UnityEngine;
using UnityEngine.UI;

public class ForceSliderUI : MonoBehaviour
{
    [SerializeField] private GameObject fillBar;
    [SerializeField] private Image fillImage;
    
    [SerializeField] private BilliardStick stick;
    
    private void Start()
    {
        stick.UpdateChargeStatus += (isCharging) => fillBar.SetActive(isCharging);
        stick.UpdateShootForceValue += (value) => fillImage.fillAmount = value;
        fillBar.SetActive(false);
    }
}
