using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

[RequireComponent(typeof(TMP_Text))]
public class AnyInput : MonoBehaviour
{
    [Serializable]
    public class OnAnyInputEvent : UnityEvent { }
    
    public TMP_Text anyInputText;
    public OnAnyInputEvent OnAnyInput;

    private void Awake()
    {
        if(!anyInputText)
            anyInputText = GetComponent<TMP_Text>();
        if(anyInputText)
        {
            if (Input.touchSupported)
                anyInputText.text = "Tab to start";
            else
                anyInputText.text = "Press any input";
        }
        InputSystem.onAnyButtonPress.CallOnce(ctrl => InvokeOnAnyInput());
    }

    private void InvokeOnAnyInput()
    {
        OnAnyInput?.Invoke();
    }
}
