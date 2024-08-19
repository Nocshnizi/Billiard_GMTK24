using TMPro;
using UnityEngine;

public class ScoreTextUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    
    [SerializeField] private BilliardManager manager;
    
    private void Start()
    {
        manager.OnScoreUpdated += UpdateScore;
        UpdateScore(0);
    }

    private void UpdateScore(int obj)
    {
        scoreText.text = $"Score: {obj}";
    }
}