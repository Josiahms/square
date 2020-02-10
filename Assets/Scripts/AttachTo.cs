using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachTo : MonoBehaviour {

   [SerializeField]
   private Transform target;

   private Vector3 lastPosition;

   private void Awake() {
      lastPosition = target.transform.position;
   }

   private void Update() {
      transform.position += target.transform.position - lastPosition;
      lastPosition = target.transform.position;
   }

}
