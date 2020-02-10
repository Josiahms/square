using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Charges))]
public class Jumper : Singleton<Jumper>, IRespawnable {

   [SerializeField]
   private float forceScale;
   [SerializeField]
   private float maxForce;

   private Charges charges;
   private Rigidbody2D rb;
   private AudioSource audioSource;
   private LineRenderer lr;
   private TimeDialator timeDialator;
   private Vector2? prevInputPosition;

   private const float DOWN_ATTACK_THRESHOLD = -0.95f;
   private const float NORMAL_DRAG = 0.5f;
   private const float SLIDING_DRAG = 0.05f;

   private void Awake() {
      lr = GetComponent<LineRenderer>();
      lr.positionCount = 2;
      lr.SetPosition(0, Vector3.zero);
      rb = GetComponent<Rigidbody2D>();
      audioSource = GetComponent<AudioSource>();
      timeDialator = GetComponent<TimeDialator>();
      charges = GetComponent<Charges>();
      Initialize();

      GetComponent<PlayerStateManager>().Subscribe(PlayerState.Any, PlayerState.Sliding, () => rb.drag = SLIDING_DRAG);
      GetComponent<PlayerStateManager>().Subscribe(PlayerState.Sliding, PlayerState.Any, () => rb.drag = NORMAL_DRAG);
   }

   private void Initialize() {
      lr.enabled = false;
   }

   private bool IsInputting() {
      return Input.GetMouseButton(0) || Input.touchCount > 0;
   }

   private Vector2 GetInputPosition() {
      if (Input.touchCount > 0) {
         return Input.GetTouch(0).position;
      }
      return Input.mousePosition;
   }

   private void Update() {

      if (IsInputting()) {
         lr.enabled = true;
         if (prevInputPosition == null) {
            prevInputPosition = GetInputPosition();
         }
      }

      if (lr.enabled) {
         if (IsInputting()) {
            Vector2 pos = (GetInputPosition() - (Vector2)prevInputPosition) / 75;
            lr.SetPosition(1, new Vector3(pos.x, pos.y, 0));
         } else {
            if (charges.UseCharge()) {
               Launch();
            }
            lr.enabled = false;
            prevInputPosition = null;
         }
      }
   }

   private void Launch() {
      // Jumper Specific
      rb.velocity = new Vector2(0, 0);
      Vector2 nextForce = Vector2.ClampMagnitude((lr.GetPosition(0) - lr.GetPosition(1)) * forceScale, maxForce);
      if (nextForce.normalized.y < DOWN_ATTACK_THRESHOLD) {
         nextForce = Vector2.down * maxForce * 2;
      }

      rb.AddRelativeForce(nextForce);

      // A little out of scope
      if (timeDialator != null) {
         timeDialator.Release();
      }
      audioSource.PlayOneShot(audioSource.clip);
   }

   public void OnRespawn() {
      rb.velocity = new Vector2(0, 0);
   }
}
