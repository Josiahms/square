using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicketStation : MonoBehaviour {
   [SerializeField]
   private float trainSpeed = 3;
   [SerializeField]
   private GameObject player;
   [SerializeField]
   private Transform train;
   [SerializeField]
   private Transform startingPosition;
   [SerializeField]
   private Transform trainStationPosition;
   [SerializeField]
   private Transform departingPosition;

   private Transform targetPosition;

   private void Start() {
      targetPosition = startingPosition;
      train.position = targetPosition.position;
   }

   public void DepartFromStation() {
      player.transform.position = train.transform.position;
      player.AddComponent<AttachTo>().SetTarget(train);
      StartCoroutine(PauseBeforeDeparture());
   }

   private IEnumerator PauseBeforeDeparture() {
      yield return new WaitForSeconds(1);
      player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
      targetPosition = departingPosition;
   }

   public void ArriveAtStation() {
      targetPosition = trainStationPosition;
   }

   public void Update() {
      train.position += (targetPosition.position - train.position) * Time.deltaTime * trainSpeed;
   }

}
