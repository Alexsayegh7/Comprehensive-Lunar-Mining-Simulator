using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningZone : InfrastructureElement
{
    // Start is called before the first frame update

    //size of mining zone
    //public float MiningRadius;
    public bool isExploMine = false;

    void Awake()
    {

        FacilityManager = GameObject.FindGameObjectWithTag("FacilityManager");

        Input = "NONE";
        Output = "REGOLITH";


       // InfiniteOutput = true;

       // CurrentCapacity = Mathf.Infinity;

        OwnContextualGameObject = GetComponentInChildren<ContextualGameObject>();

        RoleRequired = "MINING";

        SpecificIEOutput = true;
        SpecificIEOutputName = "STORAGE TANK";
    }
    void SubtractMine()
    {
        if(CurrentCapacity<=1000)
            FacilityManager.GetComponent<FacilityManager>().highlightExploRover.StartHighlighting();
        if (CurrentCapacity >= 100)
        {
            CurrentCapacity -= 100;
        }
        else
        {
            Destroy(GetComponent<MapObject>().MapIconInstance);
            Destroy(OwncontextUIObject);
            Destroy(gameObject);
        }

            
    }
    public void addForExplo(int exploRovCount)
    {
        if (isExploMine)
        {

            MaxCapacity += 10*exploRovCount;
            CurrentCapacity = MaxCapacity;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log(other.name + other.name.Contains("Rover"));
        if (other.name.Contains("Rover"))
        {
            if (other.gameObject.GetComponent<Rover>().Role == "MINING"
                && other.gameObject.GetComponent<Rover>().CargoString== "REGOLITH")
            {
                SubtractMine();
            }
        }
    }

    private void FixedUpdate()
    {
       
        base.FixedUpdate();




    }

    void Update()
    {
        // UI Stuff
        UIName = "MINING ZONE";
        OwnContextualGameObject.ContextText1 = UIName;
        OwnContextualGameObject.ContextText3 = CurrentCapacity.ToString() + "/" + MaxCapacity.ToString();

    }




}
