using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Charges : Singleton<Charges>, IRespawnable {

   [SerializeField]
   private int maxCharges = 3;
   [SerializeField]
   private float fireRechargeDelay = 1;
   [SerializeField]
   private Text numChargesText;

   private float resetTime;
   private int numCharges;

   private void Start() {
      Initalize();
   }

   private void Initalize() {
      if (maxCharges > 0) {
         numCharges = maxCharges;
      } else {
         numCharges = 1;
      }
      numChargesText.text = numCharges.ToString();
   }

   private void Update() {
      if (maxCharges <= 0) {
         return;
      }

      if (new List<PlayerState> { PlayerState.Idle, PlayerState.Grabbing, PlayerState.Sliding }.Contains(PlayerStateManager.GetInstance().CurrentState)) {
         resetTime += Time.deltaTime;
         if (resetTime > fireRechargeDelay) {
            resetTime = 0;
            if (numCharges < maxCharges) {
               numCharges++;
               numChargesText.text = numCharges.ToString();
            }
         }
      }
   }

   public bool UseCharge() {
      if (maxCharges <= 0) {
         return true;
      }

      if (numCharges <= 0) {
         return false;
      }

      numCharges--;
      numChargesText.text = numCharges.ToString();
      resetTime = 0;
      return true;
   }

   public void OnRespawn() {
      numCharges = maxCharges;
   }

}
