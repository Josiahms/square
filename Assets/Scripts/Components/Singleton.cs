using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour {

   private static T instance;

   public static T GetInstance() {
      return instance;
   }

   private void Awake() {
      instance = GetComponent<T>();
   }

}
