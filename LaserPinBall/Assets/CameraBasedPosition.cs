using System;
using UnityEngine;

public class CameraBasedPosition : MonoBehaviour {

    public static CameraBasedPosition Instance;
    public float width, height;
    public Transform leftPlayer, rightPlayer;
    public bool showBorder;
    private Camera Cam;
    private Vector3 topRightPoint;
    private Vector3 bottomLeftPoint;
    [SerializeField] private Vector2 borderMinMaxPercent;

    public Vector3 topRight, topLeft, bottomRight, bottomLeft;
    private void Awake() {
        if(Instance==null) {
            Instance = this;
        }
        else {
            Destroy(this);
        }
        
        Cam = Camera.main;
        width = Cam.pixelWidth;
        height = Cam.pixelHeight;
        
        Vector3 leftPosition = Cam.ScreenToWorldPoint(new Vector3(width * 0.9f, height/2, Cam.transform.position.y));
        Vector3 rightPosition = Cam.ScreenToWorldPoint(new Vector3(width * 0.1f, height/2, Cam.transform.position.y));

        leftPlayer.transform.position = leftPosition;
        rightPlayer.transform.position = rightPosition;
        
        topRightPoint = Cam.ScreenToWorldPoint(new Vector3(width * borderMinMaxPercent.y, height, Cam.transform.position.y));
        bottomLeftPoint = Cam.ScreenToWorldPoint(new Vector3(width * borderMinMaxPercent.x, 0, Cam.transform.position.y));
        
        topRight = new Vector3(topRightPoint.x, 0, topRightPoint.z);
        topLeft = new Vector3(bottomLeftPoint.x, 0, topRightPoint.z);
        bottomRight = new Vector3(topRightPoint.x,0, bottomLeftPoint.z);
        bottomLeft = new Vector3(bottomLeftPoint.x,0, bottomLeftPoint.z);
        
    }
    private void OnDrawGizmos() {
        if(!showBorder) return;

        Gizmos.DrawLine(topRight,topLeft);
        Gizmos.DrawLine(topLeft,bottomLeft);
        Gizmos.DrawLine(bottomLeft,bottomRight);
        Gizmos.DrawLine(bottomRight,topRight);
        
    }
}