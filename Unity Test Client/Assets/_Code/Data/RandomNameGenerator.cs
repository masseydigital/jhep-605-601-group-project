using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNameGenerator : MonoBehaviour
{
    public List<string> adjective;
    public List<string> noun;

    public string RandomName()
    {
        string adj = adjective[Random.Range(0, adjective.Count)];
        string nou = noun[Random.Range(0, noun.Count)];
        int num = Random.Range(0, 1000);

        return adj + nou + num;
    }
}
