using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BadgeController : MonoBehaviour
{
    /// <summary>
    /// Level to display
    /// </summary>
    public int Level;

    /// <summary>
    /// Army power to display
    /// </summary>
    public int ArmyPower;

    /// <summary>
    /// Type of vertex to display
    /// </summary>
    public VertexType Type;

    /// <summary>
    /// Icon of type
    /// </summary>
    public SpriteRenderer TypeImage;

    /// <summary>
    /// Owner to display
    /// </summary>
    public OwnerType Owner;

    /// <summary>
    /// Label with level
    /// </summary>
    [SerializeField]
    TextMeshProUGUI _levelText;

    /// <summary>
    /// Label with army power 
    /// </summary>
    [SerializeField]
    TextMeshProUGUI _armyPowerText;

    /// <summary>
    /// Sprite with background image
    /// </summary>
    [SerializeField]
    SpriteRenderer _background;

    /// <summary>
    /// List of fields used as background for badge
    /// </summary>
    [SerializeField]
    List<Sprite> _sprites = new List<Sprite>();

    /// <summary>
    /// List of icon types
    /// </summary>
    [SerializeField]
    List<Sprite> _iconTypes = new List<Sprite>();

    private void FixedUpdate()
    {
        SetLabels();
        SetBackground();
        SetIcons();
    }

    /// <summary>
    /// Set badge UI labels
    /// </summary>
    void SetLabels()
    {
        if (_levelText.text != $"Lv. {Level}")
        {
            _levelText.text = $"Lv. {Level}";
        }

        switch(Level)
        {
            case 1:
                _levelText.text = $"Lv. I";
                break;
            case 2:
                _levelText.text = $"Lv. II";
                break;
            case 3:
                _levelText.text = $"Lv. III";
                break;
            case 4:
                _levelText.text = $"Lv. IV";
                break;
            case 5:
                _levelText.text = $"Lv. V";
                break;
            default:
                _levelText.text = $"Lv. {Level}";
                break;
        }

        if (_armyPowerText.text != ArmyPower.ToString())
        {
            _armyPowerText.text = ArmyPower.ToString();
        }
    }

    /// <summary>
    /// Change badge background sprite
    /// </summary>
    void SetBackground()
    {
        _background.sprite = _sprites[(int)Owner];
    }

    void SetIcons()
    {
        switch(Type)
        {
            case VertexType.Village:
                TypeImage.sprite = _iconTypes[0];
                break;
            case VertexType.Shrine:
                TypeImage.sprite = _iconTypes[1];
                break;
            case VertexType.Apiary:
                TypeImage.sprite = _iconTypes[2];
                break;
        }
    }
}
