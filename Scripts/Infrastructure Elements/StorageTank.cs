using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageTank : InfrastructureElement
{
    // Start is called before the first frame update

    //both input and ouput
    public string StoredResource;

    bool shouldHighLight = true;

    void Awake()
    {


        FacilityManager = GameObject.FindGameObjectWithTag("FacilityManager");

        Input = "REGOLITH";
        Output = StoredResource;

        OwnContextualGameObject = GetComponentInChildren<ContextualGameObject>();


        InputPriority = 2;

        RoleRequired = "HAULING";

        Name = "STORAGE TANK";

        SpecificIEOutput = true;
        SpecificIEOutputName = "HRR";

    }

    void Update()
    {
        if (CurrentCapacity == MaxCapacity && shouldHighLight)
        {
            FacilityManager.GetComponent<FacilityManager>().highlightRegolithStorageTank.StartHighlighting();
            StartCoroutine(highlightStop());
        }
          
        // UI Stuff
        UIName = StoredResource + " STORAGE TANK";
        OwnContextualGameObject.ContextText1 = UIName;
        OwnContextualGameObject.ContextText2 = CurrentCapacity.ToString() + "/" + MaxCapacity.ToString();
    }
    IEnumerator highlightStop()
    {
        yield return new WaitForSeconds(5f);
        shouldHighLight = false;
    }
}
