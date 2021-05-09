using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ItemUIPlaceHolder : MonoBehaviour
{
    public enum UIType
    {
        BACKGROUND,
        TITLE,
        IMAGE,
        DESCRIPTION,
        BUTTON,
        BUTTONTEXT
    }
    [System.Serializable]    
    public class UIComponent
    {
        public UIType type;
        public GameObject component;
    }
    [SerializeField]
    [ArrayElementTitle("type")]
    private UIComponent[] components;
    public UIComponent[] Components { get { return components; } }
}
