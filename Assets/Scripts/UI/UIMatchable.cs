using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIMatchable : MonoBehaviour
{
    public Matchable matchable;
    
    public UnityEvent<Matchable> clicked;
    
    public void SetMatchable(Matchable _matchable)
    {
        GetComponent<Image>().color = _matchable.type.color;
        matchable = _matchable;
    }

    public void OnClicked()
    {
        clicked.Invoke(matchable);
    }

    public void OnRemoved()
    {
        Destroy(gameObject);
    }
}
