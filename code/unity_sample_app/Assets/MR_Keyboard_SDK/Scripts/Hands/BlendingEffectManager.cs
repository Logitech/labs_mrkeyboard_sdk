//---------------------------------------------------------------------------
//
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//
//---------------------------------------------------------------------------
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MrKeyboard.Hands
{
    public class BlendingEffectManager : MonoBehaviour
    {
        public Camera m_rightCamera;

        public GameObject keyboardQuadLeft, keyboardQuadRight;

        // We need two sets of quads to switch between raw passthrough and blending modes
        private GameObject m_keyboardQuadLeft;
        private GameObject m_keyboardQuadRight;

        private Material m_keyboardQuadLeftMaterial;
        private Material m_keyboardQuadRightMaterial;

        private LayerMask m_leftQuadLayer;
        private LayerMask m_rightQuadLayer;

        private List<Color> m_blendingTintColors;
        private int m_colorIndex = 0;

        // Limit of blending brightness that the use can adjust
        private const float m_minBlendBrightness = 0.0f;
        private const float m_maxBlendBrightness = 4.0f;
        private const float m_blendBrightnessAdjGranu = 0.2f;

        private const float m_minLeftRightBrightnessRatio = 0.0f;
        private const float m_maxLeftRightBrightnessRatio = 4.0f;
        private const float m_leftRightBrightnessRatioAdjGranu = 0.1f;

        private float m_currentBlendBrightness = 2.0f;
        private float m_currentLeftRightBrightnessRatio = 1.0f;

        private bool m_autoBrightnessMatching = false;

        // Use this for initialization
        void Start()
        {
            m_keyboardQuadLeft = keyboardQuadLeft;
            m_keyboardQuadRight = keyboardQuadRight;

            m_blendingTintColors = new List<Color>();

            m_blendingTintColors.Add(Color.white);
            m_blendingTintColors.Add(new Color(0.2f, 0.8f, 0.8f, 1.0f));
            m_blendingTintColors.Add(new Color(0.8f, 0.2f, 0.8f, 1.0f));

            m_keyboardQuadLeftMaterial = m_keyboardQuadLeft.GetComponent<Renderer>().material;
            m_keyboardQuadRightMaterial = m_keyboardQuadRight.GetComponent<Renderer>().material;

            m_leftQuadLayer = LayerMask.NameToLayer("LeftQuad");
            m_rightQuadLayer = LayerMask.NameToLayer("RightQuad");

            m_currentBlendBrightness = PlayerPrefs.GetFloat("BlendBrightness", 2.0f);
            m_currentLeftRightBrightnessRatio = PlayerPrefs.GetFloat("LeftRightBrightnessRatio", 1.0f);

            UpdateQuadBrightness();
        }

        public void ToggleMonoOrStereoCamera()
        {
            m_rightCamera.cullingMask ^= (1 << m_leftQuadLayer);
            m_rightCamera.cullingMask ^= (1 << m_rightQuadLayer);
        }

        public void ChangeBlendingTintColor()
        {
            m_colorIndex = (m_colorIndex + 1) % m_blendingTintColors.Count;
            m_keyboardQuadLeftMaterial.SetColor("_TintColor", m_blendingTintColors[m_colorIndex]);
            m_keyboardQuadRightMaterial.SetColor("_TintColor", m_blendingTintColors[m_colorIndex]);
        }

        // Change blending brightness
        public void DecreaseBlendBrightness()
        {
            m_currentBlendBrightness = Mathf.Max(m_minBlendBrightness, m_currentBlendBrightness - m_blendBrightnessAdjGranu);
            UpdateQuadBrightness();
            PlayerPrefs.SetFloat("BlendBrightness", m_currentBlendBrightness);
        }
        public void IncreaseBlendBrightness()
        {
            m_currentBlendBrightness = Mathf.Min(m_maxBlendBrightness, m_currentBlendBrightness + m_blendBrightnessAdjGranu);
            UpdateQuadBrightness();
            PlayerPrefs.SetFloat("BlendBrightness", m_currentBlendBrightness);
        }
        public void DecreaseLeftRightBrightnessRatio()
        {
            m_currentLeftRightBrightnessRatio = Mathf.Max(m_minLeftRightBrightnessRatio, m_currentLeftRightBrightnessRatio - m_leftRightBrightnessRatioAdjGranu);
            UpdateQuadBrightness();
            PlayerPrefs.SetFloat("LeftRightBrightnessRatio", m_currentLeftRightBrightnessRatio);
        }
        public void IncreaseLeftRightBrightnessRatio()
        {
            m_currentLeftRightBrightnessRatio = Mathf.Min(m_maxLeftRightBrightnessRatio, m_currentLeftRightBrightnessRatio + m_leftRightBrightnessRatioAdjGranu);
            UpdateQuadBrightness();
            PlayerPrefs.SetFloat("LeftRightBrightnessRatio", m_currentLeftRightBrightnessRatio);
        }
        private void UpdateQuadBrightness()
        {
            float leftBlendBrightness = m_currentBlendBrightness;
            float rightBlendBrightness = m_autoBrightnessMatching ? m_currentBlendBrightness : m_currentBlendBrightness * m_currentLeftRightBrightnessRatio;

            m_keyboardQuadLeftMaterial.SetFloat("_Brightness", leftBlendBrightness);
            m_keyboardQuadRightMaterial.SetFloat("_Brightness", rightBlendBrightness);
        }
    }
}
