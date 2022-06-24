using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beneficiator : InfrastructureElement
{
    void Awake()
    {
      //  HasProcess = true;

        FacilityManager = GameObject.FindGameObjectWithTag("FacilityManager");

        Input = "REGOLITH";

        Output = "REGOLITH";

        OwnContextualGameObject = GetComponentInChildren<ContextualGameObject>();

        SavedMaxCapacity = MaxCapacity;

        InputPriority = 2;

        RoleRequired = "HAULING";
        Name = "STORAGE TANK";

        SpecificIEOutput = true;
        SpecificIEOutputName = "HRR";

    }

    private void FixedUpdate()
    {
        base.FixedUpdate();
        /*
        if (ProcessComplete == true)
        {
            MaxCapacity = (2 / 10) * SavedMaxCapacity;
            if (CurrentCapacity > MaxCapacity)
            {
                CurrentCapacity = MaxCapacity;
            }


        }
        else
        {
            MaxCapacity = SavedMaxCapacity;
        }
        */
        if (CurrentCapacity == MaxCapacity&& ProcessTimer<10&& Output == "REGOLITH")
        {
            HasProcess = true;
            IsProcessing = true;
        }
        /*else if(ProcessTimer < 10)
        {
            HasProcess = false;
            IsProcessing = false;
        }
*/

    }


    // Update is called once per frame
    void Update()
    {
        if (CurrentCapacity == MaxCapacity && IsProcessing == true)
            FacilityManager.GetComponent<FacilityManager>().highlightBeneficiator.StartHighlighting();

        // UI Stuff
        UIName = "REGOLITH BENEFICIATOR";
        OwnContextualGameObject.ContextText1 = UIName;

        if (IsProcessing == true)
        {
            OwnContextualGameObject.ContextText2 = "PROCESSING";
            Output = "REGOLITH";
        }

        if (IsProcessing == false)
        {
            OwnContextualGameObject.ContextText2 = "AWAITING INPUT";
            Output = "REGOLITH";
        }


        if (IsProcessing == false && ProcessComplete == true)
        {
            OwnContextualGameObject.ContextText2 = "PROCESS COMPLETE";
            Output = "BENEFICIATED REGOLITH";
            ProcessTimer = 0f;
        }


        if (ProcessComplete == true)
        {
            OwnContextualGameObject.ContextText3 = Output + ": " + CurrentCapacity.ToString() + "/" + MaxCapacity.ToString();
        }
        else
        {
            OwnContextualGameObject.ContextText3 = Input + ": " + CurrentCapacity.ToString() + "/" + MaxCapacity.ToString();
        }



    }
}

