using System;
using System.Collections.Generic;
using UnityEngine;

public class BilliardManager : MonoBehaviour
{
    
    [SerializeField] private List<BiliardBall> balls;
    [SerializeField] private BilliardHole[] holes;
    [SerializeField] private BilliardStick stick;
    [SerializeField] private GameObject winScreen;
    
    public event Action<int> OnScoreUpdated;
    
    private int _score;
    private bool _isRoundInProgress;
    private BiliardBall _targetBall;
    
    private void Start()
    {
        _targetBall = balls[0];
        stick.SetTargetBall(_targetBall.BallRigidbody);
        
        foreach (var hole in holes)
        {
            hole.OnScoreBall += (score, ball) =>
            {
                UpdateScore(score);
                balls.Remove(ball);

                if (_targetBall == ball )
                {
                    if (balls.Count > 0)
                    {
                        _targetBall = balls[0];
                        stick.SetTargetBall(_targetBall.BallRigidbody);
                        winScreen.SetActive(false);
                    }
                    else
                    {
                        stick.SetTargetBall(null);
                        winScreen.SetActive(true);
                        Debug.Log("Game Over!");
                    }
                }
                
                Destroy(ball.gameObject);
            };
        }
    }

    private void Update()
    {
        bool anyBallMoving = false;
        foreach (var ball in balls)
        {
            if(ball == null)
                continue;
            
            bool isMoving = IsBallMoving(ball);
            if (isMoving)
            {
                anyBallMoving = true;
                
                if (!_isRoundInProgress)
                {
                    _isRoundInProgress = true;
                    stick.gameObject.SetActive(false);
                }
            }
            else if (ball.BallRigidbody.linearVelocity.magnitude > 0)
            {
                ball.BallRigidbody.linearVelocity = Vector2.zero;
                ball.BallRigidbody.angularVelocity = 0;
            }
        }
        
        if (_isRoundInProgress && !anyBallMoving)
        {
            _isRoundInProgress = false;
            stick.gameObject.SetActive(true);
        }
    }

    private bool IsBallMoving(BiliardBall ball)
    {
        return ball.BallRigidbody.linearVelocity.magnitude > 0.09f;
    }
    
    private void UpdateScore(int score)
    {
        _score += score;
        Debug.Log($"Scored {score} points!");
        OnScoreUpdated?.Invoke(_score);
    }
}