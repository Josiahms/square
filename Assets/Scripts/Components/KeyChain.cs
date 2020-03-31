using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyChain : Singleton<KeyChain> {

   [SerializeField]
   private List<Image> keySlots;

   private List<Color> keys = new List<Color>();

   private void Start() {
      keySlots.ForEach(x => x.enabled = false);
   }

   public void AddKey(Color keyColor) {
      keys.Add(keyColor);
      UpdateSprites();
   }

   public bool RemoveKey(Color keyColor) {
      var keyIndex = keys.FindIndex(x => CompareColors(x, keyColor));
      if (keyIndex == -1) {
         return false;
      }
      keys.RemoveAt(keyIndex);
      UpdateSprites();
      return true;
   }

   private void UpdateSprites() {
      for (int i = 0; i < keySlots.Count; i++) {
         if (keys.Count > i) {
            keySlots[i].enabled = true;
            keySlots[i].color = keys[i];
         } else {
            keySlots[i].enabled = false;
         }
      }

   }

   private bool CompareColors(Color lhs, Color rhs) {
      return Mathf.Abs(lhs.r - rhs.r) < 0.1f
         && Mathf.Abs(lhs.g - rhs.g)  < 0.1f
         && Mathf.Abs(lhs.b - rhs.b) < 0.1f;
   }

}
