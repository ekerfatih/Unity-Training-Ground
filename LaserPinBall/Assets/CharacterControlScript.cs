using System;
using DG.Tweening;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
public class CharacterControlScript : MonoBehaviour {

    [SerializeField] private float allowedRange = 2f;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 5;
    [SerializeField] private float jumpTime = 2f;

    private void Update() {
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
        bool isGrounded = transform.position.y == 0;

        if (Input.GetMouseButtonDown(0) && !isInRange) {
            if (!isGrounded) return;
            transform.DOScale(Vector3.one * 1.5f, jumpTime / 2).SetEase(Ease.OutBack).OnComplete(() => transform.DOScale(Vector3.one, jumpTime / 2));
            transform.DOJump(transform.position, 3, 1, jumpTime);

        }

        if (Input.GetMouseButton(0)) {
            transform.rotation = Quaternion.Lerp(transform.rotation, lookPosition, Time.deltaTime * 10);

            if (isInRange) {
                Vector3 nextPos = transform.position + transform.forward * Time.deltaTime * speed;
                if (nextPos.x - transform.localScale.x < CameraBasedPosition.Instance.bottomLeft.x) return;
                if (nextPos.x + transform.localScale.x > CameraBasedPosition.Instance.topRight.x) return;
                if (nextPos.z + transform.localScale.z > CameraBasedPosition.Instance.topRight.z) return;
                if (nextPos.z - transform.localScale.z < CameraBasedPosition.Instance.bottomLeft.z) return;

                transform.Translate(Vector3.forward * Time.deltaTime * speed);
            }
        }

    }




}