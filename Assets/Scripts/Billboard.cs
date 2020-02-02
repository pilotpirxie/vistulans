using UnityEngine;

public class Billboard : MonoBehaviour
{
    void FixedUpdate()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}