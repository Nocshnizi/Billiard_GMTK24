using System;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _instance;
    
    public static GameAssets Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            }

            return _instance;
        }
    }
    
    [SerializeField] private TextPopup scorePopupPrefab;
    [SerializeField] private AudioSource audioSource;
    
    public TextPopup ScorePopupPrefab => scorePopupPrefab;
    public AudioSource AudioSource => audioSource;

    private void Awake()
    {
        _instance = this;
    }
}