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

public class Vertex : MonoBehaviour
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
    public int Power;

    [SerializeField]
    public OwnerType Owner;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        
    }
}
