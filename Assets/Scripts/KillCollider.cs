using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class KillCollider : MonoBehaviour {

   public void OnCollisionEnter2D(Collision2D collision) {
      Respawnable player = collision.collider.GetComponent<Respawnable>();
      if (player != null) {
         player.Respawn();
      }
   }

}
