using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MrKeyboard.Keyboard
{
    public class KeyboardAnimator : MonoBehaviour
    {
        /// <summary>
        /// Call UpdateMaterial() after changing this to reflect changes on the keyboard.
        /// </summary>
        public Material regularMaterial, floatingKeysMaterial;
        /// <summary>
        /// No need to call UpdateMaterial() after changing this one.
        /// </summary>
        public Material pressedMaterial;
        /// <summary>
        /// Indicates whether the keyboard is in "floating keys" mode or not.
        /// </summary>
        public bool IsBodyVisible { get; private set; }

        private Dictionary<int, Transform> m_keyMap;
        private Dictionary<KeyCode, int> m_keyCodeMap;
        private HashSet<Transform> m_movedKeys = new HashSet<Transform>();
        private Material m_unpressed;

        private Dictionary<Transform, Pose> m_keyLocalRestPose;
        private const float KEY_DISPLACEMENT_OFFSET = -0.002f;

        /// <summary>
        /// Changes the type of keyboard from a regular to a "floating keys" one.
        /// </summary>
        public void ToggleBodyVisibility()
        {
            var body = transform.Find("mainBody").GetComponent<MeshRenderer>();
            var constellation = transform.Find("constellation").GetComponent<MeshRenderer>();

            body.enabled = !body.enabled;
            constellation.enabled = !constellation.enabled;

            // changing this bool and calling update material will re-skin the kbd
            IsBodyVisible = body.enabled;
            UpdateMaterial();
        }

        /// <summary>
        /// Call this after changing one of the public materials to make sure
        /// the new material immediately reflects on the keyboard.
        /// </summary>
        public void UpdateMaterial()
        {
            if (IsBodyVisible)
                m_unpressed = regularMaterial;
            else
                m_unpressed = floatingKeysMaterial;

            Renderer[] kbdParts = GetComponentsInChildren<Renderer>();
            foreach (Renderer r in kbdParts)
                r.material = m_unpressed;
        }

        private void Start()
        {
            IsBodyVisible = true;
            UpdateMaterial();

            PopulateKeyCodeMap(Application.streamingAssetsPath + "\\unityToVkCodes.csv");
            PopulateKeyMap();
        }

        /// <summary>
        /// Creates a map from a vkCode to the Transform representing that key.
        /// Also populates the default pose map of each key.
        /// </summary>
        private void PopulateKeyMap()
        {
            var children = GetComponentsInChildren<Transform>();
            m_keyMap = new Dictionary<int, Transform>(children.Length);
            m_keyLocalRestPose = new Dictionary<Transform, Pose>(children.Length);
            foreach (Transform child in children)
            {
                if (child.name.StartsWith("vk"))
                {
                    int keycode = 0;
                    if (int.TryParse(child.name.Substring(2), out keycode))
                    {
                        m_keyMap.Add(keycode, child);
                        m_keyLocalRestPose.Add(child, new Pose(child.localPosition, child.localRotation));
                    }
                }
            }
        }

        /// <summary>
        /// Creates a map from Unity KeyCodes to vkCodes (i.e. name of the groups
        /// within the keyboard model).
        /// </summary>
        /// <param name="csvFile">The file containing the mapping</param>
        private void PopulateKeyCodeMap(string csvFile)
        {
            var lines = File.ReadAllLines(csvFile);
            m_keyCodeMap = new Dictionary<KeyCode, int>(lines.Length);
            foreach (string line in lines)
            {
                var items = line.Split(',');
                int unityCode = int.Parse(items[0]);
                int vkCode = int.Parse(items[1]);
                m_keyCodeMap.Add((KeyCode)unityCode, vkCode);
            }
        }

        private void OnApplicationFocus(bool focus)
        {
            if (focus)
            {
                foreach (Transform key in m_movedKeys)
                {
                    // cannot use AnimateKey(key, false) here since it modifies the hash set within the foreach loop
                    key.localPosition = m_keyLocalRestPose[key].position;
                    key.GetComponent<Renderer>().material = m_unpressed;
                }
                m_movedKeys.Clear();
            }
        }

        private void Update()
        {
            foreach (KeyCode c in System.Enum.GetValues(typeof(KeyCode)))
            {
                // For whatever reason, left Win is left cmd, and twice. So to avoid troubles, ignore it.
                // Also, AltGr and RightAlt are fired at the same time, so let's just ignore one.
                if (c == KeyCode.LeftCommand
                    || c == KeyCode.RightAlt)
                    continue;

                if (Input.GetKeyDown(c))
                {
                    Transform key = TryGetTransform(c);
                    AnimateKey(key, true);
                }
                else if (Input.GetKeyUp(c))
                {
                    Transform key = TryGetTransform(c);
                    AnimateKey(key, false);
                }
            }
        }

        private Transform TryGetTransform(KeyCode c)
        {
            if (c.ToString().StartsWith("Mouse")) return null;
            if (c.ToString().StartsWith("Joystick")) return null;

            int vkCode = 0;
            Transform kbdKey = null;
            if (m_keyCodeMap.TryGetValue(c, out vkCode))
                if (m_keyMap.TryGetValue(vkCode, out kbdKey))
                    return kbdKey;
                else
                    Debug.LogWarning("Could not find transform tied to " + c + ", whose vkCode should be " + vkCode);
            else
                Debug.LogWarning("Could not find vkCode of KeyCode " + c);

            return null;
        }

        private void AnimateKey(Transform key, bool pressed)
        {
            if (key == null) return;

            if (pressed)
            {
                key.Translate(0f, KEY_DISPLACEMENT_OFFSET, 0f);
                key.GetComponent<Renderer>().material = pressedMaterial;
                m_movedKeys.Add(key);
            }
            else
            {
                key.localPosition = m_keyLocalRestPose[key].position;
                key.GetComponent<Renderer>().material = m_unpressed;
                m_movedKeys.Remove(key);
            }
        }
    }
}