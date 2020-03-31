using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachTo : MonoBehaviour {

   [SerializeField]
   public Transform target;
   [SerializeField]
   private bool xAxis = true;
   [SerializeField]
   private bool xFlip = false;
   [SerializeField]
   private bool yAxis = true;
   [SerializeField]
   private bool yFlip = false;
   [SerializeField]
   private bool zAxis = true;
   [SerializeField]
   private bool zFlip = false;

   private Vector3 lastPosition;

   private void Awake() {
      if (target != null) {
         lastPosition = target.transform.position;
      }
   }

   public void SetTarget(Transform newTarget) {
      target = newTarget;
      lastPosition = target.transform.position;
   }

   private void Update() {
      var deltaPosition = Vector3.Scale(target.transform.position - lastPosition, new Vector3(xAxis ? 1 : 0, yAxis ? 1 : 0, zAxis ? 1 : 0));
      deltaPosition.Scale(new Vector3(xFlip ? -1 : 1, yFlip ? -1 : 1, zFlip ? -1 : 1));
      var newPosition = transform.position + deltaPosition;
      transform.position = newPosition;
      lastPosition = target.transform.position;
   }

}
