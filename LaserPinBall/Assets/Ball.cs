using System;
using System.IO.Compression;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour {
    public float ballspeed;
    public int maxBounce;
    private Vector3 dir;
    private void Start() {
        transform.rotation = Quaternion.Euler(0, Random.Range(0,360f), 0);
        dir = transform.forward;
    }
    private void Update() {
        Debug.Log(dir);
        GetComponent<Rigidbody>().velocity = dir * ballspeed;
    }
    private void OnDrawGizmos() {
        Vector3 origin = transform.position;
        Ray ray = new Ray(origin,  dir);

        for (int i = 0; i < maxBounce; i++) {
            if (Physics.Raycast(ray,out RaycastHit hit)) {
                Gizmos.DrawLine(ray.origin, hit.point);
                dir.y = 0;
                Vector3 reflected = Vector3.Reflect(ray.direction,hit.normal);
                Gizmos.DrawLine(hit.point, hit.point + reflected);
                ray.direction = reflected;
                ray.origin = hit.point;
            }
            else {
                break;
            }
        }

        Vector3 Reflect(Vector3 inDir, Vector3 n) {
            float proj = Vector3.Dot(inDir, n);
            return inDir - 2 * proj * n;
        }

    }
   
    private void OnCollisionEnter(Collision other) {
        Debug.Log("sadsadsa");
        dir = Vector3.Reflect(dir, other.contacts[0].normal);
    }

}