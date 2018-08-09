//---------------------------------------------------------------------------
//
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//
//---------------------------------------------------------------------------
using UnityEngine;
using System;
using System.Runtime.InteropServices;


namespace MrKeyboard.Hands
{
    public enum AppType { Uwp = 0, Win32, SteamVr };

    public class PassthroughPlugin : MonoBehaviour
    {
        public GameObject keyboardQuadLeft, keyboardQuadRight;

        // We need two sets of quads to switch between raw passthrough and blending modes
        private GameObject m_keyboardQuadLeft;
        private GameObject m_keyboardQuadRight;

        private Texture2D m_keyboardTexLeft;
        private Texture2D m_keyboardTexRight;

        [DllImport("Burke")]
        private static extern void BurkeInitialize();

        [DllImport("Burke")]
        private static extern void BurkeUnInitialize();

        [DllImport("Burke")]
        private static extern bool BurkeSetAppType(int appType, string windowClassName);

        // We'll also pass native pointer to a texture in Unity.
        // The plugin will fill texture data from native code.
        [DllImport("Burke")]
        private static extern void BurkeSetKeyboardHandPresenceStereoTextures(
            IntPtr keyboardTextureLeft,
            IntPtr keyboardTextureRight);

        [DllImport("Burke")]
        private static extern IntPtr GetUnityRenderEventFunc();

        [DllImport("Burke")]
        private static extern bool BurkeUpdateTextures();

        [DllImport("Burke")]
        private static extern bool BurkeSetKeyboardType(string kbType);

        [DllImport("Burke")]
        private static extern void BurkeGetHandPresenceTextureLocationInKeyboardCoord(
            IntPtr pTopLeftCorner,
            IntPtr pTopRightCorner,
            IntPtr pBottomLeftCorner,
            IntPtr pBottomRightCorner);


        void Start()
        {
            m_keyboardQuadLeft = keyboardQuadLeft;
            m_keyboardQuadRight = keyboardQuadRight;

            CreateTextureAndPassToPlugin();

#if UNITY_WSA
        BurkeSetAppType((int)AppType.Uwp, null);
#elif UNITY_EDITOR
            //BurkeSetAppType((int)AppType.Win32, "UnityHoloInEditorWndClass"); // use this when in the editor without steamvr
            BurkeSetAppType((int)AppType.SteamVr, null);
#else
        BurkeSetAppType((int)AppType.SteamVr, null);
#endif
        }

        void LateUpdate()
        {
            // Issue a plugin event with arbitrary integer identifier.
            // The plugin can distinguish between different
            // things it needs to do based on this ID.
            // For our plugin, it does not matter which ID we pass here.
            GL.IssuePluginEvent(GetUnityRenderEventFunc(), 1);
        }

        private void CreateTextureAndPassToPlugin()
        {
            // Create a texture. Call Apply() so it's actually uploaded to the GPU
            const int textureHeight = 480;
            const int textureWidth = 640;
            const TextureFormat textureFormat = TextureFormat.RGBA32;

            m_keyboardTexLeft = new Texture2D(textureWidth, textureHeight, textureFormat, false);
            m_keyboardTexLeft.filterMode = FilterMode.Bilinear;
            m_keyboardTexLeft.Apply();

            m_keyboardTexRight = new Texture2D(textureWidth, textureHeight, textureFormat, false);
            m_keyboardTexRight.filterMode = FilterMode.Bilinear;
            m_keyboardTexRight.Apply();

            // Set texture onto our material
            m_keyboardQuadLeft.GetComponent<Renderer>().material.mainTexture = m_keyboardTexLeft;
            m_keyboardQuadRight.GetComponent<Renderer>().material.mainTexture = m_keyboardTexRight;

            // Pass texture pointer to the plugin
            BurkeSetKeyboardHandPresenceStereoTextures(
                m_keyboardTexLeft.GetNativeTexturePtr(),
                m_keyboardTexRight.GetNativeTexturePtr());
        }


        public bool SetKeyboardTypeInDll(string kbType)
        {
            return BurkeSetKeyboardType(kbType);
        }

        public Vector4[] GetKbCornersInKbD3dFromDll()
        {
            Vector4[] kbCornersInKbD3d = new Vector4[4];
            float[][] nums = new float[4][];

            // Get address of the four Vector4 and pass the pointers to dll
            GCHandle[] handle = new GCHandle[4];
            for (int i = 0; i < 4; ++i)
            {
                nums[i] = new float[4];
                handle[i] = GCHandle.Alloc(nums[i], GCHandleType.Pinned);
            }

            try
            {
                System.IntPtr[] ptr = new IntPtr[4];
                for (int i = 0; i < 4; ++i)
                {
                    ptr[i] = handle[i].AddrOfPinnedObject();
                }

                BurkeGetHandPresenceTextureLocationInKeyboardCoord(
                    ptr[0], ptr[1], ptr[2], ptr[3]);

                for (int i = 0; i < 4; ++i)
                {
                    kbCornersInKbD3d[i] = new Vector4(nums[i][0], nums[i][1], nums[i][2], nums[i][3]);
                }
            }
            finally
            {
                for (int i = 0; i < 4; ++i)
                {
                    if (handle[i].IsAllocated)
                    {
                        handle[i].Free();
                    }
                }
            }

            return kbCornersInKbD3d;
        }
    }
}
