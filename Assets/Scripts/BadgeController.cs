using TMPro;
using UnityEngine;

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
    /// Label with vertex type
    /// </summary>
    [SerializeField]
    TextMeshProUGUI _typeText;

    /// <summary>
    /// Sprite with background image
    /// </summary>
    [SerializeField]
    SpriteRenderer _background;

    /// <summary>
    /// Prefab of image used for wild owner type
    /// </summary>
    [SerializeField]
    Sprite _wildBackground;

    /// <summary>
    /// Prefab of image used for player owner
    /// </summary>
    [SerializeField]
    Sprite _playerBackground;

    /// <summary>
    /// Prefab of image used for first enemy owner
    /// </summary>
    [SerializeField]
    Sprite _enemyOneBackground;

    /// <summary>
    /// Prefab of image used for second enemy owner
    /// </summary>
    [SerializeField]
    Sprite _enemyTwoBackground;

    /// <summary>
    /// Prefab of image used for third enemy owner
    /// </summary>
    [SerializeField]
    Sprite _enemyThreeBackground;

    private void FixedUpdate()
    {
        SetLabels();
        SetBackground();
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

        if (_armyPowerText.text != ArmyPower.ToString())
        {
            _armyPowerText.text = ArmyPower.ToString();
        }

        if (_typeText.text != Type.ToString()[0].ToString())
        {
            _typeText.text = Type.ToString()[0].ToString();
        }
    }

    /// <summary>
    /// Change badge background sprite
    /// </summary>
    void SetBackground()
    {
        switch (Owner)
        {
            case OwnerType.Wild:
                _background.sprite = _wildBackground;
                break;
            case OwnerType.Player:
                _background.sprite = _playerBackground;
                break;
            case OwnerType.EnemyOne:
                _background.sprite = _enemyOneBackground;
                break;
            case OwnerType.EnemyTwo:
                _background.sprite = _enemyTwoBackground;
                break;
            case OwnerType.EnemyThree:
                _background.sprite = _enemyThreeBackground;
                break;
        }
    }
}
