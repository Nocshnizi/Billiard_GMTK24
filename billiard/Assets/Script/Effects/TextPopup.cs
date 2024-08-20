using DG.Tweening;
using TMPro;
using UnityEngine;

public class TextPopup : MonoBehaviour
{
    
    [SerializeField] private TMP_Text text;
    
    private void Setup(string text)
    {
        this.text.text = text;
    }
    
    public static TextPopup Create(Vector3 position, string text)
    {
        var textPopupGO = Instantiate(GameAssets.Instance.ScorePopupPrefab.gameObject, position, Quaternion.identity);
        TextPopup textPopup = textPopupGO.GetComponent<TextPopup>();
        textPopup.Setup(text);
        
        textPopup.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        
        var sequence = DOTween.Sequence();
        sequence.Append(textPopup.transform.DOScale(1f, 0.8f).SetEase(Ease.OutElastic));
        sequence.Join(textPopup.transform.DOLocalMoveY(position.y + 1.7f, 0.8f).SetEase(Ease.OutCirc));
        sequence.Join(textPopup.transform.DORotate(new Vector3(0, 0, Random.Range(-30f, 30f)), 0.8f).SetEase(Ease.OutCirc));
        sequence.AppendInterval(0.5f);
        sequence.Append(textPopup.transform.DOScale(0.3f, 0.8f).SetEase(Ease.InBack));
        sequence.Join(textPopup.text.DOFade(0, 0.8f).SetEase(Ease.InBack));
        sequence.OnComplete(() => Destroy(textPopup.gameObject));
        
        return textPopup;
    }
}
