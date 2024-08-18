using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BiliardBall : MonoBehaviour
{
    
    [SerializeField] private Vector2 xRange = new Vector2(-2.5f, 2.5f);
    [SerializeField] private Vector2 yRange = new Vector2(-4.5f, 4.5f);
    
    [SerializeField] private Vector2 xSizeRange = new Vector2(.5f, 1.5f);
    [SerializeField] private Vector2 ySizeRange = new Vector2(.5f, 1.5f);
    
    [SerializeField] private GameObject ballScale;
    
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private bool YEEtOnStart = false;
    
    private bool _shouldAdjustScale = false;

    
    private void Start()
    {
        if(YEEtOnStart)
            YEEt();
    }

    private void Update()
    {

        Moving();
    }
    
    public void YEEt()
    {
        rb.AddForce(Random.insideUnitCircle * 25, ForceMode2D.Impulse);
    }

    public void Moving() 
    {
        float xRate = (transform.position.x - xRange.x) / (xRange.y - xRange.x);
        float yRate = (transform.position.y - yRange.x) / (yRange.y - yRange.x);

        float xSize = Mathf.Lerp(xSizeRange.x, xSizeRange.y, xRate);
        float ySize = Mathf.Lerp(ySizeRange.x, ySizeRange.y, yRate);

        float scale = xSize * ySize;
        ballScale.transform.localScale = new Vector3(scale, scale, 1);
        rb.mass = scale + 1;
    }

    public void OnCollisionEnter2D(Collision2D collision) 
    {
        if (!collision.gameObject.TryGetComponent<BiliardBall>(out BiliardBall ball))
            return;

        if (_shouldAdjustScale) 
        {
            if (ball.ballScale.transform.localScale.magnitude > ballScale.transform.localScale.magnitude) {

                Debug.Log("Ball is bigger");
            }
            else if (ball.ballScale.transform.localScale.magnitude < ballScale.transform.localScale.magnitude) {
                Debug.Log("This is bigger");
            }
            _shouldAdjustScale = false;
        }
        else 
        {
            Debug.Assert(!ball._shouldAdjustScale, "Ball already adjusting scale!?");
            ball._shouldAdjustScale = true;
        }
    }
}
