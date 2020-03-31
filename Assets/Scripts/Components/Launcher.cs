using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour {

   [SerializeField]
   private Transform spawnpoint;
   [SerializeField]
   private float reloadTime = 3f;

   private float nextShotTime;
   private HeatSeeker instance;
   private LineRenderer lineRenderer;
   private float lineRendererFocusAngle;

   private const float LINE_RENDERER_STARTING_FOCUS_ANGLE = 45;

   private void Start() {
      nextShotTime = Time.time;
      lineRenderer = GetComponentInChildren<LineRenderer>();
      lineRendererFocusAngle = LINE_RENDERER_STARTING_FOCUS_ANGLE;
      lineRenderer.positionCount = 3;
      lineRenderer.SetPosition(1, Vector3.zero);
   }

   private HeatSeeker heatSeeker;
   private bool couldSeeTarget = true;
   private void Update() {
      var target = Jumper.GetInstance().transform;

      if (!CanSeeTarget() || heatSeeker != null) {
         nextShotTime = Time.time;
      }

      if (nextShotTime + reloadTime < Time.time) {
         heatSeeker = HeatSeeker.Instantiate(spawnpoint, target);
      }

      lineRenderer.enabled = nextShotTime < Time.time;

      var distanceVect = target.position - lineRenderer.transform.position;
      var angleToPlayer = Vector3.Angle(transform.up, distanceVect);
      var flipDetectionAngle = Vector3.Angle(transform.right, distanceVect);

      if (flipDetectionAngle < 90) {
         angleToPlayer = 360 - angleToPlayer;
      }

      angleToPlayer += transform.rotation.eulerAngles.z + 90;

      var t = 1 - (nextShotTime + reloadTime - Time.time - 0.35f) / (reloadTime);
      var angleOffset = Mathf.Lerp(45, 0, t);
      var color = Color.Lerp(new Color(1, 0, 0, 0), new Color(1, 0, 0, 0.75f), t);
      lineRenderer.startColor = color;
      lineRenderer.endColor = color;

      var angle1 = Mathf.Deg2Rad * (angleToPlayer + angleOffset);
      var angle2 = Mathf.Deg2Rad * (angleToPlayer - angleOffset);

      var ray1 = new Vector3(Mathf.Cos(angle1), Mathf.Sin(angle1));
      var ray2 = new Vector3(Mathf.Cos(angle2), Mathf.Sin(angle2));

      //Debug.DrawRay(lineRenderer.transform.position, new Vector2(Mathf.Cos(Mathf.Deg2Rad * angleToPlayer), Mathf.Sin(Mathf.Deg2Rad * angleToPlayer)) * 10);
      //Debug.DrawRay(lineRenderer.transform.position, ray1 * 5, Color.blue);
      //Debug.DrawRay(lineRenderer.transform.position, ray2 * 5, Color.green);

      var raycastHit1 = Physics2D.Raycast(lineRenderer.transform.position, ray1, float.MaxValue);
      var raycastHit2 = Physics2D.Raycast(lineRenderer.transform.position, ray2, float.MaxValue);

      var point1 = raycastHit1.transform != null ? new Vector3(raycastHit1.point.x, raycastHit1.point.y, target.position.z) : (lineRenderer.transform.position + (ray1 * 1000));
      var point2 = raycastHit2.transform != null ? new Vector3(raycastHit2.point.x, raycastHit2.point.y, target.position.z) : (lineRenderer.transform.position + (ray2 * 1000));

      lineRenderer.useWorldSpace = true;
      lineRenderer.SetPosition(0, point1);
      lineRenderer.SetPosition(1, lineRenderer.transform.position);
      lineRenderer.SetPosition(2, point2);

      couldSeeTarget = CanSeeTarget();
   }

   private bool CanSeeTarget() {
      var direction = Jumper.GetInstance().transform.position - transform.position;
      var hit = Physics2D.Raycast(transform.position + direction.normalized * 0.5f, direction, direction.magnitude, 524283);
      return hit.collider.gameObject.layer == LayerMask.NameToLayer("Player");
   }

}