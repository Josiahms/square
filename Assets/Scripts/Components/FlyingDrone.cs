using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingDrone : MonoBehaviour, FlyableSubscriber {

   [SerializeField]
   private float repositionDistanceThreshold = 6f;
   [SerializeField]
   private float fleeThreshold = 3f;
   [SerializeField]
   private float reachedTargetThreshold = 0.5f;
   [SerializeField]
   private float outOfRangeThreshold = 30f;
   [SerializeField]
   private Transform muzzle;
   [SerializeField]
   private Bullet bulletPrefab;
   [SerializeField]
   private float targetingTime = 0.5f;
   [SerializeField]
   private float reloadTime = 1.5f;
   [SerializeField]
   private float timeBetweenShots = 0.15f;

   private bool relocate;
   private Flyable flyable;
   private Coroutine fireRoutine;

   private void Start() {
      flyable = GetComponent<Flyable>();
      flyable.AddSubscriber(this);
      flyable.enabled = false;
   }

   private void Update() {
      var distanceToTarget = (transform.position - DroneTarget.GetInstance().transform.position).magnitude;
      var distanceToPlayer = (Jumper.GetInstance().transform.position - transform.position).magnitude;
      var directionToPlayer = (Jumper.GetInstance().transform.position - transform.position).normalized;
      var outOfRange = distanceToPlayer > outOfRangeThreshold;

      flyable.enabled = !outOfRange || flyable.enabled;

      var tooFarFromTarget = distanceToTarget > repositionDistanceThreshold;
      if (tooFarFromTarget) {
         relocate = true;
      }

      var playerNotTooClose = distanceToPlayer < fleeThreshold;
      if (!relocate && distanceToPlayer < fleeThreshold) {
         relocate = true;
      }

      if (relocate) {
         flyable.SetTarget(DroneTarget.GetInstance().transform.position, directionToPlayer);
         if (distanceToPlayer < reachedTargetThreshold) {
            relocate = false;
         }
      } else {
         flyable.SetTarget(flyable.transform.position, directionToPlayer);
      }
   }

   public void OnHasReachedDestination() {
      fireRoutine = StartCoroutine(FireRoutine());
   }

   public void OnChangePosition() {
      if (fireRoutine != null) {
         StopCoroutine(fireRoutine);
      }
   }

   private IEnumerator FireRoutine() {
      yield return new WaitForSeconds(targetingTime);
      while (true) {
         Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
         yield return new WaitForSeconds(timeBetweenShots);
         Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
         yield return new WaitForSeconds(timeBetweenShots);
         Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
         yield return new WaitForSeconds(reloadTime);
      }
   }
}
