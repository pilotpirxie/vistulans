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

    void Start()
    {
        _badgeObject = GameObject.Instantiate(BadgeObject, gameObject.transform.position - new Vector3(0, 1f, 2f), Quaternion.identity);

        if (_mechanismObject == null)
        {
            _mechanismObject = GameObject.Find("Mechanism");
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
