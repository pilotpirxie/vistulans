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

    void Start()
    {
        _badgeObject = GameObject.Instantiate(BadgeObject, gameObject.transform.position - new Vector3(0, 1f, 2f), Quaternion.identity);
    }

    void FixedUpdate()
    {
        _badgeObject.GetComponent<Badge>().LevelText.GetComponent<TextMeshProUGUI>().text = $"Lv. {Level}";
        _badgeObject.GetComponent<Badge>().PowerText.GetComponent<TextMeshProUGUI>().text = $"{ArmyPower}";
        _badgeObject.GetComponent<Badge>().TypeText.GetComponent<TextMeshProUGUI>().text = $"{Type.ToString()[0]}";
    }

    private void OnMouseDown()
    {
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>().OnVertexTouch(gameObject.GetComponent<VertexController>().Id);
    }
}
