using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public struct DialogAction {

   public string message;
   public UnityEvent onSelect;

}

public class Dialog : MonoBehaviour {

   [SerializeField]
   private Text dialogIndicator;
   [SerializeField]
   private Image speechBubble;
   [SerializeField]
   private bool isImportant;

   [SerializeField, TextArea]
   private List<string> messages;
   [SerializeField]
   private List<DialogAction> actions;

   private void Awake() {
      GetComponentInChildren<Button>().onClick.AddListener(Talk);
      dialogIndicator.gameObject.SetActive(isImportant);
   }

   private void Talk() {
      DialogBox.GetInstance().SetMessages(messages, actions, speechBubble);
      speechBubble.gameObject.SetActive(true);
      dialogIndicator.gameObject.SetActive(false);
   }

}
