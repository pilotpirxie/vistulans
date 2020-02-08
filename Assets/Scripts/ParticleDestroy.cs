using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    void Start()
    {
        Invoke("Destroy", 3f);
    }

    void Destroy()
    {
        GameObject.Destroy(gameObject);
    }
}
