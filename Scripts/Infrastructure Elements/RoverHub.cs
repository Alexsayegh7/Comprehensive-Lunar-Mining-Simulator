using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverHub : InfrastructureElement
{
    public GameObject explorationRoverGamePrefab;

    // where rovers go when they have to repair, recharge, or if they just have no tasks



    // Start is called before the first frame update
    void Awake()
    {
        FacilityManager = GameObject.FindGameObjectWithTag("FacilityManager");
        OwnContextualGameObject = GetComponentInChildren<ContextualGameObject>();
    }

    private void Start()
    {
        StartCoroutine(WaitTImeuncertaintyElementEnum());
    }

    // Update is called once per frame
    void Update()
    {
        // UI Stuff
        UIName = "ROVER HUB";
        OwnContextualGameObject.ContextText1 = UIName;
    }
    public void importRoverInHub()
    {
        if (Random.Range(0, 2) >= 1.0f)
        {
           FacilityManager.GetComponent<FacilityManager>().RoverPrefab.GetComponent<Rover>().Role = "MINING";
        }
        else
        {
           FacilityManager.GetComponent<FacilityManager>().RoverPrefab.GetComponent<Rover>().Role = "HAULING";
        }
        GameObject.Instantiate(FacilityManager.GetComponent<FacilityManager>().RoverPrefab, this.transform.position + Vector3.up*0.5f,this.transform.rotation);
        FacilityManager.GetComponent<FacilityManager>().QuedRovers += -1;
    }

    public void importExploRoverInHub()
    {
        GameObject.Instantiate(explorationRoverGamePrefab, this.transform.position + Vector3.up * 0.5f, this.transform.rotation);
        FacilityManager.GetComponent<FacilityManager>().QuedRovers += -1;
    }

    IEnumerator WaitTImeuncertaintyElementEnum()
    {
        yield return new WaitForSeconds(120f);
        StartCoroutine(uncertaintyElementEnum());
    }

    IEnumerator uncertaintyElementEnum()
    {
        var allRovers = FindObjectsOfType<Rover>();
        foreach (Rover rover in allRovers)
        {
            rover.uncertaintyElement();
            yield return new WaitForSeconds(40f);
        }
        StartCoroutine(uncertaintyElementEnum());
    }
}
