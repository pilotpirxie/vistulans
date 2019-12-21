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

    public GraphController _graphController;

    public float TransportPart = 0.5f;

    /// <summary>
    /// -1 = none
    /// 0 = offensive
    /// 1 = earthquake
    /// 2 = takeover
    /// </summary>
    public int SpellToCast = -1;

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

        _graphController = gameObject.GetComponent<GraphController>();
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

    public void FixedUpdate()
    {
        if (SelectedVertexA != null && SelectedVertexB == null && SpellToCast != -1)
        {
            switch(SpellToCast)
            {
                case 0:
                    CastOffensiveSpell(SelectedVertexA);
                    Mana[0] -= 100;
                    break;
                case 1:
                    CastEarthquakeSpell(SelectedVertexA);
                    Mana[0] -= 300;
                    break;
                case 2:
                    CastTakeoverCast(SelectedVertexA, OwnerType.Player);
                    Mana[0] -= 500;
                    break;
            }

            SpellToCast = -1;
            _graphController.ClearSelection();
        }
    }

    public void UpgradeVertex(VertexController vertex)
    {
        if (vertex.Level < 5 && Honey[(int)vertex.Owner-1] >= vertex.Level * 25)
        {
            Honey[(int)vertex.Owner - 1] -= vertex.Level * 25;
            vertex.Level++;
        }
    }

    public void SetSpellToCast(int spellIndex = -1)
    {
        if (spellIndex == 0 && Mana[0] >= 100
            || spellIndex == 1 && Mana[0] >= 300
            || spellIndex == 2 && Mana[0] >= 500)
        {
            if (spellIndex == SpellToCast)
            {
                SpellToCast = -1;
            }
            else
            {
                SpellToCast = spellIndex;
            }
        }
        else
        {
            Debug.Log("Insufficient mana");
        }

        _graphController.ClearSelection();
    }

    public void CastOffensiveSpell(VertexController vertex)
    {
        vertex.ArmyPower -= 100;

        if (vertex.ArmyPower < 1)
        {
            vertex.ArmyPower = 1;
        }
    }

    public void CastEarthquakeSpell(VertexController vertex)
    {
        foreach (VertexController tempVertex in _vertexList)
        {
            if (tempVertex.Owner == vertex.Owner)
            {
                tempVertex.ArmyPower -= 50;

                if (tempVertex.ArmyPower < 1)
                {
                    tempVertex.ArmyPower = 1;
                }
            }
        }
    }

    public void CastTakeoverCast(VertexController vertex, OwnerType whoCast)
    {
        vertex.ArmyPower -= (int)Mathf.Floor(vertex.ArmyPower * 0.5f);

        if (vertex.ArmyPower < 1)
        {
            vertex.ArmyPower = 1;
        }

        vertex.Owner = whoCast;
    }
}
