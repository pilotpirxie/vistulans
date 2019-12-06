using System.Collections;
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
    private bool Selected = false;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        
    }

    private void OnMouseDown()
    {
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>().OnVertexTouch(gameObject.GetComponent<VertexController>().Id);
    }
}
