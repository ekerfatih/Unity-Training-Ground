using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class Police : MonoBehaviour {

    [SerializeField] private Animator animator;
    [SerializeField] private PoliceAnimatorHelper policeAnimatorHelper;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform[] points;
    private TrainDoorOpeningAnimation _trainDoorOpeningAnimation;
    private GateOpening _gateOpening;
    private int order;
    private Vector3 startPos,startRot;
    private void Start() {
        _trainDoorOpeningAnimation = FindObjectOfType<TrainDoorOpeningAnimation>();
        _gateOpening = FindObjectOfType<GateOpening>();
        startPos = transform.position;
        startRot = transform.eulerAngles;
    }
    public IEnumerator Stand() {
        animator.SetBool("stand", true);
        animator.SetBool("walk", true);
        yield return new WaitForSeconds(2f);
        agent.destination = points[1].position;
        yield return new WaitForSeconds(2f);
        agent.destination = points[2].position;
        yield return new WaitForSeconds(12f);
        animator.SetBool("button", true);
        AudioClip clip = FindObjectOfType<TrainDoorOpeningAnimation>().doorOpeningSound;
        SoundManager.Instance.PlaySound(clip);
        _trainDoorOpeningAnimation.IsDoorOpen(true);
        yield return new WaitForSeconds(4f);
        agent.destination = points[0].position;
        yield return new WaitForSeconds(14f);
        animator.SetBool("button", true);
        _gateOpening.ActivateDoor(true);
        yield return new WaitForSeconds(2f);
        transform.DORotate(startRot, 1f);
        animator.SetBool("idle", true);
        

    }

    public void Started() {
        StartCoroutine(Stand());
    }

}
