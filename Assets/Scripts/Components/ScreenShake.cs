using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ScreenShake : Singleton<ScreenShake> {
   [SerializeField]
   private float frequency = 5;
   [SerializeField]
   private float scale = 5;
   [SerializeField]
   private float duration = 5;

   private float shakeTime;

   private void Update() {
      var frequency2 = Mathf.Lerp(Mathf.Sin(Time.time * frequency), 0, (Time.time - shakeTime) / duration);
      var scale2 = Mathf.Lerp(scale, 0, (Time.time - shakeTime) / duration);
      transform.localPosition = new Vector3(0,  frequency2 * scale2, 0);
   }

   public void Shake(float amount) {
      shakeTime = Time.time;
   }

}
