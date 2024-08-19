using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BilliardStick : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float rotationSpeedBoost;
    
    [SerializeField] private Vector2 shootForceRange;
    [SerializeField] private float shootForceChargeSpeed;
    
    [SerializeField] private Rigidbody2D targetBall;

    public Vector2 ShootForceRange => shootForceRange;

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
    private float _shootForce;
    
    private void Update()
    {
        if(targetBall == null)
            return;
        
        transform.position = targetBall.transform.position;
        
        RotateStick();
        UpdateShootForce();
    }
    
    public void SetTargetBall(Rigidbody2D ball)
    {
        targetBall = ball;
    }
    
    public void Shoot()
    {
        targetBall.AddForce(transform.up * _shootForce, ForceMode2D.Impulse);
        targetBall.AddTorque((Random.Range(0, 2) - 1) * (_shootForce + Random.Range(40, 80)) * Mathf.Deg2Rad, ForceMode2D.Impulse);
    }
    
    private void RotateStick()
    {
        float horizontal = Input.GetAxis("Horizontal");
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
        
        if(Input.GetKeyUp(KeyCode.Space))
        {
            Shoot();
            IsCharge = false;
            _shootForce = shootForceRange.x;
            return;
        }
        
        _shootForce += shootForceChargeSpeed * Time.deltaTime;
        _shootForce = Mathf.Clamp(_shootForce, shootForceRange.x, shootForceRange.y);
        UpdateShootForceValue?.Invoke(_shootForce);
    }
}
