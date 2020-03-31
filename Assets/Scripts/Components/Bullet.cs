using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour {

   [SerializeField]
   private float speed = 10f;

   private void Start() {
      GetComponent<Rigidbody2D>().velocity = transform.forward * speed;
   }

   private void OnCollisionEnter2D(Collision2D collision) {
      if (collision.collider.gameObject.name == "Player") {
         Health.GetInstance().OffsetHealth(-1);
      }
      Destroy(gameObject);
   }

}
