using UnityEngine;

public class Billboard : MonoBehaviour
{
    /// <summary>
    /// Billboard offset
    /// </summary>
    public Vector3 Offset;

    void FixedUpdate()
    {
        transform.rotation = Camera.main.transform.rotation * Quaternion.Euler(Offset);
    }
}