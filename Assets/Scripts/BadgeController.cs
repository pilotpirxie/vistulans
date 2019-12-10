using TMPro;
using UnityEngine;

public class BadgeController : MonoBehaviour
{
    public int Level;
    public int ArmyPower;
    public VertexType Type;
    public OwnerType Owner;

    public GameObject LevelText;
    public GameObject PowerText;
    public GameObject TypeText;
    public GameObject Background;

    public Sprite WildBackground;
    public Sprite PlayerBackground;
    public Sprite EnemyOneBackground;
    public Sprite EnemyTwoBackground;
    public Sprite EnemyThreeBackground;

    private void FixedUpdate()
    {
        if (LevelText.GetComponent<TextMeshProUGUI>().text != $"Lv. {Level}")
        {
            LevelText.GetComponent<TextMeshProUGUI>().text = $"Lv. {Level}";
        }

        if (LevelText.GetComponent<TextMeshProUGUI>().text != $"{ArmyPower}")
        {
            PowerText.GetComponent<TextMeshProUGUI>().text = $"{ArmyPower}";
        }

        if (LevelText.GetComponent<TextMeshProUGUI>().text != $"{Type.ToString()[0]}")
        {
            TypeText.GetComponent<TextMeshProUGUI>().text = $"{Type.ToString()[0]}";
        }

        switch (Owner)
        {
            case OwnerType.Wild:
                Background.GetComponent<SpriteRenderer>().sprite = WildBackground;
                break;
            case OwnerType.Player:
                Background.GetComponent<SpriteRenderer>().sprite = PlayerBackground;
                break;

            case OwnerType.EnemyOne:
                Background.GetComponent<SpriteRenderer>().sprite = EnemyOneBackground;
                break;

            case OwnerType.EnemyTwo:
                Background.GetComponent<SpriteRenderer>().sprite = EnemyTwoBackground;
                break;

            case OwnerType.EnemyThree:
                Background.GetComponent<SpriteRenderer>().sprite = EnemyThreeBackground;
                break;
        }
    }
}
