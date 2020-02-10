using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WallGrabber))]
[RequireComponent(typeof(Rigidbody2D))]
public class TimeDialator : MonoBehaviour {

   [SerializeField]
   private float velocityThreshold = 5;

   private WallGrabber wallGrabber;
   private float nextDialationTime;
   private Rigidbody2D rb;

   private void Awake() {
      rb = GetComponent<Rigidbody2D>();
      nextDialationTime = Time.time;
      wallGrabber = GetComponent<WallGrabber>();
   }

   private void Update() {

      if (wallGrabber.IsMakingContact()) {
         Release();
      } else if (rb.velocity.y < velocityThreshold && Time.time > nextDialationTime) {
         Time.timeScale = 0.2f;
      }
   }

   public void Release() {
      Time.timeScale = 1;
      nextDialationTime = Time.time + 0.25f;
   }

}
