using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WallGrabber))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerAnimator : MonoBehaviour {

   [SerializeField]
   private Animator animator;
   [SerializeField]
   private Animator impactAnimator;
   private Rigidbody2D rb;
   private WallGrabber wallGrabber;
   private BoxCollider2D boxCollider;

   private void Awake() {
      wallGrabber = GetComponent<WallGrabber>();
      rb = GetComponent<Rigidbody2D>();
      boxCollider = GetComponent<BoxCollider2D>();

      var stateManager = GetComponent<PlayerStateManager>();

      animator.SetTrigger("Jump");
      stateManager.Subscribe(PlayerState.Diving, PlayerState.Idle, LandHard);
      stateManager.Subscribe(PlayerState.Any, PlayerState.Idle, () => animator.SetTrigger("Land"));
      stateManager.Subscribe(PlayerState.Any, PlayerState.Grabbing, () => animator.SetTrigger("Grab"));
      stateManager.Subscribe(PlayerState.Any, PlayerState.Flying, () => animator.SetTrigger("Jump"));
      stateManager.Subscribe(PlayerState.Any, PlayerState.Diving, () => animator.SetTrigger("Dive"));
      stateManager.Subscribe(PlayerState.Any, PlayerState.Sliding, () => {
         animator.ResetTrigger("Land");
         animator.SetTrigger("Slide");
         boxCollider.size = new Vector2(0.95f,0.35f);
         boxCollider.offset = new Vector2(0, 0.16f);
      });
      stateManager.Subscribe(PlayerState.Sliding, PlayerState.Any, () => {
         boxCollider.size = new Vector2(0.26f, 1.03f);
         boxCollider.offset = new Vector2(0, 0.5f);
      });
   }

   private void Update() {

      if (transform.childCount > 0) {
         float yRot = transform.GetChild(0).rotation.eulerAngles.y;
         if (GetComponent<PlayerStateManager>().CurrentState == PlayerState.Grabbing) {
            yRot = 90 + 90 * wallGrabber.GrabbingDirection();
         } else {
            if (rb.velocity.x > 1) {
               yRot = 0;
            } else if (rb.velocity.x < -1) {
               yRot = 180;
            }
         }
         transform.GetChild(0).rotation = Quaternion.Euler(0, yRot, 0);
      }
   }

   private void LandHard() {
      ScreenShake.GetInstance().Shake(1);
      impactAnimator.SetTrigger("Impact");
   }

}
