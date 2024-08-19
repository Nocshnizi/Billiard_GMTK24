using System;
using UnityEngine;

public class BilliardHole : MonoBehaviour
{
    [SerializeField] private int score;
    [SerializeField] private float maxScale;
    
    public event Action<int, BiliardBall> OnScoreBall;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out BiliardBall ball))
            return;

        if(ball.BallScale > maxScale)
            return;
        
        OnScoreBall?.Invoke(score, ball);
    }
}
