using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement {
    public class Mover : MonoBehaviour ,IAction{
        private static readonly int ForwardSpeed = Animator.StringToHash("forwardSpeed");
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private Health _health;

        private void Start() {
            _animator = GetComponent<Animator>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _health = GetComponent<Health>();
        }
        private void Update() {
            _navMeshAgent.enabled = !_health.IsDead();
            UpdateAnimator();
        }
        public void MoveTo(Vector3 destination) {
            _navMeshAgent.destination = destination;
            _navMeshAgent.isStopped = false;
        }
        private void UpdateAnimator() {
            Vector3 velocity = _navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            _animator.SetFloat(ForwardSpeed,speed);
        }

        public void StartMoveAction(Vector3 destination) {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }
        
        public void Cancel() {
            _navMeshAgent.isStopped = true;
        }
    }
}