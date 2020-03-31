using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface FlyableSubscriber {
   void OnHasReachedDestination();
   void OnChangePosition();
}

[RequireComponent(typeof(Rigidbody2D))]
public class Flyable : MonoBehaviour {

   [SerializeField]
   private float speed = 5;
   [SerializeField]
   private float turnSpeed = 3f;
   [SerializeField]
   private float brakingDistance = 2f;
   [SerializeField]
   private float reachedDestinationThreshold = 0.5f;

   private List<FlyableSubscriber> subscribers = new List<FlyableSubscriber>();
   private Rigidbody2D rb;
   private Transform target;
   private Vector3 targetPosition;
   private Vector3 targetDirection;
   private bool hadReachedDestination;

   private void Start() {
      rb = GetComponent<Rigidbody2D>();
   }

   public void AddSubscriber(FlyableSubscriber subscriber) {
      subscribers.Add(subscriber);
   }

   public void SetTarget(Transform target) {
      this.target = target;
   }

   public void SetTarget(Vector3 target, Vector3 targetDirection) {
      this.target = null;
      this.targetDirection = targetDirection;
      targetPosition = target;
   }

   private void Update() {
      if (target != null) {
         targetPosition = target.position;
         targetDirection = transform.forward;
      }

      float forwardAngle;
      var distanceToTarget = (transform.position - targetPosition).magnitude;
      if (distanceToTarget < reachedDestinationThreshold) {
         rb.velocity = Vector2.zero;
         forwardAngle = Vector3.Angle(transform.forward, targetDirection);
         var rightAngle = Vector2.Angle(transform.up, targetDirection);
         if (rightAngle < 90) {
            forwardAngle *= -1;
         }

         if (!hadReachedDestination) {
            hadReachedDestination = true;
            subscribers.ForEach(subscriber => subscriber.OnHasReachedDestination());
         }

      } else {
         if (brakingDistance <= 0) {
            rb.velocity = transform.forward * speed;
         } else {
            rb.velocity = transform.forward * Mathf.Lerp(0.5f, speed, distanceToTarget / brakingDistance);
         }
         forwardAngle = Vector3.Angle(transform.forward, targetPosition - transform.position);
         var rightAngle = Vector2.Angle(transform.up, targetPosition - transform.position);
         if (rightAngle < 90) {
            forwardAngle *= -1;
         }

         if (hadReachedDestination) {
            hadReachedDestination = false;
            subscribers.ForEach(subscriber => subscriber.OnChangePosition());
         }
      }

      transform.Rotate(new Vector3(forwardAngle * Time.deltaTime * turnSpeed, 0, 0));
   }

}
