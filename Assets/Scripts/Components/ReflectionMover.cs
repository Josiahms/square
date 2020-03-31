using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionMover : MonoBehaviour {

   private void Update() {
      var playerPosition = Camera.main.transform.position;
      var distanceToPlane = playerPosition.y - transform.parent.position.y;
      transform.position = new Vector3(playerPosition.x, -distanceToPlane, playerPosition.z);
   }

}
