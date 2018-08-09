using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MrKeyboard.Keyboard;

/// <summary>
/// The goal here is to illustrate how one can change the keyboard material
/// programmatically.
/// Note that here we could do it more easily trough the 'regularMaterial' of
/// the keyboard animator directly in Unity's interface, since we only do it
/// once at startup.
/// </summary>
public class DynamicSkinChange : MonoBehaviour
{
    public KeyboardAnimator keyboardAnimator;
    public Material customKeyboardSkin;

	private void Start () {
        // Just check that all variables have been assigned correctly
        if (keyboardAnimator == null
            || customKeyboardSkin == null)
        {
            Debug.LogError("Make sure to have assigned all variables!");
            return;
        }

        // Change the skin in the animator and let it know it has changed
        keyboardAnimator.regularMaterial = customKeyboardSkin;
        keyboardAnimator.UpdateMaterial();
    }
}
