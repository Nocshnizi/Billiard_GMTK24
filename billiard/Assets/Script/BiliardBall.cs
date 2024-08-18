using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

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

    [SerializeField] private bool YEEtOnStart = false;
    
    private bool _shouldAdjustScale = false;
    private float _givenScale = 0;
    
    private void Start()
    {
        if(YEEtOnStart)
            YEEt();
    }

    private void Update()
    {
        float scale = CalculateTotalScale();
        ballScale.transform.localScale = new Vector3(scale, scale, 1);
        rb.mass = scale + 1;
    }

    public void OnCollisionEnter2D(Collision2D collision) 
    {
        if (!collision.gameObject.TryGetComponent<BiliardBall>(out BiliardBall ball))
            return;

        if (_shouldAdjustScale) 
        {
            ball._givenScale = 0;
            _givenScale = 0;



            if (ball.ballScale.transform.localScale.x> ballScale.transform.localScale.x)
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

    public void YEEt() {
        rb.AddForce(Random.insideUnitCircle * 25, ForceMode2D.Impulse);
        rb.AddTorque((Random.Range(0, 2) - 1) * Random.Range(60, 250) * Mathf.Deg2Rad, ForceMode2D.Impulse);
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
}
