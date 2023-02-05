using UnityEngine;
using DG.Tweening;
public class WindowSmall : MonoBehaviour {

    [SerializeField] private Police _police;
    public void Interact() {
        transform.DOScaleZ(0.7f, 5).OnComplete(_police.Started);
    }
}
