using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum VertexType
{
    Village,
    Shrine,
    Apiary
};

public enum OwnerType
{
    Wild,
    Player,
    EnemyOne,
    EnemyTwo,
    EnemyThree
};

public class VertexController : MonoBehaviour
{

    public GameObject BadgeObject;
    private GameObject _badgeObject;
    private GameObject _mechanismObject;

    [SerializeField]
    public int Id;

    [SerializeField]
    public VertexType Type;

    [SerializeField]
    public int X;

    [SerializeField]
    public int Y;

    [SerializeField]
    public int Level;

    [SerializeField]
    public List<GameObject> Connections;

    [SerializeField]
    public int ArmyPower;

    [SerializeField]
    public OwnerType Owner;

    [SerializeField]
    private bool _selected = false;

    private byte _timer = 0;

    void Start()
    {
        _badgeObject = GameObject.Instantiate(BadgeObject, gameObject.transform.position - new Vector3(0, 1f, 2f), Quaternion.identity);

        InvokeRepeating("IncreaseUnits", 2.0f, 2.0f);

        if (_mechanismObject == null)
        {
            _mechanismObject = GameObject.Find("Mechanism");
        }
    }

    void IncreaseUnits()
    {
        switch (Type)
        {
            case VertexType.Shrine:
                _mechanismObject.GetComponent<GameplayController>().Mana += Level + 1;
                break;
            case VertexType.Village:
                ArmyPower += Level + 1;
                break;
            case VertexType.Apiary:
                _mechanismObject.GetComponent<GameplayController>().Honey += Level + 1;
                break;
        }
    }

    void FixedUpdate()
    {
        _badgeObject.GetComponent<Badge>().LevelText.GetComponent<TextMeshProUGUI>().text = $"Lv. {Level}";
        _badgeObject.GetComponent<Badge>().PowerText.GetComponent<TextMeshProUGUI>().text = $"{ArmyPower}";
        _badgeObject.GetComponent<Badge>().TypeText.GetComponent<TextMeshProUGUI>().text = $"{Type.ToString()[0]}";
    }

    private void OnMouseDown()
    {
        _mechanismObject.GetComponent<GraphController>().OnVertexTouch(gameObject.GetComponent<VertexController>().Id);
    }
}
