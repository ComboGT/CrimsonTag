// ------------------------------------------------------------
// EasyInputs.cs – uses OpenXR Common Usages
// ------------------------------------------------------------
namespace easyInputs
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.XR;

    public static partial class EasyInputs
    {
        // --------------------------------------------------------
        // Boolean helpers
        // --------------------------------------------------------
        /// <summary>
        /// Returns true while the **Squeeze (Grip) button** is pressed.
        /// </summary>
        public static bool GetGripButtonDown(EasyHand easyHand)
        {
            GetInputBool(easyHand, CommonUsages.gripButton, out bool buttonDown);
            return buttonDown;
        }

        /// <summary>
        /// Returns **true** while the thumb‑stick click (press‑down) is active.
        /// </summary>
        public static bool GetThumbStickButtonDown(EasyHand easyHand)
        {
            // Uses the OpenXR common usage for the thumb‑stick click.
            GetInputBool(easyHand, CommonUsages.primary2DAxisClick, out bool buttonDown);
            return buttonDown;
        }

        /// <summary>
        /// Returns **true** while the primary button (A on the right controller, X on the left) is pressed.
        /// </summary>
        public static bool GetPrimaryButtonDown(EasyHand easyHand)
        {
            // Uses the OpenXR common usage for the primary button.
            GetInputBool(easyHand, CommonUsages.primaryButton, out bool buttonDown);
            return buttonDown;
        }

        /// <summary>
        /// Returns true while the **Trigger button** is pressed.
        /// </summary>
        public static bool GetTriggerButtonDown(EasyHand easyHand)
        {
            GetInputBool(easyHand, CommonUsages.triggerButton, out bool buttonDown);
            return buttonDown;
        }

        /// <summary>
        /// Returns true while the trigger surface is being touched.
        /// The method emulates a trigger‑touch by reading the analog trigger value
        /// (CommonUsages.trigger) and applying a tiny threshold internally.
        /// </summary>
        public static bool GetTriggerButtonTouched(EasyHand easyHand)
        {
            // 1️⃣ Get the analog trigger axis (0 = released, 1 = fully pressed)
            //    This uses the existing helper that reads a float from CommonUsages.trigger.
            float triggerValue = GetTriggerButtonFloat(easyHand);   // see GetTriggerButtonFloat in EasyInputs.cs

            // 2️⃣ Define a small threshold that we consider a “touch”.
            //    A value of 0.02 works well for most controllers; adjust if needed.
            const float TouchThreshold = 0.02f;

            // 3️⃣ Return true when the trigger value exceeds the threshold.
            return triggerValue > TouchThreshold;
        }

        /// <summary>
        /// Returns true while the **Thumb‑stick button** is touched.
        /// </summary>
        public static bool GetThumbStickButtonTouched(EasyHand easyHand)
        {
            GetInputBool(easyHand, CommonUsages.primary2DAxisTouch, out bool buttonDown);
            return buttonDown;
        }

        // --------------------------------------------------------
        // Float / Axis helpers (the ones you were missing)
        // --------------------------------------------------------
        /// <summary>
        /// Get's the analog **trigger** value (0‑1).
        /// </summary>
        public static float GetTriggerButtonFloat(EasyHand easyHand)   // <-- added
        {
            GetInputFloat(easyHand, CommonUsages.trigger, out float triggerFloat);
            return triggerFloat;
        }

        /// <summary>
        /// Get's the analog **grip / squeeze** value (0‑1).
        /// </summary>
        public static float GetGripButtonFloat(EasyHand easyHand)    // <-- added
        {
            GetInputFloat(easyHand, CommonUsages.grip, out float gripFloat);
            return gripFloat;
        }

        /// <summary>
        /// Returns the 2‑D thumb‑stick axis.
        /// </summary>
        public static Vector2 GetThumbStick2DAxis(EasyHand easyHand)
        {
            GetInputVector2(easyHand, CommonUsages.primary2DAxis, out Vector2 axis);
            return axis;
        }

        /// <summary>
        /// Returns the device’s linear velocity vector.
        /// </summary>
        public static Vector3 GetDeviceVelocity(EasyHand easyHand)
        {
            GetInputVector3(easyHand, CommonUsages.deviceVelocity, out Vector3 velocity);
            return velocity;
        }

        // --------------------------------------------------------
        // Low‑level fetchers
        // --------------------------------------------------------
        static bool GetInputBool(EasyHand hand, InputFeatureUsage<bool> usage, out bool value)
        {
            if (hand == EasyHand.LeftHand)
                return InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(usage, out value);
            else
                return InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(usage, out value);
        }

        static void GetInputFloat(EasyHand hand, InputFeatureUsage<float> usage, out float value)   // <-- needed by the two float methods
        {
            if (hand == EasyHand.LeftHand)
                InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(usage, out value);
            else
                InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(usage, out value);
        }

        static void GetInputVector2(EasyHand hand, InputFeatureUsage<Vector2> usage, out Vector2 value)
        {
            if (hand == EasyHand.LeftHand)
                InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(usage, out value);
            else
                InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(usage, out value);
        }

        static void GetInputVector3(EasyHand hand, InputFeatureUsage<Vector3> usage, out Vector3 value)
        {
            if (hand == EasyHand.LeftHand)
                InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(usage, out value);
            else
                InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(usage, out value);
        }

        // --------------------------------------------------------
        // Haptic helper (unchanged)
        // --------------------------------------------------------
        public static IEnumerator Vibration(EasyHand hand, float amplitude, float duration)
        {
            float startTime = Time.time;
            uint channel = 0U;
            InputDevice device = (hand == EasyHand.LeftHand)
                ? InputDevices.GetDeviceAtXRNode(XRNode.LeftHand)
                : InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

            while (Time.time < startTime + duration)
            {
                device.SendHapticImpulse(channel, amplitude, duration);
                yield return new WaitForSeconds(duration * 0.9f);
            }
        }
    }

    // ------------------------------------------------------------
    // Hand enum – unchanged
    // ------------------------------------------------------------
    public enum EasyHand
    {
        LeftHand,
        RightHand
    }
}