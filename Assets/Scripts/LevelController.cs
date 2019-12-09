using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    public GameObject VertexObject;

    [SerializeField]
    public GameObject ArmyObject;

    [SerializeField]
    private int _touchedVertexAId = -1; 

    [SerializeField]
    private int _touchedVertexBId = -1;

    void Start()
    {
        TextAsset levelConfigContent = Resources.Load<TextAsset>("Config/levels");
        Debug.Log($"Loaded level configuration: {levelConfigContent}");
        LevelConfig levelConfig = JsonUtility.FromJson<LevelConfig>(levelConfigContent.text);

        foreach (VertexConfig vertexConfig in levelConfig.levels[0].verticies)
        {
            GameObject newVertex = GameObject.Instantiate(VertexObject, new Vector3(vertexConfig.x * 1f, 0.5f, -vertexConfig.y * 1f), Quaternion.identity);
            newVertex.GetComponent<VertexController>().X = vertexConfig.x;
            newVertex.GetComponent<VertexController>().Y = vertexConfig.y;
            newVertex.GetComponent<VertexController>().Owner = (OwnerType)vertexConfig.owner;
            newVertex.GetComponent<VertexController>().Type = (VertexType)vertexConfig.type;
            newVertex.GetComponent<VertexController>().ArmyPower = vertexConfig.power;
            newVertex.GetComponent<VertexController>().Level = 0;
            newVertex.GetComponent<VertexController>().Id = vertexConfig.id;
            newVertex.tag = "Vertex";
            newVertex.name = $"vertex{vertexConfig.id}";
        }

        foreach (EdgeConfig connection in levelConfig.levels[0].edges)
        {
            GameObject vertexA = GameObject.Find($"vertex{connection.a}");
            GameObject vertexB = GameObject.Find($"vertex{connection.b}");

            vertexA.GetComponent<VertexController>().Connections.Add(vertexB);
            vertexB.GetComponent<VertexController>().Connections.Add(vertexA);
        }
    }

    public void OnVertexTouch(int id)
    {
        if (_touchedVertexAId == -1)
        {
            _touchedVertexAId = id;
        } else
        {
            _touchedVertexBId = id;
        }
    }

    public void FixedUpdate()
    {
        if (_touchedVertexAId != -1 && _touchedVertexBId == -1)
        {
            GameObject selectedVertex = GameObject.Find($"vertex{_touchedVertexAId}");

            selectedVertex.GetComponent<Renderer>().material.color = Color.white;

            foreach (GameObject connectedVertex in selectedVertex.GetComponent<VertexController>().Connections)
            {
                connectedVertex.GetComponent<Renderer>().material.color = Color.yellow;
            }
        }

        if (_touchedVertexAId != -1 && _touchedVertexBId != -1)
        {
            GameObject selectedVertex = GameObject.Find($"vertex{_touchedVertexAId}");

            foreach (GameObject possibleVertex in selectedVertex.GetComponent<VertexController>().Connections)
            {
                if (possibleVertex.GetComponent<VertexController>().Id == _touchedVertexBId)
                {
                    if (selectedVertex.GetComponent<VertexController>().ArmyPower > 1)
                    {
                        int armyPowerToSend = selectedVertex.GetComponent<VertexController>().ArmyPower / 2;
                        selectedVertex.GetComponent<VertexController>().ArmyPower -= armyPowerToSend;

                        SendArmy(_touchedVertexAId, _touchedVertexBId, armyPowerToSend);
                        Debug.Log($"Sent unit from {_touchedVertexAId} to {_touchedVertexBId}");
                    }
                }
            }

            foreach (GameObject vertex in GameObject.FindGameObjectsWithTag("Vertex"))
            {
                vertex.GetComponent<Renderer>().material.color = Color.clear;
            }

            _touchedVertexAId = -1;
            _touchedVertexBId = -1;
        }
    }

    public void SendArmy(int origin, int target, int amount)
    {
        GameObject vertexA = GameObject.Find($"vertex{origin}");
        GameObject vertexB = GameObject.Find($"vertex{target}");

        if (vertexA.GetComponent<VertexController>().ArmyPower >= amount)
        {
            Vector3 spawnPosition = vertexA.gameObject.transform.position;
            spawnPosition.y = 0.25f;
            GameObject newArmy = GameObject.Instantiate(ArmyObject, spawnPosition, Quaternion.identity);
            newArmy.GetComponent<ArmyController>().Owner = vertexA.GetComponent<VertexController>().Owner;
            newArmy.GetComponent<ArmyController>().ArmyPower = amount;
            newArmy.GetComponent<ArmyController>().Origin = origin;
            newArmy.GetComponent<ArmyController>().Target = target;
        }
        else
        {
            // insufficient army power
        }
    }
}
