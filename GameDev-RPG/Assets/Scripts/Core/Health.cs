using UnityEngine;

namespace RPG.Core {
    public class Health : MonoBehaviour {
        [SerializeField] private float healthPoints = 100f;
        private static readonly int Die = Animator.StringToHash("die");
        private bool _isDead;

        public bool IsDead() {
            return _isDead;
        }


        public void TakeDamage(float damageAmount) {
            healthPoints = Mathf.Max(healthPoints - damageAmount, 0);
            if(healthPoints == 0) {
                DieDieDie();
            }
            print(healthPoints);
        }
        private void DieDieDie() {
            if (_isDead) return;
            _isDead = true;
            GetComponent<Animator>().SetTrigger(Die);
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}