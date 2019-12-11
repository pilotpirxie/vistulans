using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    /// <summary>
    /// Indexed as follow:
    /// 0 - player
    /// 1 - enemy one
    /// 2 - enemy two
    /// 3 - enemy three
    /// </summary>
    public int[] Mana;
    public int[] Honey;
    public int[] Army;

    [SerializeField]
    private List<VertexController> _vertexList;

    public VertexController SelectedVertexA;

    public VertexController SelectedVertexB;

    void Start()
    {
        Mana = new int[] { 0, 0, 0, 0 };
        Honey = new int[] { 0, 0, 0, 0 }; 
        Army = new int[] { 0, 0, 0, 0 };

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Vertex"))
        {
            _vertexList.Add(obj.GetComponent<VertexController>());
        }

        InvokeRepeating("IncreaseUnits", 2.0f, 2.0f);
    }

    void IncreaseUnits()
    {
        for (int i = 0; i < Army.Length; i++)
        {
            Army[i] = 0;
        }

        foreach (VertexController vertex in _vertexList)
        {
            if (vertex.Owner != OwnerType.Wild)
            {
                switch (vertex.Type)
                {
                    case VertexType.Shrine:
                        Mana[(int)vertex.Owner - 1] += vertex.Level;
                        break;
                    case VertexType.Village:
                        vertex.ArmyPower += vertex.Level;
                        Army[(int)vertex.Owner - 1] += vertex.ArmyPower;
                        break;
                    case VertexType.Apiary:
                        Honey[(int)vertex.Owner - 1] += vertex.Level;
                        break;
                }
            }
        }
    }

    public void UpgradeVertex(int vertexId)
    {
        if (SelectedVertexA.Level < 5 && Honey[0] >= SelectedVertexA.Level * 25)
        {
            Honey[0] -= SelectedVertexA.Level * 25;
            SelectedVertexA.Level++;
        }
    }
}
