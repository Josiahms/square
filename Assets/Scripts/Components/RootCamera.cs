using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class RootCamera : Singleton<RootCamera> {

   public enum ZoomLevel { Close, Normal, Far }

   private Camera cam;
   private float targetFOV;
   private Vector3 targetDelta;
   private Vector3 currentDelta;

   private int zoomCalls;

   private int maxZoomLevelId;
   private Dictionary<int, ZoomLevel> zoomLevels = new Dictionary<int, ZoomLevel>();

   private void Start() {
      cam = GetComponentInChildren<Camera>();
      targetFOV = cam.fieldOfView;
   }

   private void Update() {
      AdjustCamera();
   }

   public int PushZoomLevel(ZoomLevel zoomLevel) {
      maxZoomLevelId++;
      zoomLevels.Add(maxZoomLevelId, zoomLevel);
      SetTargets();
      return maxZoomLevelId;
   }

   public void RemoveZoomLevel(int zoomId) {
      zoomLevels.Remove(zoomId);
      maxZoomLevelId = zoomLevels.Count > 0 ? zoomLevels.Keys.ToList().Max() : 0;
      SetTargets();
   }

   private void SetTargets() {
      var currentZoomLevel = zoomLevels.Count > 0 ? zoomLevels[maxZoomLevelId] : ZoomLevel.Normal;
      switch (currentZoomLevel) {
         case ZoomLevel.Close:
            targetDelta = new Vector3(0, -7, 0);
            targetFOV = 30;
            return;
         case ZoomLevel.Normal:
            targetDelta = new Vector3(0, 0, 0);
            targetFOV = 60;
            return;
         case ZoomLevel.Far:
            targetDelta = new Vector3(0, 0, -40);
            targetFOV = 60;
            return;
      }
   }

   private void AdjustCamera() {
      var moveDelta = Time.deltaTime * (targetDelta - currentDelta) * 5;
      currentDelta += moveDelta;
      transform.position += moveDelta;
      cam.fieldOfView += Time.deltaTime * (targetFOV - cam.fieldOfView) * 5;
   }

}
