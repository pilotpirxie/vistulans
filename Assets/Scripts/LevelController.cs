using UnityEngine;

public class LevelController : MonoBehaviour
{

    public GameObject VertexObject;

    void Start()
    {
        TextAsset levelConfigContent = Resources.Load<TextAsset>("Config/levels");
        Debug.Log($"Loaded level configuration: {levelConfigContent}");
        LevelConfig levelConfig = JsonUtility.FromJson<LevelConfig>(levelConfigContent.text);

        foreach (VertexProxy vertexProxy in levelConfig.levels[0].vertexProxies)
        {
            GameObject newVertex = Object.Instantiate(VertexObject, new Vector3(vertexProxy.x * 1f, 0.5f, -vertexProxy.y * 1f), Quaternion.identity);
            newVertex.GetComponent<Vertex>().X = vertexProxy.x;
            newVertex.GetComponent<Vertex>().Y = vertexProxy.y;
            newVertex.GetComponent<Vertex>().Owner = (OwnerType)vertexProxy.owner;
            newVertex.GetComponent<Vertex>().Type = (VertexType)vertexProxy.type;
            newVertex.GetComponent<Vertex>().Power = 0;
            newVertex.GetComponent<Vertex>().Level = 0;
            newVertex.GetComponent<Vertex>().Id = vertexProxy.id;
            newVertex.tag = "Vertex";
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
}
