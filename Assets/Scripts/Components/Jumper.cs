using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(TimeDialator))]
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
   private const float SLIDING_DRAG = 0.03f;

   private void Start() {
      lr = GetComponent<LineRenderer>();
      lr.positionCount = 2;
      lr.SetPosition(0, Vector3.zero);
      rb = GetComponent<Rigidbody2D>();
      audioSource = GetComponent<AudioSource>();
      timeDialator = GetComponent<TimeDialator>();
      charges = Charges.GetInstance();
      Initialize();

      GetComponent<PlayerStateManager>().Subscribe(PlayerState.Any, PlayerState.Sliding, () => rb.drag = SLIDING_DRAG);
      GetComponent<PlayerStateManager>().Subscribe(PlayerState.Sliding, PlayerState.Any, () => rb.drag = NORMAL_DRAG);
   }

   private void Initialize() {
      lr.enabled = false;
   }

   private bool IsInputting() {
      return (Input.GetMouseButton(0) || Input.touchCount > 0) && (!EventSystem.current.IsPointerOverGameObject() || lr.enabled);
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
      if (nextForce.normalized.y < 0) {
         nextForce *= 1.5f;
      }

      rb.AddRelativeForce(nextForce);

      // A little out of scope
      if (timeDialator != null) {
         timeDialator.Release();
      }
      if (GetComponent<PlayerStateManager>().CurrentState == PlayerState.Diving) {
         GetComponent<PlayerStateManager>().SetState(PlayerState.Flying);
      }

      audioSource.PlayOneShot(audioSource.clip);
   }

   public void OnRespawn() {
      rb.velocity = new Vector2(0, 0);
   }
}
