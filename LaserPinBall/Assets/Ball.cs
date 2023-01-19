using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour {
    [SerializeField] private LayerMask playerLayer;
    public float ballspeed;
    public int maxBounce;
    private RaycastHit hit;
    public RaycastHit hitInfo => hit;
    private Vector3 dir;
    public AIMovePattern _ai;
    public IEnumerator StartRoutine() {
        _ai = FindObjectOfType<AIMovePattern>();

        bool coinFlip = Random.value >= 0.5;
        
        var rot = Quaternion.Euler(0, coinFlip ? Random.Range(60,140) : Random.Range(240,300) , 0);
        transform.rotation = rot;
        
        transform.GetChild(0).transform.rotation = Quaternion.identity ;
        dir = transform.forward;
        yield return new WaitForSeconds(1f);
        transform.GetChild(0).gameObject.SetActive(true);
    }
    private void Update() {
        GetComponent<Rigidbody>().velocity = dir * ballspeed;
        RayCheck();
        ballspeed = math.remap(0, 1, 3, 15, Difficulty.GetDifficultyPercent());
    }
    private void RayCheck() {
        Vector3 origin = transform.position;
        Ray ray = new Ray(origin,  dir);

        string checkPlayer = _ai.isLeftPlayerTurn ? "leftPlayer" : "rightPlayer";
        for (int i = 0; i < 100; i++) {
            if (Physics.Raycast(ray,out hit,playerLayer) && !hit.collider.CompareTag(checkPlayer)) {
                //Gizmos.DrawLine(ray.origin, hit.point);
                dir.y = 0;
                Vector3 reflected = Vector3.Reflect(ray.direction,hit.normal);
                //Gizmos.DrawLine(hit.point, hit.point + reflected);
                ray.direction = reflected;
                ray.origin = hit.point;
            }
            else {
                break;
            }
        }
        
    }
   
    private void OnCollisionEnter(Collision other) {
        dir = Vector3.Reflect(dir, other.contacts[0].normal);
    }

}