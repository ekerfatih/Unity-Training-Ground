using RPG.Combat;
using RPG.Core;
using UnityEngine;
using RPG.Movement;

namespace RPG.Control {
    public class PlayerController : MonoBehaviour {
        private Health _health;

        private void Start() {
            _health = GetComponent<Health>();
        }
        private void Update() {
            if(_health.IsDead()) return;
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
        }

        private bool InteractWithCombat() {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits) {
                if (hit.transform.TryGetComponent(out CombatTarget combatTarget)) {
                    if (!GetComponent<Fighter>().CanAttack(combatTarget.gameObject)) continue;
                    if (Input.GetMouseButtonDown(0)) {
                        GetComponent<Fighter>().Attack(combatTarget.gameObject);
                    }
                    return true;
                }
            }
            return false;
        }

        private bool InteractWithMovement() {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit) {
                if (Input.GetMouseButton(0)) {
                    GetComponent<Mover>().StartMoveAction(hit.point);
                }
                return true;
            }
            return false;
        }
        private static Ray GetMouseRay() {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return ray;
        }

    }
}