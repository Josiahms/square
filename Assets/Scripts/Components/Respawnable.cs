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
      GetComponents<IRespawnable>().ToList().ForEach(x => x.OnRespawn());
   }

}
