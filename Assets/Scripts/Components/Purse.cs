using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Purse : Singleton<Purse> {

   [SerializeField]
   private Text moneyText;

   private int _moneyAmount;
   private int MoneyAmount {
      get { return _moneyAmount; }
      set {
         _moneyAmount = value;
         moneyText.text = value.ToString().PadLeft(6, '0');
      }
   }

   public void AddMoney(int amount) {
      MoneyAmount += amount;
   }

   public bool SpendMoney(int amount) {
      if (MoneyAmount >= amount) {
         MoneyAmount -= amount;
         return true;
      }
      return false;
   }

}
