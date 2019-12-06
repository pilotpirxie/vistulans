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
            newVertex.GetComponent<Vertex>().X = vertexProxy.x;
            newVertex.GetComponent<Vertex>().Y = vertexProxy.y;
            newVertex.GetComponent<Vertex>().Owner = (OwnerType)vertexProxy.owner;
            newVertex.GetComponent<Vertex>().Type = (VertexType)vertexProxy.type;
            newVertex.GetComponent<Vertex>().Power = 0;
            newVertex.GetComponent<Vertex>().Level = 0;
            newVertex.GetComponent<Vertex>().Id = vertexProxy.id;
            newVertex.tag = "Vertex";
            newVertex.name = $"vertex{vertexProxy.id}";
        }

        foreach (Connection connection in levelConfig.levels[0].connections)
        {
            foreach (GameObject vertexA in GameObject.FindGameObjectsWithTag("Vertex"))
            {
                foreach (GameObject vertexB in GameObject.FindGameObjectsWithTag("Vertex"))
                {
                    if (vertexA.GetComponent<Vertex>().Id == connection.a)
                    {
                        if (vertexB.GetComponent<Vertex>().Id == connection.b)
                        {
                            vertexA.GetComponent<Vertex>().Connections.Add(vertexB);
                            vertexB.GetComponent<Vertex>().Connections.Add(vertexA);
                        }
                    }
                }
            }
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

            foreach (GameObject connectedVertex in vertex.GetComponent<Vertex>().Connections)
            {
                connectedVertex.GetComponent<Renderer>().material.color = Color.yellow;
            }
        }
        else
        {
            foreach (GameObject vertex in GameObject.FindGameObjectsWithTag("Vertex"))
            {
                vertex.GetComponent<Renderer>().material.color = Color.clear;
            }

        }


        if (aId >= 0 && bId >= 0)
        {
            GameObject sourceVertex = GameObject.Find($"vertex{aId}");

            foreach (GameObject possibleVertex in sourceVertex.GetComponent<Vertex>().Connections)
            {
                if (possibleVertex.GetComponent<Vertex>().Id == bId)
                {
                    // send signal to move units
                    Debug.Log($"Sent unit from {aId} to {bId}");
                }
            }

            aId = -1;
            bId = -1;
        }
    }
}
