using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Flyable))]
public class HeatSeeker : MonoBehaviour {

   public static HeatSeeker Instantiate(Transform spawnpoint, Transform target) {
      var instance = Instantiate(PrefabLoader.GetInstance().HeatSeeker, spawnpoint);
      instance.GetComponent<Flyable>().SetTarget(target);
      return instance;
   }

   private void OnCollisionEnter2D(Collision2D collision) {
      if (collision.collider.gameObject.name == "Player") {
         Health.GetInstance().OffsetHealth(-2);
      }
      Destroy(gameObject);
   }
}
