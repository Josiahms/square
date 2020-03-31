using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageCollider : MonoBehaviour {

   [SerializeField]
   // damage less than or equal to 0 insta kills the player
   private int damage;

   [SerializeField]
   private bool isFriendly;

   public void OnCollisionEnter2D(Collision2D collision) {
      if (isFriendly) {
         return;
      }
      Respawnable player = collision.collider.GetComponent<Respawnable>();
      if (player != null) {
         if (damage <= 0) {
            Health.GetInstance().SetHealth(0);
         } else {
            Health.GetInstance().OffsetHealth(-damage);
         }
      }
   }

   public void OnTriggerEnter2D(Collider2D other) {
      if (!isFriendly) {
         return;
      }

      var killable = other.GetComponent<Killable>();
      if (killable != null) {
         killable.Kill();
      }
   }

}
