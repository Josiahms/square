using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Turret : MonoBehaviour {

   private const float MIN_ANGLE = 0;
   private const float MAX_ANGLE = 90;

   [SerializeField]
   private Transform muzzle;
   [SerializeField]
   private Transform gun;
   [SerializeField]
   private Transform desiredRotation;

   [SerializeField]
   private float lockOnDelay = 1;
   [SerializeField]
   private float range = 10;
   [SerializeField]
   private float fireDelay = 0.09f;
   [SerializeField]
   private float beamDuration = 0.10f;
   [SerializeField]
   private float cooldown = 0.25f;


   private AudioSource audioSource;
   private bool firing;
   private float? prevTime;
   private Jumper target;
   private List<SpriteRenderer> sprites;
   private LineRenderer lr;

	void Start () {
      target = Jumper.GetInstance();
      sprites = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());
      lr = GetComponentInChildren<LineRenderer>();
      audioSource = GetComponent<AudioSource>();
   }
	
	void Update () {

      if (firing) {
         return;
      }

      sprites.ForEach(s => {
         if (prevTime == null) {
            s.color = Color.white;
         } else {
            float ratio = (Time.time - (float)prevTime) / lockOnDelay;
            s.color = Color.Lerp(Color.white, new Color(0.96f, 0.59f, 0.64f), ratio);
         }
      });

      if (CanHitTarget()) {
         if (prevTime == null) {
            prevTime = Time.time;
         }

         var xRot = Vector3.Angle(Vector3.right, target.transform.position - transform.position);
         if (target.transform.position.y > transform.position.y) {
            xRot = 360 - xRot;
         }
         desiredRotation.rotation = Quaternion.Euler(xRot, 90, 0);
         if (!IsAngleBetween(xRot, 65 + transform.rotation.eulerAngles.z, 210 + transform.rotation.eulerAngles.z)) {
            if (prevTime + lockOnDelay <= Time.time) {
               StartCoroutine(FireRoutine());
            }

            gun.rotation = Quaternion.Lerp(gun.rotation, desiredRotation.rotation, (Time.time - (float)prevTime) / 5);
            return;
         }
      }

      prevTime = null;
   }

   private bool IsAngleBetween(float angle, float a, float b) {
      a %= 360;
      b %= 360;

      if (a < b) {
         return a < angle && angle < b;
      } else {
         return b < angle && angle < a;
      }
   }

   private bool CanHitTarget() {
      var distanceVector = target.transform.position - transform.position;
      if (distanceVector.magnitude > range) {
         return false;
      }

      RaycastHit2D hit = Physics2D.Raycast(gun.transform.position, distanceVector, range);
      return hit.collider != null && hit.collider.gameObject == target.gameObject;
   }

   private IEnumerator FireRoutine() {
      firing = true;
      lr.SetPosition(1, lr.transform.InverseTransformPoint(muzzle.transform.position + muzzle.transform.forward * range));
      sprites.ForEach(s => s.color = Color.red);
      yield return new WaitForSeconds(fireDelay);
      audioSource.Play();
      lr.enabled = true;
      if (CanHitTarget()) {
         //target.Respawn();
      }
      yield return new WaitForSeconds(beamDuration);
      lr.enabled = false;
      sprites.ForEach(s => s.color = Color.white);
      yield return new WaitForSeconds(cooldown);
      firing = false;
      prevTime = Time.time;
   }
}
