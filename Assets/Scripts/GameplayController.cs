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

    /// <summary>
    /// List of all vertex on the map
    /// </summary>
    public List<VertexController> VertexList;

    /// <summary>
    /// Represents which vertex is actually selected
    /// </summary>
    public GameObject SunshaftPrefab;

    /// <summary>
    /// First and second vertices
    /// </summary>
    public VertexController SelectedVertexA;
    public VertexController SelectedVertexB;

    /// <summary>
    /// Reference to Graph Controller in Mechanism object
    /// </summary>
    public GraphController _graphController;

    /// <summary>
    /// Part of total army power to send from vertex A to B
    /// </summary>
    public float TransportPart = 0.5f;

    /// <summary>
    /// Used for Time scale in UI Controller
    /// </summary>
    public float GameplaySpeedMultiplier = 1.0f;

    /// <summary>
    /// -1 = none
    /// 0 = offensive
    /// 1 = earthquake
    /// 2 = takeover
    /// </summary>
    public int SpellToCast = -1;

    /// <summary>
    /// Is howing pause menu or not
    /// </summary>
    public bool IsShowingMenu = false;

    void Start()
    {
        Mana = new int[] { 0, 0, 0, 0, 0 };
        Honey = new int[] { 0, 0, 0, 0, 0 }; 
        Army = new int[] { 0, 0, 0, 0, 0 };

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Vertex"))
        {
            VertexList.Add(obj.GetComponent<VertexController>());
        }

        InvokeRepeating("IncreaseUnits", 2.0f, 2.0f);

        _graphController = gameObject.GetComponent<GraphController>();
    }

    public void FixedUpdate()
    {
        CastPlayerSpell();
        SetTimeScale();
    }

    /// <summary>
    /// Set sunshaft to vertex
    /// </summary>
    /// <param name="vertex">Vertex</param>
    public void SetPositionOfSunshaft()
    {
        if (SelectedVertexA != null && SelectedVertexB == null)
        {
            SunshaftPrefab.SetActive(true);
            SunshaftPrefab.transform.position = GameObject.Find($"vertex{SelectedVertexA.Id}").transform.position;
        }
        else
        {
            SunshaftPrefab.SetActive(false);
        }
    }

    /// <summary>
    /// Check if current time scale is different for expected
    /// and set new one based on choosen by player
    /// </summary>
    void SetTimeScale()
    {
        if (Time.timeScale != GameplaySpeedMultiplier)
        {
            Time.timeScale = GameplaySpeedMultiplier;
        }
    }

    /// <summary>
    /// Increase Mana and Honey
    /// Recalculate Army power 
    /// </summary>
    void IncreaseUnits()
    {
        for (int i = 0; i < Army.Length; i++)
        {
            Army[i] = 0;
        }

        foreach (VertexController vertex in VertexList)
        {
            switch (vertex.Type)
            {
                case VertexType.Shrine:
                    Mana[(int)vertex.Owner] += vertex.Level;
                    break;
                case VertexType.Village:
                    vertex.ArmyPower += vertex.Level;
                    Army[(int)vertex.Owner] += vertex.ArmyPower;
                    break;
                case VertexType.Apiary:
                    Honey[(int)vertex.Owner] += vertex.Level;
                    break;
            }
        }
    }

    /// <summary>
    /// Check if player selects spell and first vertex (vertex to affect by spell)
    /// And cast spell then substract mana
    /// </summary>
    void CastPlayerSpell()
    {
        if (SelectedVertexA != null && SelectedVertexB == null && SpellToCast != -1)
        {
            switch (SpellToCast)
            {
                case 0:
                    CastOffensiveSpell(SelectedVertexA);
                    Mana[1] -= 100;
                    break;
                case 1:
                    CastEarthquakeSpell(SelectedVertexA);
                    Mana[1] -= 300;
                    break;
                case 2:
                    CastTakeoverCast(SelectedVertexA, OwnerType.Player);
                    Mana[1] -= 500;
                    break;
            }

            SpellToCast = -1;
            _graphController.ClearSelection();
        }
    }

    /// <summary>
    /// Check if owner of vertex has sufficient amount of honey
    /// Then upgrade or not vertex by increasing vertex level
    /// </summary>
    /// <param name="vertex">Vertex to upgrade</param>
    public void UpgradeVertex(VertexController vertex)
    {
        if (vertex.Level < 5 && Honey[(int)vertex.Owner] >= vertex.Level * 25)
        {
            Honey[(int)vertex.Owner] -= vertex.Level * 25;
            vertex.Level++;
            vertex.SetViewObject();

            if (vertex.Level == 5)
            {
                SelectedVertexA = null;
                SelectedVertexB = null;
                SetPositionOfSunshaft();
            }
        }
    }
    
    /// <summary>
    /// Set index of spell from UI Controller if player has sufficient amount of mana
    /// </summary>
    /// <param name="spellIndex">Index of spell</param>
    public void SetSpellToCastByPlayer(int spellIndex = -1)
    {
        if (spellIndex == 0 && Mana[1] >= 100
            || spellIndex == 1 && Mana[1] >= 300
            || spellIndex == 2 && Mana[1] >= 500)
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

    /// <summary>
    /// Remove 100 army power for choosen vertex
    /// </summary>
    /// <param name="vertex"></param>
    public void CastOffensiveSpell(VertexController vertex)
    {
        vertex.ArmyPower -= 100;

        if (vertex.ArmyPower < 1)
        {
            vertex.ArmyPower = 1;
        }
    }

    /// <summary>
    /// Substract 50 army power from each vertex of vertex owner
    /// </summary>
    /// <param name="vertex"></param>
    public void CastEarthquakeSpell(VertexController vertex)
    {
        foreach (VertexController tempVertex in VertexList)
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

    /// <summary>
    /// Substract half of the army power from vertex
    /// And change owner to the spell caster
    /// </summary>
    /// <param name="vertex"></param>
    /// <param name="whoCast"></param>
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
