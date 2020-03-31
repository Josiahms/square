using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FloatingHealthText : MonoBehaviour {

   [SerializeField]
   private float startingSpeed = 10;
   [SerializeField]
   private RectTransform image;

   public static FloatingHealthText Instantiate(int damageAmount) {
      var instance = Instantiate(PrefabLoader.GetInstance().FloatingHealthText, Jumper.GetInstance().transform.position + Vector3.up * 0.5f, new Quaternion());
      instance.image.sizeDelta = new Vector2((float)damageAmount / 2, instance.image.sizeDelta.y);
      return instance;
   }

   private void Start() {
      GetComponent<Canvas>().worldCamera = Camera.main;
      GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-0.5f, 0.5f), 1) * startingSpeed;
   }

}
