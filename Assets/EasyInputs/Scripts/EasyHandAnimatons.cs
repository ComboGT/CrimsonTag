using easyInputs;               // Gives access to EasyInputs static helpers
using UnityEngine;

public class EasyHandAnimations : MonoBehaviour
{
    [Header("Hand")]
    public EasyHand Hand;                 // Which hand (LeftHand / RightHand)

    [Header("Animator")]
    public Animator animator;             // Animator that drives the hand model
    public string Trigger_Parameter = "Trigger"; // Animator float name for trigger
    public string Grip_Parameter = "Grip";    // Animator float name for grip

    void Update()
    {
        // ----- Trigger -------------------------------------------------
        // Returns the analog trigger value (0‑1) for the selected hand.
        float triggerValue = EasyInputs.GetTriggerButtonFloat(Hand);
        if (animator != null)
            animator.SetFloat(Trigger_Parameter, triggerValue);

        // ----- Grip ----------------------------------------------------
        // Returns the analog grip/squeeze value (0‑1) for the selected hand.
        float gripValue = EasyInputs.GetGripButtonFloat(Hand);
        if (animator != null)
            animator.SetFloat(Grip_Parameter, gripValue);
    }
}