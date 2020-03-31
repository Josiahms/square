using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ZoomZone : MonoBehaviour {

   [SerializeField]
   private RootCamera.ZoomLevel zoomLevel = RootCamera.ZoomLevel.Normal;

   private int zoomId;

   private void OnTriggerEnter2D(Collider2D collision) {
      if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) {
         zoomId = RootCamera.GetInstance().PushZoomLevel(zoomLevel);
      }
   }

   private void OnTriggerExit2D(Collider2D collision) {
      if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) {
         RootCamera.GetInstance().RemoveZoomLevel(zoomId);
      }
   }

}
