using System;
using JetBrains.Annotations;
using UnityEngine;

public class BiliardBall : MonoBehaviour
{
    [SerializeField] private float maxTotalScale;
    [SerializeField] private float minTotalScale;

    //Position scalling
    [SerializeField] private Vector2 xRange;
    [SerializeField] private Vector2 yRange;
    [SerializeField] private Vector2 positionScaleRange;

    [SerializeField] private GameObject ballScale;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] [CanBeNull] private MeshRenderer meshRenderer;
    [SerializeField] private float animationSpeed = 1;
    
    private bool _shouldAdjustScale = false;
    private float _givenScale = 0;
    private Material _material;

    public Rigidbody2D BallRigidbody => rb;
    public float BallScale => ballScale.transform.localScale.x;

    private void Start()
    {
        if(meshRenderer != null)
            _material = meshRenderer.material;
    }

    private void Update()
    {
        float scale = CalculateTotalScale();
        ballScale.transform.localScale = new Vector3(scale, scale, 1);
        rb.mass = scale + 1;
        
        _material?.SetVector("_Speed", new Vector2(rb.linearVelocity.x, rb.linearVelocity.y) * animationSpeed);
    }

    public void OnCollisionEnter2D(Collision2D collision) 
    {
        if (!collision.gameObject.TryGetComponent(out BiliardBall ball))
            return;

        if (_shouldAdjustScale) 
        {
            ball._givenScale = 0;
            _givenScale = 0;
            
            if (ball.BallScale > BallScale)
            {
                ball._givenScale = minTotalScale - ball.CalculateTotalScale();
                _givenScale = ball.CalculateTotalScale() - minTotalScale;
            }
            else
            {
                ball._givenScale = CalculateTotalScale() - minTotalScale;
                _givenScale = minTotalScale - CalculateTotalScale();
            }

            _shouldAdjustScale = false;
        }
        else 
        {
            Debug.Assert(!ball._shouldAdjustScale, "Ball already adjusting scale!?");
            ball._shouldAdjustScale = true;
        }
    }

    private float CalculatePositionalScale() {
        // 0 .. 1
        float xRate = (transform.position.x - xRange.x) / (xRange.y - xRange.x);
        float yRate = (transform.position.y - yRange.x) / (yRange.y - yRange.x);

        float positionScaleRate = xRate * yRate;
        return Mathf.Lerp(positionScaleRange.x, positionScaleRange.y, positionScaleRate);
    }

    private float CalculateTotalScale() 
    {
        float positionScale = CalculatePositionalScale();
        float scale = positionScale + _givenScale;
        return Mathf.Clamp(scale, minTotalScale, maxTotalScale);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, CalculatePositionalScale());
    }
}
