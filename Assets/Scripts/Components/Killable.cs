using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killable : MonoBehaviour {

   [SerializeField]
   private List<PlayerState> vulnerabilities;
   [SerializeField]
   private List<MonoBehaviour> componentsToKill;

   private void OnCollisionEnter2D(Collision2D collision) {
      if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) {
         if (vulnerabilities.Contains(PlayerStateManager.GetInstance().CurrentState) || vulnerabilities.Contains(PlayerState.Any)) {
            Kill();
         } else {
            Health.GetInstance().OffsetHealth(-1);
            //GetComponentInChildren<Animator>().SetTrigger("Attack");
         }
      }
   }

   private void OnTriggerEnter2D(Collider2D collider) {
      if (collider.gameObject.layer == LayerMask.NameToLayer("Player")) {
         if (vulnerabilities.Contains(PlayerStateManager.GetInstance().CurrentState) || vulnerabilities.Contains(PlayerState.Any)) {
            Kill();
         } else {
            Health.GetInstance().OffsetHealth(-1);
            //GetComponentInChildren<Animator>().SetTrigger("Attack");
         }
      }
   }

   public void Kill() {
      componentsToKill.ForEach(x => x.enabled = false);
      GetComponentInChildren<Collider2D>().enabled = false;
      GetComponent<Rigidbody2D>().velocity = Vector2.up * 10;
      GetComponent<Rigidbody2D>().gravityScale = 2.6f;
      Destroy(gameObject, 5);
   }

}
