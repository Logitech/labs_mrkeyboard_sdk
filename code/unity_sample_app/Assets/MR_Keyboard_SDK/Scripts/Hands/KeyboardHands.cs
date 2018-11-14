using System.Collections;
using UnityEngine;
using MrKeyboard.Keyboard;

namespace MrKeyboard.Hands
{
    /// <summary>
    /// Takes care of setting up the passthrough plugin.
    /// </summary>
    [RequireComponent(typeof(TrackedKeyboard))]
    public class KeyboardHands : MonoBehaviour
    {
        public PassthroughPlugin passthroughPlugin;

        private Transform m_keyboardQuadLeft, m_keyboardQuadRight;
        private QuadTracker m_quadTracker;
        private IEnumerator currentKbdSearch = null;

        /// <summary>
        /// Call this when changing the keyboard model at runtime.
        /// </summary>
        public void RescanDevices()
        {
            passthroughPlugin.Reset();
            if (currentKbdSearch != null)
                StopCoroutine(currentKbdSearch);

            currentKbdSearch = SetKeyboardTypeAfterFound();
            StartCoroutine(currentKbdSearch);
        }

        private void Awake()
        {
            m_keyboardQuadLeft = transform.Find("KeyboardQuadLeft");
            m_keyboardQuadRight = transform.Find("KeyboardQuadRight");

            if (passthroughPlugin == null)
                passthroughPlugin = GetComponentInChildren<PassthroughPlugin>();
            if (passthroughPlugin == null)
                Debug.LogError("PassthroughPlugin was not assigned and cannot be found in children, please fix.");
        }

        private void Start()
        {
            if (SteamVR.initializing) Debug.LogWarning("System initializing when starting");
            RescanDevices();
        }

        private IEnumerator SetKeyboardTypeAfterFound()
        {
            yield return new WaitForEndOfFrame();
            var trackedKeyboard = GetComponent<TrackedKeyboard>();

            while (!trackedKeyboard.KeyboardFound)
            {
                MrKeyboardWatchdog.Instance.DebugText("Waiting for tracked keyboard to acquire video feed.");
                yield return new WaitForSeconds(5f);
            }

            // submit the correct keyboard type to the DLL
            var model = trackedKeyboard.keyboardModel;
            string dllModelName;
            switch (model)
            {
                case TrackedKeyboard.KeyboardModel.K780_Proto:
                    dllModelName = "Logi_K780_Proto1";
                    break;
                default:
                    Debug.LogWarningFormat("Keyboard model {0} is likely not supported by the passthrough DLL.", model);
                    dllModelName = model.ToString("G");
                    break;
            }

            bool success = passthroughPlugin.SetKeyboardTypeInDll(dllModelName);

            if (!success)
            {
                Debug.LogErrorFormat("Cannot set the keyboard type {0} in the DLL.", model);
            }
            else
            {
                // position que quads
                m_quadTracker = new QuadTracker();
                m_quadTracker.SetObjectCornersInObjectD3d(passthroughPlugin.GetKbCornersInKbD3dFromDll());
                PositionHandQuads();
            }
        }

        private void PositionHandQuads()
        {
            // get the quad offset as computed by the quad tracker
            Vector3 quadPosition = new Vector3();
            Quaternion quadOrientation = new Quaternion();
            Vector3 quadScale = new Vector3();

            m_quadTracker.GetQuad(
                out quadPosition,
                out quadOrientation,
                out quadScale,
                transform.position,
                transform.rotation);

            // the offset between constellation tracking and actual keyboard pose
            // (will later be integrated into the DLL itself)
            Pose localOffset = new Pose(
                    new Vector3(0.0446f, 0.0026f, -0.0837f),
                    Quaternion.Euler(85.44901f, 1.479f, 1.448f)
                );

            // apply this to each quad
            m_keyboardQuadLeft.position = quadPosition;
            m_keyboardQuadLeft.rotation = quadOrientation;
            m_keyboardQuadLeft.localScale = quadScale;
            m_keyboardQuadLeft.localPosition = localOffset.position;
            m_keyboardQuadLeft.localRotation = localOffset.rotation;

            m_keyboardQuadRight.position = quadPosition;
            m_keyboardQuadRight.rotation = quadOrientation;
            m_keyboardQuadRight.localScale = quadScale;
            m_keyboardQuadRight.localPosition = localOffset.position;
            m_keyboardQuadRight.localRotation = localOffset.rotation;
        }
    }
}
