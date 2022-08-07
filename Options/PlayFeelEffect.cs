using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class PlayFeelEffect : MonoBehaviour
{
   public MMFeedbacks refFeedback;

   public void PlayFeelFeedbackForArrow()
   {
      refFeedback.PlayFeedbacks();
   }
}
