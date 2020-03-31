using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class WallGrabber : MonoBehaviour {

   [SerializeField]
   private float minTimeBetweenGrab = 0.125f;

   private Rigidbody2D rb;
   private int grabbingDirection; // -1 left, 1 right, 0 not grabbing
   private BoxCollider2D boxCollider2D;

   private const float FLAT_NORMAL_THRESHOLD = 0.9f;
   private const float DIVE_VELOCITY_THRESHOLD = -20f;
   private const float DOWN_SLIDE_THRESHOLD = 7f;
   private const float DOWN_SLIDE_STOP_THRESHOLD = 6f;
   private const float NORMAL_GRAVITY = 2.6f;
   private const float GRABBING_GRAVITY = 0.5f;

   private void Awake() {
      rb = GetComponent<Rigidbody2D>();
      boxCollider2D = GetComponent<BoxCollider2D>();
      GetComponent<PlayerStateManager>().Subscribe(PlayerState.Any, PlayerState.Grabbing, () => {
         rb.gravityScale = GRABBING_GRAVITY;
         rb.velocity = Vector2.zero;
      });
      GetComponent<PlayerStateManager>().Subscribe(PlayerState.Grabbing, PlayerState.Any, () => { rb.gravityScale = NORMAL_GRAVITY; });
   }

   private void Update() {
      if (rb.velocity.y < DIVE_VELOCITY_THRESHOLD) {
         GetComponent<PlayerStateManager>().SetState(PlayerState.Diving);
      }

      if (IsGrounded()) {
         if (Mathf.Abs(rb.velocity.x) > DOWN_SLIDE_THRESHOLD) {
            GetComponent<PlayerStateManager>().SetState(PlayerState.Sliding);
         } else if (Mathf.Abs(rb.velocity.x) < DOWN_SLIDE_STOP_THRESHOLD) {
            GetComponent<PlayerStateManager>().SetState(PlayerState.Idle);
         }
      }
   }

   private int numColliders = 0;
   private void OnCollisionEnter2D(Collision2D collision) {
      if (collision.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast")) {
         return;
      }

      numColliders++;

      if (collision.contacts[0].normal.y < -FLAT_NORMAL_THRESHOLD) {

      } else if (collision.contacts[0].normal.y > FLAT_NORMAL_THRESHOLD) {
         GetComponent<PlayerStateManager>().SetState(PlayerState.Idle);
      } else if (!IsGrounded()) {
         GetComponent<PlayerStateManager>().SetState(PlayerState.Grabbing);
         grabbingDirection = (int)Mathf.Sign(collision.transform.position.x - transform.position.x);
      }
   }

   private void OnCollisionExit2D(Collision2D collision) {
      if (collision.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast")) {
         return;
      }

      numColliders--;

      if (numColliders == 0) {
         if (IsGrabbing()) {
            Release();
         }
         GetComponent<PlayerStateManager>().SetState(PlayerState.Flying);
      }
   }

   private void FixedUpdate() {
      /*if (IsGrabbing()) {
         rb.AddForce(new Vector2(grabbingDirection * 200, 0));
         rb.sharedMaterial.friction = 0;
      }  else {
         rb.sharedMaterial.friction = 0.4f;
      }*/
   }

   public int GrabbingDirection() {
      return grabbingDirection;
   }

   private bool IsGrabbing() {
      return GetComponent<PlayerStateManager>().CurrentState == PlayerState.Grabbing;
   }

   private bool IsGrounded() {
      return GetComponent<PlayerStateManager>().CurrentState == PlayerState.Idle 
         || GetComponent<PlayerStateManager>().CurrentState == PlayerState.Sliding;
   }

   public bool IsMakingContact() {
      return IsGrabbing() || IsGrounded();
   }

   public void Release() {
      grabbingDirection = 0;
   }

}
