using System;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraBasedPosition : MonoBehaviour {

    public static CameraBasedPosition Instance;
    public float width, height;
    public Transform leftPlayer, rightPlayer;
    public bool showBorder;
    private Camera Cam;
    private Vector3 topRightPoint;
    private Vector3 bottomLeftPoint;
    [SerializeField] private Vector2 borderMinMaxPercent;
    [SerializeField] private GameObject light;
    [SerializeField] private Transform[] borders;
    [SerializeField] private Collider player;
    [SerializeField] private UnityEngine.UI.Image panel,startPanel;
    [SerializeField]private int lightCount;
    [SerializeField] private TMP_Text currentScoreTxt, MaxScoreTxt,newHigh;
    public bool isGameStarted;

    public Vector3 topRight, topLeft, bottomRight, bottomLeft;
    private void Awake() {
        panel.transform.position = new Vector3(Camera.main.pixelWidth/2, Camera.main.pixelHeight*2, 0);
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
        leftPosition.y = 0.5f;
        rightPosition.y = 0.5f;
        leftPlayer.transform.position = leftPosition;
        rightPlayer.transform.position = rightPosition;
        
        topRightPoint = Cam.ScreenToWorldPoint(new Vector3(width * borderMinMaxPercent.y, height, Cam.transform.position.y));
        bottomLeftPoint = Cam.ScreenToWorldPoint(new Vector3(width * borderMinMaxPercent.x, 0, Cam.transform.position.y));
        
        topRight = new Vector3(topRightPoint.x, 0, topRightPoint.z);
        topLeft = new Vector3(bottomLeftPoint.x, 0, topRightPoint.z);
        bottomRight = new Vector3(topRightPoint.x,0, bottomLeftPoint.z);
        bottomLeft = new Vector3(bottomLeftPoint.x,0, bottomLeftPoint.z);
        LetThereBeLight(topRightPoint,bottomLeft);
        SetBorder();
        startPanel.transform.GetChild(1).DOScale(Vector3.one * 1.2f, 1).SetLoops(-1,LoopType.Yoyo);
    }


    private void Update() {
        if (!isGameStarted && Input.GetMouseButtonDown(0)) {
            isGameStarted = true;
            Ball ball = FindObjectOfType<Ball>();
            ball.StartCoroutine(ball.StartRoutine());
            startPanel.gameObject.SetActive(false);
        }
    }
    private void LetThereBeLight(Vector3 topRight,Vector3 bottomLeft) {
        for (int i = 0; i < lightCount ; i++) {
            Vector3 pos = topRightPoint;
            Instantiate(light, topRightPoint - (i * Vector3.forward) + Vector3.up * .1f, Quaternion.identity,transform);
        }
        for (int i = 0; i < lightCount; i++) {
            Vector3 pos = bottomLeft;
            Instantiate(light, bottomLeft - (i * Vector3.back) + Vector3.up * .1f, Quaternion.identity,transform);
        }
    }
    public void GameOver() {
        CharacterControlScript _characterController = FindObjectOfType<CharacterControlScript>();
        _characterController.isGameEnded = true;
        player.enabled = false;
        if (_characterController.score > PlayerPrefs.GetInt("MaxScore", 0)) {
            
            PlayerPrefs.SetInt("MaxScore", _characterController.score);
            newHigh.gameObject.SetActive(true);
            newHigh.DOColor(new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1), 1f).SetLoops(-1,LoopType.Yoyo);
            
        }
        currentScoreTxt.text = _characterController.score.ToString();
        MaxScoreTxt.text = PlayerPrefs.GetInt("MaxScore", 0).ToString();
        player.transform.DOScale(0, 1.5f).SetEase(Ease.InBack);
        panel.transform.DOMove(new Vector3(Camera.main.pixelWidth/2,Camera.main.pixelHeight/2,0), 2f).SetEase(Ease.OutBounce);
        
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SetBorder() {
        borders[0].transform.position = Cam.ScreenToWorldPoint(new Vector3(-100, height, Cam.transform.position.y));
        borders[1].transform.position = Cam.ScreenToWorldPoint(new Vector3(width , height + 30, Cam.transform.position.y));
        borders[2].transform.position = Cam.ScreenToWorldPoint(new Vector3(width +100 , 0, Cam.transform.position.y));
        borders[3].transform.position = Cam.ScreenToWorldPoint(new Vector3(width , -30, Cam.transform.position.y));
    }
    
    private void OnDrawGizmos() {
        if(!showBorder) return;

        Gizmos.DrawLine(topRight,topLeft);
        Gizmos.DrawLine(topLeft,bottomLeft);
        Gizmos.DrawLine(bottomLeft,bottomRight);
        Gizmos.DrawLine(bottomRight,topRight);
        
    }

    public void PlayAgain() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        startPanel.gameObject.SetActive(true);
    }
}