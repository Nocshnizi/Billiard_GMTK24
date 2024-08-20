using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ForceSliderUI : MonoBehaviour
{
    [SerializeField] private GameObject fillBar;
    [SerializeField] private Image fillImage;
    
    [SerializeField] private BilliardStick stick;
    
    [SerializeField] private Vector2 shakeAmount;
    
    private Vector3 _startPosition;
    private Quaternion _startRotation;
    
    private float _shakeThreshold = 0f;
    
    private void Start()
    {
        _startPosition = transform.position;
        //_startRotation = transform.rotation;
        
        stick.UpdateChargeStatus += (isCharging) =>
        {
            fillBar.SetActive(isCharging);
            if (isCharging)
            {
                fillImage.fillAmount = 0;
                _shakeThreshold = 0f;
                
                //transform.DOShakeRotation(1f, 1, 10, 90, false).SetLoops(-1);
            }
            else
            {
                transform.DOKill(true);
                transform.position = _startPosition;
                //transform.rotation = _startRotation;
            }
        };
        stick.UpdateShootForceValue += (value) =>
        {
            fillImage.fillAmount = value;
            if ((value - _shakeThreshold) > .1f)
            {
                _shakeThreshold = value;
                transform.DOKill(true);
                //transform.position = _startPosition;
                transform.DOShakePosition(1, Mathf.Lerp(value, shakeAmount.x, shakeAmount.y), 10, 90, false, false).SetLoops(-1);
            }
            
            
        };
        fillBar.SetActive(false);
    }
}
