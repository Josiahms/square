using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AltitudeMeter : MonoBehaviour {

   [SerializeField]
   private float multiplier = 5;
   [SerializeField]
   private float offset = 0;
   [SerializeField]
   private float mod = 160;
   [SerializeField]
   private RectTransform meter;
   [SerializeField]
   private Text text;
   [SerializeField]
   private Transform groundZero;

   private Transform target;
   private float initialHeight;

   private void Start() {

      target = Jumper.GetInstance().transform;

      if (groundZero != null) {
         initialHeight = groundZero.position.y;
      } else {
         initialHeight = target.position.y;
      }
   }

   private void Update() {
      var yPos = (multiplier * (initialHeight - target.position.y)) % mod + offset;
      text.text = Mathf.Round((target.position.y - initialHeight) * 10) / 10 + "m";
      meter.anchoredPosition = new Vector2(meter.anchoredPosition.x, yPos);
   }

}
