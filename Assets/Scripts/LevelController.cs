using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    public GameObject VertexObject;

    [SerializeField]
    private int aId = -1; 

    [SerializeField]
    private int bId = -1;

    void Start()
    {
        TextAsset levelConfigContent = Resources.Load<TextAsset>("Config/levels");
        Debug.Log($"Loaded level configuration: {levelConfigContent}");
        LevelConfig levelConfig = JsonUtility.FromJson<LevelConfig>(levelConfigContent.text);

        foreach (VertexProxy vertexProxy in levelConfig.levels[0].vertexProxies)
        {
            GameObject newVertex = GameObject.Instantiate(VertexObject, new Vector3(vertexProxy.x * 1f, 0.5f, -vertexProxy.y * 1f), Quaternion.identity);
            newVertex.GetComponent<VertexController>().X = vertexProxy.x;
            newVertex.GetComponent<VertexController>().Y = vertexProxy.y;
            newVertex.GetComponent<VertexController>().Owner = (OwnerType)vertexProxy.owner;
            newVertex.GetComponent<VertexController>().Type = (VertexType)vertexProxy.type;
            newVertex.GetComponent<VertexController>().ArmyPower = 0;
            newVertex.GetComponent<VertexController>().Level = 0;
            newVertex.GetComponent<VertexController>().Id = vertexProxy.id;
            newVertex.tag = "Vertex";
            newVertex.name = $"vertex{vertexProxy.id}";
        }

        foreach (Connection connection in levelConfig.levels[0].connections)
        {
            GameObject vertexA = GameObject.Find($"vertex{connection.a}");
            GameObject vertexB = GameObject.Find($"vertex{connection.b}");

            vertexA.GetComponent<VertexController>().Connections.Add(vertexB);
            vertexB.GetComponent<VertexController>().Connections.Add(vertexA);
        }
    }

    public void OnVertexTouch(int id)
    {
        if (aId == -1)
        {
            aId = id;
        } else
        {
            bId = id;
        }
    }

    public void FixedUpdate()
    {
        if (aId != -1 && bId == -1)
        {
            GameObject vertex = GameObject.Find($"vertex{aId}");

            vertex.GetComponent<Renderer>().material.color = Color.white;

            foreach (GameObject connectedVertex in vertex.GetComponent<VertexController>().Connections)
            {
                connectedVertex.GetComponent<Renderer>().material.color = Color.yellow;
            }
        }

        if (aId != -1 && bId != -1)
        {
            GameObject sourceVertex = GameObject.Find($"vertex{aId}");

            foreach (GameObject possibleVertex in sourceVertex.GetComponent<VertexController>().Connections)
            {
                if (possibleVertex.GetComponent<VertexController>().Id == bId)
                {
                    // send signal to move units
                    Debug.Log($"Sent unit from {aId} to {bId}");
                }
            }

            foreach (GameObject vertex in GameObject.FindGameObjectsWithTag("Vertex"))
            {
                vertex.GetComponent<Renderer>().material.color = Color.clear;
            }

            aId = -1;
            bId = -1;
        }
    }

    public void SendArmy(int from, int to, int amount)
    {
        GameObject vertexA = GameObject.Find($"vertex{from}");
        GameObject vertexB = GameObject.Find($"vertex{to}");

        if (vertexA.GetComponent<VertexController>().ArmyPower >= amount)
        {

        }
        else
        {
            // insufficient army power
        }
    }
}
