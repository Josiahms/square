using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour {

   [SerializeField]
   private float speed = 1;

   private void Update() {
      transform.Rotate(transform.forward, speed * Time.deltaTime);
   }

}
