using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace MrKeyboard.Keyboard
{
    /// <summary>
    /// Assigns the correct device ID to the Tracked Object script of this game object.
    /// </summary>
    [RequireComponent(typeof(SteamVR_TrackedObject))]
    public class TrackedKeyboard : MonoBehaviour
    {
        public enum KeyboardModel
        {
            K780_Proto = 0xB356
        }

        [Tooltip("The model of the keyboard to look for.")]
        public KeyboardModel keyboardModel;
        /// <summary>
        /// Use this to know if the desired keyboard could be found.
        /// </summary>
        public bool KeyboardFound { get; private set; }
        /// <summary>
        /// Contains model poses relative to each keyboard model. Is populated
        /// after this script's Awake method is executed (i.e. on Start and
        /// further calls this is guaranteed to be initialized).
        /// </summary>
        public Dictionary<KeyboardModel, Pose> KeyboardModelPose { get; private set; }

        const string logitechVid = "0x046D";

        private IEnumerator currentKbdSearch = null;
        private SteamVR_TrackedObject svrTrackedObject;

        /// <summary>
        /// Call this when changing the keyboard model at runtime.
        /// </summary>
        public void RescanDevices()
        {
            if (currentKbdSearch == null)
            {
                KeyboardFound = false;
                currentKbdSearch = LookForKeyboard(keyboardModel);
                StartCoroutine(currentKbdSearch);
            }
            else
            {
                Debug.LogWarning("Keyboard search already ongoing");
            }
        }

        private void PopulateKeyboardPoses()
        {
            KeyboardModelPose = new Dictionary<KeyboardModel, Pose>
            {
                {
                    KeyboardModel.K780_Proto,
                    new Pose(
                        new Vector3(0.0442f, -0.0002f, -0.0014f),
                        Quaternion.Euler(4.374f, 180.029f, -0.016f)
                    )
                }
            };
        }

        private void Awake()
        {
            KeyboardFound = false;
            PopulateKeyboardPoses();
            svrTrackedObject = GetComponent<SteamVR_TrackedObject>();
        }

        private void Start()
        {
            RescanDevices();
        }

        /// <summary>
        /// Lists all OpenVR devices connected, looking for a given keyboard model.
        /// Repeats the search every 2 seconds if no keyboard could be found.
        /// </summary>
        /// <param name="model"></param>
        private IEnumerator LookForKeyboard(KeyboardModel model)
        {
            int keyboardIndex;
            int retryInterval = 1;
            while (!GetFirstKeyboard(keyboardModel, out keyboardIndex))
            {
                if (retryInterval < 60)
                {
                    retryInterval *= 2;
                    MrKeyboardWatchdog.Instance.DebugText("'" + keyboardModel + "' not found, will try again in " + retryInterval + "s.");
                }
                else
                {
                    MrKeyboardWatchdog.Instance.DebugText("Keyboard takes a while to appear, you may need to restart SteamVR.");
                }

                yield return new WaitForSeconds(retryInterval);
            }
            svrTrackedObject.SetDeviceIndex(keyboardIndex);
            PositionModel(model);
            KeyboardFound = true;
            Debug.Log("Found the keyboard object.");
        }

        /// <summary>
        /// Gets the openvr device id of the first tracked device whose product id
        /// corresponds to the required keyboard model. Returns true if such a
        /// device could be found.
        /// </summary>
        /// <param name="model">The keyboard model to look for.</param>
        /// <param name="deviceId">The id of the first device found if the function
        /// returned true, -1 otherwise.</param>
        /// <returns>True if a device could be found, false otherwise.</returns>
        private bool GetFirstKeyboard(KeyboardModel model, out int deviceId)
        {
            // KeyboardModel as a 4-digit hex
            string pid = "0x" + model.ToString("X").Substring(4);

            var system = OpenVR.System;
            if (system != null)
            {
                for (uint i = 0; i < OpenVR.k_unMaxTrackedDeviceCount; i++)
                {
                    // check if a model number exists for device i
                    var error = ETrackedPropertyError.TrackedProp_Success;
                    var capacity = system.GetStringTrackedDeviceProperty(i, ETrackedDeviceProperty.Prop_ModelNumber_String, null, 0, ref error);
                    if (capacity <= 1) continue;
                    // get that model number
                    var modelNumber = new System.Text.StringBuilder((int)capacity);
                    system.GetStringTrackedDeviceProperty(i, ETrackedDeviceProperty.Prop_ModelNumber_String, modelNumber, capacity, ref error);
                    // compare against required one
                    // note: a proper regex could be used here to avoid corner cases
                    if (modelNumber.ToString().Contains(logitechVid)
                        && modelNumber.ToString().Contains(pid))
                    {
                        deviceId = (int)i;
                        return true;
                    }
                }
            }
            else
            {
                Debug.LogError("SteamVR likely not initialized, cannot look for a tracked keyboard.");
            }

            deviceId = -1;
            return false;
        }

        /// <summary>
        /// Looks for a matching child and position it accordingly.
        /// </summary>
        private void PositionModel(KeyboardModel model)
        {
            var keyboardModel = transform.Find(model.ToString("G"));
            Pose keyboardLocalPose;
            if (KeyboardModelPose.TryGetValue(model, out keyboardLocalPose))
            {
                keyboardModel.localPosition = keyboardLocalPose.position;
                keyboardModel.localRotation = keyboardLocalPose.rotation;
            }
        }
    }
}
