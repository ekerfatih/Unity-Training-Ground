using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;
    
    public int gameState;
    public Transform train;
    public WindowSmall windowSmall;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }
        else {
            Destroy(this.gameObject);
        }
    }

    private void Update() {
        if(gameState == 0) {
            if (train.transform.position == Vector3.zero) {
                gameState++;
            }
        }
        if (gameState == 1) {
            windowSmall.Interact();
            gameState++;
        }
    }

}