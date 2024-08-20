using DG.Tweening;
using UnityEngine;

public class RadioEffect : MonoBehaviour
{
    private void Start()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(0.64f, 0.4f));
        sequence.Join(transform.DOLocalRotate(new Vector3(0, 0, 1.3f), 0.4f));
        sequence.Append(transform.DOScale(0.6f, 0.6f));
        sequence.Join(transform.DOLocalRotate(new Vector3(0, 0, 0), 0.6f));
        sequence.AppendInterval(1f);
        sequence.SetLoops(-1);
    }
}
