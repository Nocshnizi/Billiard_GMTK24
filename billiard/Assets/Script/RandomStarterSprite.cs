using UnityEngine;

public class RandomStarterSprite : MonoBehaviour
{

    [SerializeField] private Sprite[] sprites;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer.sprite = sprites[UnityEngine.Random.Range(0, sprites.Length)];
    }
}