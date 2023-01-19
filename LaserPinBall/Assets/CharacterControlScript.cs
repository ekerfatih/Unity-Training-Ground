using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Vector3 = UnityEngine.Vector3;
public class CharacterControlScript : MonoBehaviour {

    [SerializeField] private float allowedRange = 2f;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 5;
    [SerializeField] private float jumpTime = 2f;
    [SerializeField] private Transform ball;
    public bool isGameEnded;
    public int score;
    public TMP_Text scoreText;
    private bool temp;
    

    private void Update() {
        if (isGameEnded) return;
        print(!CameraBasedPosition.Instance.isGameStarted);
        if (!CameraBasedPosition.Instance.isGameStarted) return;
        scoreText.text = score.ToString();
        Vector3 mousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
        Vector3 mousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
        Vector3 Near = Camera.main.ScreenToWorldPoint(mousePosNear);
        Vector3 Far = Camera.main.ScreenToWorldPoint(mousePosFar);

        Ray ray = new Ray(Near, Far);
        Physics.Raycast(ray, out RaycastHit hit, ground);
        

        var lookPosition = Quaternion.LookRotation(hit.point - transform.position);
        lookPosition.x = 0;
        lookPosition.z = 0;
        bool isInRange = (hit.point - transform.position).magnitude > allowedRange;
        bool isGrounded = transform.position.y == 0.5f;

        if (Input.GetMouseButtonDown(0) && !isInRange) {
            if (!isGrounded) return;
            transform.DOScale(Vector3.one * 1.5f, jumpTime / 2).SetEase(Ease.OutBack).OnComplete(() => transform.DOScale(Vector3.one, jumpTime / 2));
            Vector3 jumpPos = transform.position + transform.forward * 2;
            if (jumpPos.x - transform.localScale.x < CameraBasedPosition.Instance.bottomLeft.x) jumpPos = transform.position;
            if (jumpPos.x + transform.localScale.x > CameraBasedPosition.Instance.topRight.x) jumpPos = transform.position;
            if (jumpPos.z + transform.localScale.z > CameraBasedPosition.Instance.topRight.z) jumpPos = transform.position;
            if (jumpPos.z - transform.localScale.z < CameraBasedPosition.Instance.bottomLeft.z) jumpPos = transform.position;
            transform.DOJump(jumpPos, 3, 1, jumpTime);

        }

        if (Input.GetMouseButton(0)) {
            transform.rotation = Quaternion.Lerp(transform.rotation, lookPosition, Time.deltaTime * 10);

            if (isInRange) {
                Vector3 nextPos = transform.position + transform.forward  * Time.deltaTime * speed;
                if (nextPos.x - transform.localScale.x < CameraBasedPosition.Instance.bottomLeft.x) return;
                if (nextPos.x + transform.localScale.x > CameraBasedPosition.Instance.topRight.x) return;
                if (nextPos.z + transform.localScale.z > CameraBasedPosition.Instance.topRight.z) return;
                if (nextPos.z - transform.localScale.z < CameraBasedPosition.Instance.bottomLeft.z) return;

                transform.Translate(Vector3.forward * Time.deltaTime * speed);
            }
        }

        
    }
    private void FixedUpdate() {
        if (IsBallDodged() != temp) {
            score++;
            temp = IsBallDodged();
        }
    }
    public bool IsBallDodged() {
        if(transform.position.z > ball.position.z) {
            return true;
        }
        return false;
    }
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Laser")) {
            CameraBasedPosition.Instance.GameOver();
            transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        }
    }



}