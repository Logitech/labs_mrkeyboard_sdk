using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

namespace MrKeyboard
{
    using Keyboard;
    using Hands;


    [RequireComponent(typeof(TrackedKeyboard), typeof(KeyboardHands), typeof(SteamVR_TrackedObject))]
    public class MrKeyboardWatchdog : MonoBehaviour
    {
        public static MrKeyboardWatchdog Instance { get; private set; }

        /// <summary>
        /// Controls whether user can see debug messages on top of the keyboard.
        /// </summary>
        public bool showDebugMessagesInVr = true;

        private TrackedKeyboard trackedKeyboard;
        private KeyboardHands keyboardHands;
        private SteamVR_TrackedObject svrTrackedKeyboard;
        private Vector3 previousKbdPosition = Vector3.zero;
        private TextMesh debugText;

        private const float INIT_TIME = 10f;
        private const float KBD_JUMP_TOLERANCE = 0.01f;
        private const int MAX_INPUT_SIZE = 710;

        public void DebugText(string text)
        {
            Debug.Log("MR Keyboard: " + text);
            if (showDebugMessagesInVr
                && debugText != null)
            {
                StartCoroutine(ShowDebugText(text, 3f));
            }
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Trying to init more than one watchdog, aborting the one on " + gameObject.name);
                return;
            }
            Instance = this;

            trackedKeyboard = GetComponent<TrackedKeyboard>();
            keyboardHands = GetComponent<KeyboardHands>();
            svrTrackedKeyboard = GetComponent<SteamVR_TrackedObject>();
            debugText = transform.Find("Keyboard messages").GetComponent<TextMesh>();
        }

        private void Start()
        {
            previousKbdPosition = transform.position;
        }

        private void Update()
        {
            // if we think we have the keyboard but steam doesn't
            if (trackedKeyboard.KeyboardFound && !SteamVR.connected[(int)svrTrackedKeyboard.index]
                && Time.time > INIT_TIME)
            {
                DebugText("Lost keyboard tracking, rescanning...");
                trackedKeyboard.RescanDevices();
            }

            // Keyboard jumped position: headset likely just lost tracking or regained it
            if ((transform.position - previousKbdPosition).magnitude > KBD_JUMP_TOLERANCE
                && Time.time > INIT_TIME)
            {
                DebugText("Likely lost hands, resetting...");
                keyboardHands.RescanDevices();
            }
            previousKbdPosition = transform.position;
        }

        private IEnumerator ShowDebugText(string text, float seconds)
        {
            UpdateTextField(text);
            yield return new WaitForSeconds(seconds);
            if (debugText.text == text)
            {
                debugText.text = "";
            }
        }

        /// <summary>
        /// This asks the text field's font about the size of each character, and
        /// puts the last n ones fitting the text field into it.
        /// </summary>
        private void UpdateTextField(string text)
        {
            var font = debugText.font;
            var size = debugText.fontSize;
            var style = debugText.fontStyle;
            CharacterInfo charInfo;
            int totalSize = 0;
            StringBuilder displayedText = new StringBuilder();

            // Traverse the string backwards until we hare too far
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                font.GetCharacterInfo(c, out charInfo, size, style);
                totalSize += charInfo.advance;
                if (totalSize <= MAX_INPUT_SIZE)
                    displayedText.Append(c);
                else
                {
                    displayedText.Remove(displayedText.Length - 3 - 1, 3);
                    displayedText.Append("...");
                    break;
                }
            }

            debugText.text = displayedText.ToString();
        }
    }
}