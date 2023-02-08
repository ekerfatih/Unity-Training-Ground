using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TrainPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        transform.DOMove(Vector3.zero, 2f).SetSpeedBased();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
