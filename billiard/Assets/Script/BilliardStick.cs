using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BilliardStick : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float rotationSpeedBoost;
    
    [SerializeField] private Vector2 shootForceRange;
    [SerializeField] private float shootForceChargeingTime;
    
    [SerializeField] private Rigidbody2D targetBall;
    
    [SerializeField] private Transform stickSpriteTransform;
    [SerializeField] private float stickLenght;
    
    [SerializeField] private LineRenderer aimAssistLine;
    
    public event Action<bool> UpdateChargeStatus;
    public event Action<float> UpdateShootForceValue;
    
    private bool IsCharge
    {
        get => _isCharging;
        set
        {
            if (_isCharging == value)
                return;
            
            _isCharging = value;
            UpdateChargeStatus?.Invoke(_isCharging);
        }
    }
    
    private bool _isCharging;
    private float _chargingTimer;
    
    private readonly RaycastHit2D[] _hits = new RaycastHit2D[1];
    
    private void Update()
    {
        if(targetBall == null)
            return;
        
        transform.position = targetBall.transform.position;
        float ballSize = targetBall.transform.localScale.x;
        stickSpriteTransform.localPosition = new Vector3(0, -(stickLenght + ballSize + 0.2f), 0);
        
        RotateStick();
        UpdateShootForce();
        UpdateAimAssist();
    }
    
    public void SetTargetBall(Rigidbody2D ball)
    {
        targetBall = ball;
    }
    
    public void Shoot(float force)
    {
        targetBall.AddForce(transform.up * force, ForceMode2D.Impulse);
        targetBall.AddTorque((Random.Range(0, 2) - 1) * (force + Random.Range(60, 90)) * Mathf.Deg2Rad, ForceMode2D.Impulse);
    }
    
    private void RotateStick()
    {
        float horizontal = Input.GetAxis("Horizontal") * (Input.GetAxisRaw("Horizontal") == 0 ? 0 : 1);
        bool isRotating = Mathf.Abs(horizontal) > 0.1f;
        bool fastRotation = Input.GetKey(KeyCode.LeftShift);
        
        if (isRotating)
        {
            float speed = rotationSpeed * (fastRotation ? rotationSpeedBoost : 1);
            transform.Rotate(Vector3.forward, horizontal * speed * Time.deltaTime);
        }
    }
    
    private void UpdateShootForce()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            IsCharge = true;
        }
        
        if(!IsCharge)
            return;
        
        _chargingTimer += Time.deltaTime;
        _chargingTimer = Mathf.Clamp(_chargingTimer, 0, shootForceChargeingTime);
        float chargeProgress = _chargingTimer / shootForceChargeingTime;
        chargeProgress = chargeProgress * chargeProgress;  
        UpdateShootForceValue?.Invoke(chargeProgress);
        
        if(Input.GetKeyUp(KeyCode.Space))
        {
            float shootForce = Mathf.Lerp(shootForceRange.x, shootForceRange.y, chargeProgress);
            Shoot(shootForce);
            IsCharge = false;
            _chargingTimer = 0;
        }
    }

    private void UpdateAimAssist()
    {
        if(targetBall.Cast(transform.up, _hits) == 0)
            return;
        
        var hit = _hits[0];
        
        var collisionPoint = transform.up * hit.distance + transform.position;
        var collisionBounceDirection = Vector3.Reflect(transform.up, _hits[0].normal);
        
        aimAssistLine.SetPosition(0, transform.position);
        aimAssistLine.SetPosition(1, hit.point);
        aimAssistLine.SetPosition(2, collisionPoint + collisionBounceDirection * 2);
    }
}
