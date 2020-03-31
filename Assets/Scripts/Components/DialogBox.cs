using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class DialogBox : Singleton<DialogBox> {

   [SerializeField]
   private Text pageCount;
   [SerializeField]
   private Text messageText;
   [SerializeField]
   private float openTransitionTime = 0.2f;
   [SerializeField]
   private float boxHeight = 830f;
   [SerializeField]
   private Button dialogButtonPrefab;

   private int selectedActionButton;
   private Button advanceButton;
   private RectTransform rt;
   private bool isOpen;
   private float transitionTime;
   private List<DialogAction> actions;
   private List<Button> actionButtons = new List<Button>();
   private List<string> messages;
   private int currentMessage = 0;
   private Image speechBubble;
   private int zoomLevelId;

   private void Start() {
      rt = GetComponent<RectTransform>();
      transitionTime = float.MinValue;
      advanceButton = GetComponentInChildren<Button>();
      advanceButton.onClick.AddListener(Advance);
      
   }

   private void InstatiateDialogButtons() {
      foreach (var actionButton in actionButtons) {
         Destroy(actionButton);
      }
      actionButtons.Clear();
      for (int i = 0; i < actions.Count; i++) {
         var j = i; // Must make a copy of i for it to work in asynchronous listener
         var dialogButton = Instantiate(dialogButtonPrefab, transform);
         dialogButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, -125 * (i + 1) - 425, 0);
         dialogButton.GetComponentsInChildren<Text>()[1].text = actions[j].message;
         dialogButton.onClick.AddListener(() => {
            if (selectedActionButton == j) {
               actions[j].onSelect.Invoke();
            }
            selectedActionButton = j;
         });
         actionButtons.Add(dialogButton);
      }
      if (actionButtons.Count > 0) {
         selectedActionButton = 0;
         actionButtons[selectedActionButton].Select();
      }
   }

   private void Update() {
      var interpolationAmount = (Time.time - transitionTime) / openTransitionTime;
      if (!isOpen) {
         interpolationAmount = 1 - interpolationAmount;
      }
      var newHeight = Mathf.Lerp(0, boxHeight, interpolationAmount);
      rt.sizeDelta = new Vector2(rt.sizeDelta.x, newHeight);
      advanceButton.gameObject.SetActive(interpolationAmount >= 1);
   }

   private void Open() {
      if (isOpen) {
         return;
      }
      isOpen = true;
      transitionTime = Time.time;
      zoomLevelId = RootCamera.GetInstance().PushZoomLevel(RootCamera.ZoomLevel.Close);
   }

   public void Close() {
      if (!isOpen) {
         return;
      }
      isOpen = false;
      transitionTime = Time.time;
      RootCamera.GetInstance().RemoveZoomLevel(zoomLevelId);
      speechBubble.gameObject.SetActive(false);
   }

   private void Advance() {
      if (currentMessage >= messages.Count) {
         Close();
      } else {
         pageCount.text = string.Format("({0}/{1})", currentMessage + 1, messages.Count);
         messageText.text = messages[currentMessage++];
      }
   }

   public void SetMessages(List<string> messages, List<DialogAction> actions, Image speechBubble) {
      if (messages.Count <= 0) {
         return;
      }
      this.speechBubble = speechBubble;
      this.messages = messages;
      this.actions = actions;
      currentMessage = 0;
      pageCount.text = string.Format("({0}/{1})", currentMessage + 1, messages.Count);
      messageText.text = messages[currentMessage++];
      InstatiateDialogButtons();
      Open();
   }
}
