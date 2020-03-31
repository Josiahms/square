using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IRespawnable {
   void OnRespawn();
}

public class Respawnable : MonoBehaviour {
   [SerializeField]
   private Transform spawnpoint;
   private Vector3 spawnPosition;

   private void Start() {
      if (spawnpoint == null) {
         spawnPosition = transform.position;
      } else {
         spawnPosition = spawnpoint.position;
      }
   }

   public void Respawn() {
      transform.position = spawnPosition;
      GetComponentsInChildren<IRespawnable>().ToList().ForEach(x => x.OnRespawn());
      MainCanvas.GetInstance().GetComponentsInChildren<IRespawnable>().ToList().ForEach(x => x.OnRespawn());
   }

}
