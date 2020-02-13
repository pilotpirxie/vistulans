using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    void Start()
    {
        Invoke("Destroy", 3f);
    }

    /// <summary>
    /// Destroy this object
    /// </summary>
    void Destroy()
    {
        GameObject.Destroy(gameObject);
    }
}
