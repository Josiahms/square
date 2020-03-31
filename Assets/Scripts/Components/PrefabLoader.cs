using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabLoader : Singleton<PrefabLoader> {

   [SerializeField]
   private HeatSeeker heatSeeker;
   public HeatSeeker HeatSeeker { get { return heatSeeker; } }

   [SerializeField]
   private FloatingHealthText floatingHealthText;
   public FloatingHealthText FloatingHealthText { get { return floatingHealthText;  } }

}
