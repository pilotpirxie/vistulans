using UnityEngine;

public class LevelController : MonoBehaviour
{

    public GameObject VertexObject;

    void Start()
    {
        TextAsset levelConfigContent = Resources.Load<TextAsset>("Config/levels");
        Debug.Log($"Loaded level configuration: {levelConfigContent}");
        LevelConfig levelConfig = JsonUtility.FromJson<LevelConfig>(levelConfigContent.text);

        foreach(VertexProxy vertexProxy in levelConfig.levels[0].vertexProxies)
        {
            Object.Instantiate(VertexObject, new Vector3(vertexProxy.x * 1f, 0.5f, vertexProxy.y * 1f), Quaternion.identity);
        }
    }

}
