using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydrogenReductionReactor : InfrastructureElement
{

    //float SavedMaxCapacity;

    void Awake()
    {
        HasProcess = true;

        FacilityManager = GameObject.FindGameObjectWithTag("FacilityManager");

        //Input = "REGOLITH";
        Name = "HRR";
        //Needs to have more than 1 input
        //Inputs.Add("REGOLITH");
        Inputs.Add("REGOLITH_S");
        Inputs.Add("BENEFICIATED REGOLITH");

        SavedMaxCapacity = MaxCapacity;

        Output = "WATER";

        OwnContextualGameObject = GetComponentInChildren<ContextualGameObject>();

        InputPriority = 3;

        RoleRequired = "HAULING";
    }


    private void FixedUpdate()
    {
        // base.FixedUpdate(); 

        //quick debug check to see if storage thing is full 
        if (CurrentCapacity == MaxCapacity)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            gameObject.GetComponent<Renderer>().material.color = Color.grey;
        }

        if (IsProcessing == true)
        {
            gameObject.GetComponent<Renderer>().material.color =new Color(0.7080812f, 0.9811321f, 0.9664581f);
        }

        if (ProcessComplete == true && CurrentInput != "BENEFICIATED REGOLITH")
        {
          gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 122);
        }

        if (CurrentInput == "BENEFICIATED REGOLITH" && ProcessComplete == true)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.blue;
        }

        if (CurrentCapacity > MaxCapacity)
            CurrentCapacity = MaxCapacity;



        if (HasProcess == true)
        {
            Processing();
        }
        else
        {
            if (Output != "NONE")
            {
                CreateRoverTask(this, Output, FacilityManager.GetComponent<FacilityManager>());
            }



        }

        /*if (AcceptsInputs & CurrentInput == "BENEFICIATED REGOLITH" &&CurrentCapacity==0)
        { 
                CurrentInput = "";
        }*/

        if (CurrentInput == "BENEFICIATED REGOLITH")
        {
            MaxCapacity = SavedMaxCapacity * 0.25f;
           
        }
        if (CurrentInput == "")
        {
            MaxCapacity = SavedMaxCapacity;
        }


    }


    void Update()
    {
        if (CurrentCapacity == MaxCapacity && IsProcessing == true)
            FacilityManager.GetComponent<FacilityManager>().highlightHRR.StartHighlighting();


        // UI Stuff
        UIName = "HYDROGEN REDUCTION REACTOR";
        OwnContextualGameObject.ContextText1 = UIName;

        if (IsProcessing == true)
        {
            OwnContextualGameObject.ContextText2 = "PROCESSING";
        }

        if (IsProcessing == false)
        {
            OwnContextualGameObject.ContextText2 = "AWAITING INPUT";
        }


        if (IsProcessing == false && ProcessComplete == true)
        {
            OwnContextualGameObject.ContextText2 = "PROCESS COMPLETE";
        }


        if (ProcessComplete == true)
        {
            OwnContextualGameObject.ContextText3 = Output + ": " + CurrentCapacity.ToString() + "/" + MaxCapacity.ToString();
        }
        else
        {
            OwnContextualGameObject.ContextText3 = CurrentInput + ": " + CurrentCapacity.ToString() + "/" + MaxCapacity.ToString();
        }


        
    }

}
