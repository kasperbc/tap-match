using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// The visual representation of a Matchable
/// </summary>
[RequireComponent(typeof(Image))]
public class UIMatchable : MonoBehaviour
{
    public Matchable Matchable;
    
    public UnityEvent<Matchable> clicked;

    public RectTransform RectTransform
    {
        get
        {
            if (_rectTransform == null)
                _rectTransform = GetComponent<RectTransform>();
            return _rectTransform;
        }
    }
    private RectTransform _rectTransform;

    public void SetMatchable(Matchable _matchable)
    {
        GetComponent<Image>().color = _matchable.type.color;
        Matchable = _matchable;
    }
    
    // Used by Event Trigger
    public void OnClicked()
    {
        clicked.Invoke(Matchable);
    }

    #region Animation

    public void OnRemoved()
    {
        DOTween.Sequence().
            Append(RectTransform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.OutBack)).
            AppendInterval(0.25f).
            AppendCallback(() => { Destroy(gameObject); });
    }

    public void OnMoved(RectTransform newTile)
    {
        transform.SetParent(newTile, true);
        BounceToParentTile();
    }
    
    public void BounceToParentTile() => RectTransform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.OutBounce);
    
    #endregion
}
