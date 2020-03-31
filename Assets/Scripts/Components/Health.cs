using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class Health : Singleton<Health>, IRespawnable {
   [SerializeField]
   private RectTransform healthImage;
   [SerializeField]
   private RectTransform maxHealthImage;

   private int health = 8;
   private int maxHealth = 8;
   private float invulnerabilityEndTime;

   private void Start() {
      healthImage.sizeDelta = new Vector2(13 * health / 2, healthImage.sizeDelta.y);
      maxHealthImage.sizeDelta = new Vector2(13 * maxHealth / 2, maxHealthImage.sizeDelta.y);
   }

   public void SetHealth(int newHealth) {
      if (health == newHealth) {
         // Avoids side effects of setting health to 0 recursively and respawning
         return;
      }
      health = Mathf.Min(maxHealth, newHealth);
      healthImage.sizeDelta = new Vector2(13 * health / 2, healthImage.sizeDelta.y);
      if (health <= 0) {
         Jumper.GetInstance().GetComponent<Respawnable>().Respawn();
      }
   }

   public void OffsetHealth(int offsetAmount) {
      if (offsetAmount < 0 && Time.time > invulnerabilityEndTime) {
         SetHealth(health + offsetAmount);
         invulnerabilityEndTime = Time.time + 0.5f;
         Jumper.GetInstance().GetComponent<Rigidbody2D>().velocity += Vector2.up * 20f;
         FloatingHealthText.Instantiate(-offsetAmount);
      } else if (offsetAmount > 0) {
         SetHealth(health + offsetAmount);
      }
   }

   public void SetMaxHealth(int newMaxHealth) {
      if (newMaxHealth == 0) {
         return;
      }
      maxHealth = newMaxHealth;
      maxHealthImage.sizeDelta = new Vector2(13 * maxHealth / 2, maxHealthImage.sizeDelta.y);
      SetHealth(health);
   }

   public void OnRespawn() {
      SetMaxHealth(8);
      SetHealth(8);
   }

   public bool IsAtAMax() {
      return health >= maxHealth;
   }
}
