using System.Collections.Generic;
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

    public int Id;

    public VertexType Type;

    public int X;

    public int Y;

    public int Level;

    public List<GameObject> Connections;

    public int ArmyPower;

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
        if (Owner != OwnerType.Wild)
        {
            switch (Type)
            {
                case VertexType.Shrine:
                    _mechanismObject.GetComponent<GameplayController>().Mana[(int)Owner - 1] += Level;
                    break;
                case VertexType.Village:
                    ArmyPower += Level + 1;
                    break;
                case VertexType.Apiary:
                    _mechanismObject.GetComponent<GameplayController>().Honey[(int)Owner - 1] += Level;
                    break;
            }
        }
    }

    void FixedUpdate()
    {
        _badgeObject.GetComponent<BadgeController>().Level = Level;
        _badgeObject.GetComponent<BadgeController>().ArmyPower = ArmyPower;
        _badgeObject.GetComponent<BadgeController>().Type = Type;
        _badgeObject.GetComponent<BadgeController>().Owner = Owner;
    }

    private void OnMouseDown()
    {
        _mechanismObject.GetComponent<GraphController>().OnVertexTouch(gameObject.GetComponent<VertexController>().Id);
    }
}
