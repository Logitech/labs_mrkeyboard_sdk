using UnityEngine;

namespace MrKeyboard.Hands
{
    /// <summary>
    /// Through the callback below, this script makes sure each eye sees only the
    /// desired stereoscopic quad.
    /// </summary>
    public class KeyboardQuadCulling : MonoBehaviour
    {
        private LayerMask m_leftQuadLayer, m_rightQuadLayer;

        private void Awake()
        {
            CheckLayersAndConfigureQuads();
            ConfigureCameras();
        }

        private void ConfigureCameras()
        {
            var cameras = GameObject.FindObjectsOfType<Camera>();

            foreach(Camera c in cameras) ConfigureCamera(c);
        }

        private void ConfigureCamera(Camera camera)
        {
            if (m_leftQuadLayer == -1
                || m_rightQuadLayer == -1)
                return;

            var eye = camera.stereoTargetEye;
            switch (eye)
            {
                case StereoTargetEyeMask.Both:
                    // otherwise camera is most likley the 'head' component of SteamVR's camera rig, not an issue
                    if (camera.GetComponent<SteamVR_TrackedObject>() == null)
                        Debug.LogError("You cannot have one camera that targets both eyes for now. Please consider duplicating your camera and target each eye separately.");
                    break;
                case StereoTargetEyeMask.Right:
                    camera.cullingMask |= (1 << m_rightQuadLayer);
                    camera.cullingMask &= ~(1 << m_leftQuadLayer);
                    break;
                case StereoTargetEyeMask.Left:
                case StereoTargetEyeMask.None: // main display gets left eye
                default:
                    camera.cullingMask |= (1 << m_leftQuadLayer);
                    camera.cullingMask &= ~(1 << m_rightQuadLayer);
                    break;
            }
        }

        /// <summary>
        /// Checks that the quad layers exist and assigns them to the quads.
        /// </summary>
        private void CheckLayersAndConfigureQuads()
        {
            Transform keyboardQuadLeft, keyboardQuadRight;
            keyboardQuadLeft = transform.Find("KeyboardQuadLeft");
            keyboardQuadRight = transform.Find("KeyboardQuadRight");

            if (keyboardQuadLeft == null
                || keyboardQuadRight == null)
            {
                Debug.LogError("Cannot find KeyboardQuadLeft and Right in children, make sure they are there and named correctly.");
                return;
            }

            m_leftQuadLayer = LayerMask.NameToLayer("LeftQuad");
            m_rightQuadLayer = LayerMask.NameToLayer("RightQuad");

            if (m_leftQuadLayer != -1
                && m_rightQuadLayer != -1)
            {
                keyboardQuadLeft.gameObject.layer = m_leftQuadLayer;
                keyboardQuadRight.gameObject.layer = m_rightQuadLayer;
            }
            else
            {
                Debug.LogError("Two layers named 'LeftQuad' and 'RightQuad' must be assigned in Layer Manager. Please check the github readme for more info.");
            }
        }
    }
}
