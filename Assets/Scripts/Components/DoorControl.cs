using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour {

   [SerializeField]
   private Door door;
   [SerializeField]
   private bool requireKey;
   [SerializeField]
   private SpriteRenderer keySprite;


   private void Start() {
      keySprite.enabled = requireKey;
   }

   private void OnTriggerEnter2D(Collider2D collision) {

      if (collision.gameObject.name == "Player" && (!requireKey || KeyChain.GetInstance().RemoveKey(keySprite.color))) {
         keySprite.color = new Color(keySprite.color.r, keySprite.color.g, keySprite.color.b, 1);
         door.Toggle();
         GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);
      }

   }
}
