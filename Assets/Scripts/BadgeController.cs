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
    public TextMeshProUGUI LevelText;

    /// <summary>
    /// Label with army power 
    /// </summary>
    public TextMeshProUGUI ArmyPowerText;

    /// <summary>
    /// Label with vertex type
    /// </summary>
    public TextMeshProUGUI TypeText;

    /// <summary>
    /// Sprite with background image
    /// </summary>
    public SpriteRenderer Background;

    /// <summary>
    /// Prefab of image used for wild owner type
    /// </summary>
    public Sprite WildBackground;

    /// <summary>
    /// Prefab of image used for player owner
    /// </summary>
    public Sprite PlayerBackground;

    /// <summary>
    /// Prefab of image used for first enemy owner
    /// </summary>
    public Sprite EnemyOneBackground;

    /// <summary>
    /// Prefab of image used for second enemy owner
    /// </summary>
    public Sprite EnemyTwoBackground;

    /// <summary>
    /// Prefab of image used for third enemy owner
    /// </summary>
    public Sprite EnemyThreeBackground;

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
        if (TypeText.text != $"Lv. {Level}")
        {
            TypeText.text = $"Lv. {Level}";
        }

        if (ArmyPowerText.text != ArmyPower.ToString())
        {
            ArmyPowerText.text = ArmyPower.ToString();
        }

        if (TypeText.text != Type.ToString()[0].ToString())
        {
            TypeText.text = Type.ToString()[0].ToString();
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
                Background.sprite = WildBackground;
                break;
            case OwnerType.Player:
                Background.sprite = PlayerBackground;
                break;
            case OwnerType.EnemyOne:
                Background.sprite = EnemyOneBackground;
                break;
            case OwnerType.EnemyTwo:
                Background.sprite = EnemyTwoBackground;
                break;
            case OwnerType.EnemyThree:
                Background.sprite = EnemyThreeBackground;
                break;
        }
    }
}
