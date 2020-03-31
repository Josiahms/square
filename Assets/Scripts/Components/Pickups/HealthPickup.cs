using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour {

   private void Start() {
      GetComponentInChildren<Animator>().speed = 0;
   }

   private void OnTriggerEnter2D(Collider2D collision) {
      if (collision.gameObject.name == "Player" && !Health.GetInstance().IsAtAMax()) {
         Health.GetInstance().OffsetHealth(2);
         GetComponentInChildren<Animator>().speed = 1;
         GetComponentInChildren<AudioSource>().PlayOneShot(GetComponentInChildren<AudioSource>().clip);
         GetComponent<Collider2D>().enabled = false;
         Destroy(gameObject, 0.47f);
      }
   }
}
