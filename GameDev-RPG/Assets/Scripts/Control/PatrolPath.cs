using System;
using UnityEngine;

namespace RPG.Control {
    public class PatrolPath : MonoBehaviour {

        private void OnDrawGizmos() {
            for (int i = 0; i < transform.childCount; i++) {
                Gizmos.DrawSphere(transform.GetChild(i).position,.2f);
                Gizmos.DrawLine(transform.GetChild(i).position,GetWaypoint(i));
            }
        }

        public int GetNextIndex(int i) {
            return (i + 1) % transform.childCount;
        }

        public Vector3 GetWaypoint(int i) {
            return transform.GetChild(GetNextIndex(i)).position;
        }
        

    }
}