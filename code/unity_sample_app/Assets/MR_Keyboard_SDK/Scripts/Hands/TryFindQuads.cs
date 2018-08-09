using UnityEngine;

namespace MrKeyboard.Hands
{
    /// <summary>
    /// Assigns the first quads likely to be recipients of the hands images to
    /// the passthrough and blending effect managers.
    /// </summary>
    [RequireComponent(typeof(PassthroughPlugin), typeof(BlendingEffectManager))]
    public class TryFindQuads : MonoBehaviour
    {
        private void Awake()
        {
            var passthrough = GetComponent<PassthroughPlugin>();
            var blending = GetComponent<BlendingEffectManager>();

            if (passthrough.keyboardQuadLeft != null
                || passthrough.keyboardQuadRight != null
                || blending.keyboardQuadLeft != null
                || blending.keyboardQuadRight != null)
            {
                Debug.Log("It looks like you have already assigned the keyboard quads, I won't do anything.");
                return;
            }

            var quadLeft = transform.parent.Find("KeyboardQuadLeft").gameObject;
            var quadRight = transform.parent.Find("KeyboardQuadRight").gameObject;

            if (quadLeft == null
                || quadRight == null)
            {
                Debug.LogError("Cannot find the keyboard quads. Do you have a tracked keyboard in your scene?");
                return;
            }

            quadLeft.SetActive(true);
            quadRight.SetActive(true);

            passthrough.keyboardQuadLeft = quadLeft;
            passthrough.keyboardQuadRight = quadRight;
            blending.keyboardQuadLeft = quadLeft;
            blending.keyboardQuadRight = quadRight;
        }
    }
}
