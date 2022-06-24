using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class findHiddenMine : MonoBehaviour
{
    public int exploCount = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ExploRover")
        {
            GetComponent<MiningZone>().enabled = true;
            GetComponent<MapObject>().enabled = true;
            
            other.gameObject.GetComponent<ExploRover>().rorateAround(transform);
            //GetComponent<findHiddenMine>().enabled = false;
            exploCount++;
        }
    }
    void Start()
    {
        InvokeRepeating("addMaxOfExploMine", 5f, 5f);
    }

    void addMaxOfExploMine()
    {
        if (GetComponent<MiningZone>().enabled )
         gameObject.GetComponent<MiningZone>().addForExplo(exploCount);
    }
}
