using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfrastructureElement : MonoBehaviour
{

    public GameObject FacilityManager;

    // Something to put in front of an input to alter it somehow; i.e beneficiated?
    public string InputDescriptor;
    public string Input;
    public string Output;

    //support for multiple inputs 
    public bool MultipleInputs = false;
    public List<string> Inputs = new List<string>();
    public string CurrentInput = "";
    

    public bool DelayedAwake = false;
    // for when it is not at max capacity, but inputs should not be accepted.
    public bool AcceptsInputs = true;

    public float CurrentCapacity;
    public float MaxCapacity;
    public float ReservedCapacity;
    public float ReservedOutput;
    public float SavedMaxCapacity;

    public float OutputToInputRatio = 1;

    //helps to create random radius around input, so rovers look like they are more scattered
    public float InputRadius = 0;

    public bool InfiniteOutput;

    public bool HasProcess = false;
    public float ProcessDuration;
    public bool IsProcessing = false;
    public bool ProcessComplete = false;
    public float ProcessTimer = 0f;

    public int InputPriority = 1;

    public float TaskTime = 0;
    public string TaskText = "";


    //For Rover Roles
    public string RoleRequired;

    public bool SpecificIEOutput = false;
    public string SpecificIEOutputName;


    //UI things
    public ContextualGameObject OwnContextualGameObject;
    public GameObject OwncontextUIObject;
    public string Name;
    public string UIName;

    // Start is called before the first frame update
    void Awake()
    {
        FacilityManager = GameObject.FindGameObjectWithTag("FacilityManager");
        DelayedAwake = true;
        SavedMaxCapacity = MaxCapacity;
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        //quick debug check to see if storage thing is full 
        if (CurrentCapacity ==  MaxCapacity)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            gameObject.GetComponent<Renderer>().material.color = Color.grey;
        }
        
        if (IsProcessing == true)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        }

        if (ProcessComplete == true)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.green;
        }



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


        



    }





    //Function to create Tasks
    public void CreateRoverTask(InfrastructureElement caller, string resource, FacilityManager FM)
    {
        
        if (caller.CurrentCapacity != 0 || caller.InfiniteOutput == true)
        {
            //find end IE
            InfrastructureElement EndIE = null;
            float TempDistance;
            float MinDistance = Mathf.Infinity;

            int _tempInputPriority = 1000;

            foreach (InfrastructureElement _IE in FM.GetComponent<FacilityManager>().InfrastructureElements)
            {
                //TempDistance = Vector3.Magnitude(_IE.transform.position - caller.transform.position);
                if (((_IE.MultipleInputs == false && _IE.Input == resource) || (_IE.MultipleInputs == true && (_IE.CurrentInput == resource || (_IE.CurrentInput == "" && _IE.Inputs.Contains(resource)))))
                    //&& TempDistance < MinDistance 
                    && (_IE.MaxCapacity - (_IE.CurrentCapacity + _IE.ReservedCapacity)) >= GlobalConstants.RoverMaxStorage
                    && _IE.AcceptsInputs == true
                    && _IE != caller //prevents a storage tank from delviering to itself
                    && caller.CurrentCapacity - caller.ReservedOutput >= GlobalConstants.RoverMaxStorage
                    && !(caller.GetComponent<StorageTank>() != null && _IE.GetComponent<StorageTank>() != null) //prevents storage tanks from delivering to other storage tanks, causing a loop
                    && _IE.InputPriority <= _tempInputPriority
                    && (caller.SpecificIEOutput == false || caller.SpecificIEOutputName == _IE.Name)) // for specific outputs, i.e mining zones only ouput to storage tanks 
                {
                    EndIE = _IE;
                    //MinDistance = TempDistance;
                    _tempInputPriority = _IE.InputPriority;
                }
            }

            if (EndIE != null)
            {



                RoverTask Task = new RoverTask(caller, EndIE,TaskTime,TaskText, caller.RoleRequired);
                //AvailableRoverTasks.Add(TestTask);
                FacilityManager.GetComponent<FacilityManager>().AvailableRoverTasks.Add(Task);


                if (EndIE.MultipleInputs == true)
                {
                    EndIE.CurrentInput = resource;
                }

            }

        }



    }


    public void Processing()
    {
        
        if (IsProcessing == false)
        {

            if (CurrentCapacity == MaxCapacity && ProcessComplete == false)
            {
                IsProcessing = true;
                ProcessTimer = 0f; 
            }


            if (ProcessComplete == true)
            {
                if (CurrentInput != "BENEFICIATED REGOLITH")
                    CurrentInput = "";
                // tasks only created when proccesses are complete
                CreateRoverTask(this, Output, FacilityManager.GetComponent<FacilityManager>());

                if (CurrentCapacity == 0)
                {
                    CurrentInput = "";
                    ProcessComplete = false;
                    AcceptsInputs = true;
                    MaxCapacity = SavedMaxCapacity;
                }



            }

           


        }
        else
        {
            ProcessTimer += Time.deltaTime;
            if (ProcessTimer >= ProcessDuration)
            {
                if (SavedMaxCapacity != 0f)
                {
                    MaxCapacity = SavedMaxCapacity * OutputToInputRatio;
                }


                //if (CurrentInput != "BENEFICIATED REGOLITH")
                    CurrentCapacity = SavedMaxCapacity * OutputToInputRatio;
                IsProcessing = false;
                ProcessComplete = true;
                AcceptsInputs = false;

            }
        }


    }


}
