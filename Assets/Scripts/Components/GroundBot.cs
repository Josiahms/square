using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GroundBot : MonoBehaviour {

   [SerializeField]
   private float speed = 25;

   [SerializeField]
   private Transform leftSensor;
   [SerializeField]
   private Transform rightSensor;
   [SerializeField]
   private Transform leftLimit;
   [SerializeField]
   private Transform rightLimit;

   private Rigidbody2D rb;

   private void Start() {
      rb = GetComponent<Rigidbody2D>();
   }

   private void Update() {
      var leftCollider = Physics2D.Raycast(leftSensor.position, -leftSensor.right, transform.position.x - leftLimit.position.x).collider;
      var rightCollider = Physics2D.Raycast(rightSensor.position, rightSensor.right, rightLimit.position.x - transform.position.x).collider;
      if (leftCollider != null && leftCollider.gameObject.layer == LayerMask.NameToLayer("Player")) {
         rb.velocity = -transform.right * speed;
      }

      if (rightCollider != null && rightCollider.gameObject.layer == LayerMask.NameToLayer("Player")) {
         rb.velocity = transform.right * speed;
      }

      if (transform.position.x >= rightLimit.position.x && rb.velocity.x > 0) {
         rb.velocity = Vector3.zero;
         transform.position = new Vector3(rightLimit.position.x, transform.position.y, transform.position.z);
      }

      if (transform.position.x <= leftLimit.position.x && rb.velocity.x < 0) {
         rb.velocity = Vector3.zero;
         transform.position = new Vector3(leftLimit.position.x, transform.position.y, transform.position.z);
      }
   }

   private void OnCollisionEnter2D(Collision2D collision) {
      if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Player")) {
         Health.GetInstance().OffsetHealth(-1);
      }
   }
}
