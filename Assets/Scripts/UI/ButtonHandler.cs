using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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


    public void OnValidate()
    {
        _button = GetComponent<Button>();
        _text = _button.GetComponentInChildren<Text>();
    }
}
