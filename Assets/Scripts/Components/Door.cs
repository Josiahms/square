using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class Door : MonoBehaviour {
   [SerializeField]
   private float height = 5;
   [SerializeField]
   private float openDuration = 0.5f;

   [SerializeField]
   private Transform top;
   [SerializeField]
   private Transform topDoor;
   [SerializeField]
   private Transform bottomDoor;

   [SerializeField]
   private bool isOpen;
   private bool wasOpen;
   private float changeTime;

   private void Update() {
      float scale;
      if (Application.isPlaying) {
         if (isOpen != wasOpen) {
            changeTime = Time.time - (1 - Mathf.Lerp(0, 1, (Time.time - changeTime) / openDuration));
            wasOpen = isOpen;
         }
         scale = Mathf.Lerp(0, 1, (Time.time - changeTime) / openDuration);
         if (isOpen) {
            scale = 1 - scale;
         }
      } else {
         scale = 1;
      }
      topDoor.transform.localScale = new Vector3(topDoor.localScale.x, height * scale, topDoor.localScale.z);
      bottomDoor.transform.localScale = new Vector3(bottomDoor.localScale.x, height * scale, bottomDoor.localScale.z);
      top.transform.position = transform.position + new Vector3(0, height, 0);
   }

   public void Toggle() {
      isOpen = !isOpen;
   }
}
