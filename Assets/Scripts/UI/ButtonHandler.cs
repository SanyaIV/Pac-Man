using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// This class is used to add a ">" to the beginning of the selected button and to remove it whenever it is deselected. ">" in the game represents the current selection.
/// </summary>
[RequireComponent(typeof(Button))]
public class ButtonHandler : MonoBehaviour, ISelectHandler, IDeselectHandler {

    [Header("Constants")]
    private const string PREPEND_ON_SELECT = ">";

    [Header("Button")]
    [SerializeField] private Button _button;
    private Text _text;

    public void OnSelect(BaseEventData eventData)
    {
        if (_text.text[0] != '>')
            _text.text = _text.text.Insert(0, PREPEND_ON_SELECT);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if(_text.text[0] == '>')
            _text.text = _text.text.Remove(0, 1);
    }

    /// <summary>
    /// This method is called whenever Unity tries to validate the data and is used to get the Button and the text automatically in the editor.
    /// </summary>
    public void OnValidate()
    {
        _button = GetComponent<Button>();
        _text = _button.GetComponentInChildren<Text>();
    }
}
