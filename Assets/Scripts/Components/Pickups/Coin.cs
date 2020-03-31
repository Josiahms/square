using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider2D))]
public class Coin : MonoBehaviour {

   private void Start() {
      GetComponent<Animator>().speed = 0;
   }

   private void OnTriggerEnter2D(Collider2D collision) {
      if (collision.gameObject.name == "Player") {
         Purse.GetInstance().AddMoney(1);
         GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);
         GetComponent<Animator>().speed = 1;
         GetComponent<Collider2D>().enabled = false;
         Destroy(transform.parent.gameObject, 0.65f);
      }
   }
}
