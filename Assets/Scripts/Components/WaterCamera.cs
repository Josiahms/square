using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCamera : MonoBehaviour {

   [SerializeField]
   private Transform mainCamera;
   [SerializeField]
   private MeshRenderer waterMesh;

   private Camera waterCamera;
   private Vector3 lastPosition;

   private void Awake() {
      waterCamera = GetComponent<Camera>();
      lastPosition = mainCamera.position;
   }

   private void LateUpdate() {
      var delta = mainCamera.position - lastPosition;
      waterCamera.lensShift = new Vector2(waterCamera.lensShift.x, GetLensShiftY());

      lastPosition = mainCamera.position;

      var deltaTextureOffsetX = delta.x * 0.0108f;

      waterMesh.material.mainTextureOffset = new Vector2(GetTextureOffsetX(), waterMesh.material.mainTextureOffset.y);
   }

   private float GetTextureOffsetX() {
      var distanceBetweenCameraAndWater = waterCamera.transform.position.x - waterMesh.transform.position.x;
      var offset = (distanceBetweenCameraAndWater + 10.44525f) * 0.0108f - 0.113f; // All numbers based on measurements in editor
      return offset;
   }

   private float GetLensShiftY() {
      var distBetweenWaterCamAndWater = waterCamera.transform.position.y - waterMesh.transform.position.y;
      var shift = (distBetweenWaterCamAndWater - 38.01795f) * 0.0217f - 0.176f; // All numbers based on measurements in editor
      return shift;
   }

}
