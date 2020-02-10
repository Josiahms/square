using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WallGrabber))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerAnimator : MonoBehaviour {

   private Animator animator;
   private Rigidbody2D rb;
   private WallGrabber wallGrabber;

   private void Awake() {
      animator = GetComponentInChildren<Animator>();
      wallGrabber = GetComponent<WallGrabber>();
      rb = GetComponent<Rigidbody2D>();

      var stateManager = GetComponent<PlayerStateManager>();

      animator.SetTrigger("Jump");
      stateManager.Subscribe(PlayerState.Diving, PlayerState.Idle, LandHard);
      stateManager.Subscribe(PlayerState.Any, PlayerState.Idle, () => animator.SetTrigger("Land"));
      stateManager.Subscribe(PlayerState.Any, PlayerState.Grabbing, () => animator.SetTrigger("Grab"));
      stateManager.Subscribe(PlayerState.Any, PlayerState.Flying, () => animator.SetTrigger("Jump"));
      stateManager.Subscribe(PlayerState.Any, PlayerState.Sliding, () => {
         animator.ResetTrigger("Land");
         animator.SetTrigger("Slide"); });
   }

   private void Update() {

      if (transform.childCount > 0) {
         float yRot = transform.GetChild(0).rotation.eulerAngles.y;
         float zRot = transform.GetChild(0).rotation.eulerAngles.z;
         if (GetComponent<PlayerStateManager>().CurrentState == PlayerState.Grabbing) {
            yRot = 90 + 90 * wallGrabber.GrabbingDirection();
         } else {
            if (rb.velocity.x > 1) {
               yRot = 0;
            } else if (rb.velocity.x < -1) {
               yRot = 180;
            }
         }

         if (GetComponent<PlayerStateManager>().CurrentState == PlayerState.Diving) {
            zRot = 200;
         } else {
            zRot = 0;
         }

         transform.GetChild(0).rotation = Quaternion.Euler(0, yRot, zRot);
      }
   }

   private void LandHard() {
      ScreenShake.GetInstance().Shake(1);
   }

}
