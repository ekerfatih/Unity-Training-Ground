using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat {
    public class Fighter : MonoBehaviour, IAction {

        [SerializeField] private float weaponRange;
        [SerializeField] private float timeBetweenAttacks;
        [SerializeField] private float weaponDamage;


        private Health _target;
        private Mover _mover;
        private Animator _animator;

        private float _timeSinceLastAttack = Mathf.Infinity;
        private static readonly int AttackAnimation = Animator.StringToHash("attack");
        private static readonly int StopAttackAnimation = Animator.StringToHash("stopAttack");
        private void Start() {
            _animator = GetComponent<Animator>();
            _mover = GetComponent<Mover>();
        }
        private void Update() {
            _timeSinceLastAttack += Time.deltaTime;
            if (_target == null) return;
            if (_target.IsDead()) return;

            if (_target != null && !IsInRange()) {
                _mover.MoveTo(_target.transform.position);
            }
            else {
                _mover.Cancel();
                AttackBehaviour();
            }
        }
        private void AttackBehaviour() {
            transform.LookAt(_target.transform, Vector3.up);
            if (_timeSinceLastAttack > timeBetweenAttacks) {
                TriggerAttack();
                _timeSinceLastAttack = 0;
            }
        }
        private void TriggerAttack() {
            _animator.ResetTrigger(StopAttackAnimation);
            _animator.SetTrigger(AttackAnimation);
        }
        private bool IsInRange() {
            return Vector3.Distance(transform.position, _target.transform.position) < weaponRange;
        }

        public bool CanAttack(GameObject combatTarget) {
            if (combatTarget == null) return false;
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Attack(GameObject target) {
            GetComponent<ActionScheduler>().StartAction(this);
            _target = target.GetComponent<Health>();
        }
        public void Cancel() {
            StopAttack();
            _target = null;
        }
        private void StopAttack() {

            _animator.ResetTrigger(AttackAnimation);
            _animator.SetTrigger(StopAttackAnimation);
        }

        //Animation Event
        void Hit() {
            if (_target == null) return;
            if (_target.TryGetComponent(out Health health)) {
                health.TakeDamage(weaponDamage);
            }
        }
    }
}