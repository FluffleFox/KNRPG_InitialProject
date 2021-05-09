using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ItemUIData", menuName = "ScriptableObjects/ItemUIData", order = 2)]
public class ItemUIData : ScriptableObject
{
    [SerializeField] private InteractableType type;
    public InteractableType Type { get { return type; } }
    [SerializeField] private Sprite itemImage;
    public Sprite ItemImage { get { return itemImage; } }
    [SerializeField] private Color backgroundColor;
    public Color BackgroundColor { get { return backgroundColor; } }
    [SerializeField] private Color textColor;
    public Color TextColor { get { return textColor; } }
    [SerializeField] private Color buttonColor;
    public Color ButtonColor { get { return buttonColor; } }
    [SerializeField] private Color buttonTextColor;
    public Color ButtonTextColor { get { return buttonTextColor; } }
    [SerializeField] private string title;
    public string Title { get { return title; } }
    [TextArea(15, 20)]
    [SerializeField] private string description;
    public string Description { get { return description; } }
    [SerializeField] private string buttonText;
    public string ButtonText { get { return buttonText; } }
}
