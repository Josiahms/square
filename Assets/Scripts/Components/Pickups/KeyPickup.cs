using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour {

   private void Start() {
      GetComponentInChildren<Animator>().speed = 0;
   }

   private void OnTriggerEnter2D(Collider2D collision) {
      if (collision.gameObject.name == "Player") {
         KeyChain.GetInstance().AddKey(GetComponentInChildren<SpriteRenderer>().color);
         GetComponentInChildren<Animator>().speed = 1;
         GetComponentInChildren<AudioSource>().PlayOneShot(GetComponentInChildren<AudioSource>().clip);
         GetComponent<Collider2D>().enabled = false;
         Destroy(transform.parent.gameObject, 0.47f);
      }
   }

}
