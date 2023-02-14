using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control {

    public class AIController : MonoBehaviour {

        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float suspicionTime = 3f;
        [SerializeField] private PatrolPath patrolPath;
        [SerializeField] private float waypointTolerance = 1f;
        private GameObject _player;
        private Fighter _fighter;
        private Health _health;
        private Mover _mover;
        private Vector3 _guardLocation;

        private float _timeSinceLastSawPlayer = Mathf.Infinity;
        private int _currentWaypointIndex = 0;
        
        
        private void Start() {
            _mover = GetComponent<Mover>();
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            _player = GameObject.FindWithTag("Player");
            _guardLocation = transform.position;
        }
        private void Update() {
            if(_health.IsDead()) return;
            if (InAttackRange() && _fighter.CanAttack(_player)) {
                _timeSinceLastSawPlayer = 0;
                AttackBehaviour();
            }
            else if(_timeSinceLastSawPlayer < suspicionTime) {
                SuspicionBehaviour();
            }
            else {
                PatrolBehaviour();
            }
            _timeSinceLastSawPlayer += Time.deltaTime;
        }
        private void PatrolBehaviour() {
            Vector3 nextPosition = _guardLocation;
            if(patrolPath != null) {
                if (AtWaypoint()) {
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            
            _mover.StartMoveAction(nextPosition);
        }

        private Vector3 GetCurrentWaypoint() {
            return patrolPath.GetWaypoint(_currentWaypointIndex);
        }

        private void CycleWaypoint() {
            _currentWaypointIndex = patrolPath.GetNextIndex(_currentWaypointIndex);
        }

        private bool AtWaypoint() {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private void SuspicionBehaviour() {

            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
        private void AttackBehaviour() {

            _fighter.Attack(_player);
        }

        private bool InAttackRange() {
            float distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);
            return distanceToPlayer < chaseDistance;
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position,chaseDistance);
        }
    }
}