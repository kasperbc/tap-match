using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIMatchable : MonoBehaviour
{
    private Matchable matchable;
    
    public UnityEvent<Matchable> clicked;
    
    public void SetMatchable(Matchable _matchable)
    {
        GetComponent<Image>().color = _matchable.type.color;
        matchable = _matchable;

        matchable.removed += OnRemoved;
    }

    public void OnClicked()
    {
        clicked.Invoke(matchable);
    }

    private void OnRemoved()
    {
        Destroy(gameObject);
    }
}
