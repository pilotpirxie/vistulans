using System.Collections;
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

    void Start()
    {
        Mana = new int[] { 0, 0, 0, 0 };
        Honey = new int[] { 0, 0, 0, 0 }; 
        Army = new int[] { 0, 0, 0, 0 }; 
    }

    void Update()
    {
        
    }
}
