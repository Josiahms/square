using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public enum PlayerState {
   Any,
   Idle,
   Flying,
   Diving,
   Grabbing,
   Sliding
}

public class PlayerStateManager : Singleton<PlayerStateManager> {

   [SerializeField]
   public PlayerState PrevState;// { get; private set; }
   [SerializeField]
   public PlayerState CurrentState;// { get; private set; }

   private struct StatechangeSubscriber {

      public PlayerState prevState;
      public PlayerState newState;
      public UnityAction callback;

      public StatechangeSubscriber(PlayerState prevState, PlayerState newState, UnityAction callback) {
         this.prevState = prevState;
         this.newState = newState;
         this.callback = callback;
      }

   }

   private List<StatechangeSubscriber> subscribers = new List<StatechangeSubscriber>();

   public void SetState(PlayerState newState) {
      if (CurrentState != newState) {
         PrevState = CurrentState;
         CurrentState = newState;

         subscribers
            .Where(x => 
               (x.prevState == PlayerState.Any || x.prevState == PrevState) && 
               (x.newState == PlayerState.Any || x.newState == newState)).ToList()
            .ForEach(x => x.callback());
      }
   }

   public void Subscribe(PlayerState prevState, PlayerState newState, UnityAction callback) {
      subscribers.Add(new StatechangeSubscriber(prevState, newState, callback));
   }
}
