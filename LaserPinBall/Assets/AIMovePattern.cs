using System;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class AIMovePattern : MonoBehaviour {

    public float playerSpeed ;
    public float returnOriginSpeed ;
    private Ball ball;
    private Rigidbody ball_rigidbody;
    public Transform leftPlayer, rightPlayer;
    private Vector3 startPosL, startPosR;
    public bool isLeftPlayerTurn;
    private void Awake() {
        ball = FindObjectOfType<Ball>();
        ball_rigidbody = ball.GetComponent<Rigidbody>();
        startPosL = leftPlayer.position;
        startPosR = rightPlayer.position;
    }
    private void Update() {
        RaycastHit hit = ball.hitInfo;
        isLeftPlayerTurn = ball_rigidbody.velocity.x > 0;
        if (isLeftPlayerTurn) {
            MoveObjectwAddforce(leftPlayer, hit.point, playerSpeed);
            MoveObjectwAddforce(rightPlayer, startPosR, returnOriginSpeed);
        }
        else {
            MoveObjectwAddforce(rightPlayer, hit.point, playerSpeed);
            MoveObjectwAddforce(leftPlayer, startPosL, returnOriginSpeed);
        }
    }


    void MoveObjectwAddforce(Transform player, Vector3 returnPoint, float speed) {
        if (player.transform.position.z > returnPoint.z) {
            if(Mathf.Abs(player.transform.position.z - returnPoint.z) < .6f) return;
            //player.transform.Translate(Vector3.back * Time.deltaTime *speed);
            player.GetComponent<Rigidbody>().velocity = Vector3.back * Time.deltaTime * speed;
            //player.GetComponent<Rigidbody>().AddForce(Vector3.back * speed);
        }
        else {
            if(Mathf.Abs(player.transform.position.z - returnPoint.z) < .6f) return;
            player.GetComponent<Rigidbody>().velocity = Vector3.forward * Time.deltaTime * speed;
            //player.GetComponent<Rigidbody>().AddForce(Vector3.forward * speed);
        }
    }

    void ReturnCenter(Transform player, Vector3 returnPoint, float speed) {
        if (player.transform.position.z > returnPoint.z) {
            //player.transform.Translate(Vector3.back * Time.deltaTime *speed);
            //player.GetComponent<Rigidbody>().velocity = Vector3.back * Time.deltaTime * speed;
            player.GetComponent<Rigidbody>().AddForce(Vector3.back * speed * Time.deltaTime);
        }
        else {
            //player.GetComponent<Rigidbody>().velocity = Vector3.forward * Time.deltaTime * speed;
            player.GetComponent<Rigidbody>().AddForce(Vector3.forward * speed * Time.deltaTime);
        }
    }

}