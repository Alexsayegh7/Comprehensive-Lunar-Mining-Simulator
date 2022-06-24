using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Rover : MonoBehaviour
{

        // set in inspector 
    public float MaxMovementSpeed;
    public float TurnSpeed;
    public float AccTime;
    //Time taken to get up to full turn speed
    public float TurnSpeedTime;

    public bool HasTask = false;
    public RoverTask CurrentTask;

    // Set in inpector for now
    [HideInInspector]
    public FacilityManager FacilityManager;

    public float WaypointSensitivity = 1;

    public bool ArrivedAtStart;
    public bool ArrivedAtEnd;

    private float _currentSpeed;
    private float _AccTimer = 0;
    private float _TurnSpeedTimer = 0;

    public bool PerformingTask = false;

    //Storage things
    public string CargoString;

    //universal
    public float MaxStorage = GlobalConstants.RoverMaxStorage;
    public float CurrentStorage;

    //UI things
    public ContextualGameObject OwnContextualGameObject;

    private float _yAxixConstraint;

    // rover roles 
    public string Role;
    public bool AtRoverHub = false;
    public bool Reconfiguring = false;


    public GameObject MiningAugerPrefab;
    public GameObject StorageTankPrefab;

    public GameObject Owncam;

    // Start is called before the first frame update
    void Awake()
    {
        //_yAxixConstraint = this.transform.position.y;
        OwnContextualGameObject = GetComponentInChildren<ContextualGameObject>();
        FacilityManager = FindObjectOfType<FacilityManager>();

        if (Role == "MINING")
        {
            var Auger = GameObject.Instantiate(MiningAugerPrefab, this.transform);
        }
        if (Role == "HAULING")
        {
            var Tank = GameObject.Instantiate(StorageTankPrefab, this.transform);
        }

        MaxMovementSpeed = 20;
    }

    public void uncertaintyElement()
    {
        var r = Random.Range(0, 100);
        if (r < 30)
        {
            MaxMovementSpeed = 1;
            GetComponent<Renderer>().material.color = Color.red;
            FacilityManager.roverWarning.SetActive(true);
            StartCoroutine(fixRoverFromUncertainThings());
        }
        
        
    }
    IEnumerator fixRoverFromUncertainThings()
    {
        yield return new WaitForSeconds(15f);
        MaxMovementSpeed = 20;
        GetComponent<Renderer>().material.color = Color.white;
        FacilityManager.roverWarning.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //makes sure it does not go anywhere funky at speed up times - does not really work 
        //this.transform.position = new Vector3(this.transform.position.x,_yAxixConstraint,this.transform.position.z);

        if (CurrentTask != null)
        {
            HasTask = true;
            AtRoverHub = false;
        }
        else
        {
            // Get Task
            if (FacilityManager.AvailableRoverTasks.Count != 0)
            {
                //prioritize tasks with closest start point to rover
                float TempDistance;
                float MinDistance = Mathf.Infinity;
                int _taskIndex = -1;
                foreach (RoverTask _task in FacilityManager.AvailableRoverTasks)
                {
                    if (_task.StartIE != null)
                    {
                        TempDistance = Vector3.Magnitude(this.transform.position - _task.StartIE.transform.position);

                        if (TempDistance < MinDistance && _task.RoverRole == Role)
                        {
                            _taskIndex = FacilityManager.AvailableRoverTasks.IndexOf(_task);
                        }
                    }
          

                }


                if (_taskIndex != -1)
                {
                    CurrentTask = FacilityManager.AvailableRoverTasks[_taskIndex];
                    FacilityManager.AvailableRoverTasks[_taskIndex].TaskAssigned = true;
                    //remove task from global list
                    FacilityManager.AvailableRoverTasks.Remove(FacilityManager.AvailableRoverTasks[_taskIndex]);
                    ArrivedAtStart = false;
                    ArrivedAtEnd = false;
                }
                else
                {
                    HasTask = false;
                }



            }
            else
            {
                HasTask = false;
            }
   



        }


        //performing task and reconfiguring indicates the coroutine is running
        if (HasTask == true && PerformingTask == false && Reconfiguring == false)
        {
            
            if (!ArrivedAtStart)
            {
                MoveToWaypoint(CurrentTask.StartWaypoint);
                if (Vector3.Magnitude(VectorFunctions.XZPlane(transform.position) - CurrentTask.StartWaypoint) <= WaypointSensitivity)
                {
                    ArrivedAtStart = true;
                    _AccTimer = 0;
                    _TurnSpeedTimer = 0;

                    //IE part
                    CargoString = CurrentTask.StartIE.Output;

                    if (CurrentTask.StartIE.InfiniteOutput == true)
                    {
                        CurrentStorage = MaxStorage;
                    }
                    else
                    {
                        CurrentStorage = Mathf.Min(MaxStorage, CurrentTask.StartIE.CurrentCapacity);
                        CurrentTask.StartIE.CurrentCapacity = CurrentTask.StartIE.CurrentCapacity - CurrentStorage;
                        CurrentTask.StartIE.ReservedOutput += -CurrentStorage;
                    }




                    PerformingTask = true;
                    StartCoroutine(PerformTask(CurrentTask.TaskTime));
                    

                    


                }
            }
            else if (!ArrivedAtEnd)
            {
                MoveToWaypoint(CurrentTask.EndWaypoint);

                if (Vector3.Magnitude(VectorFunctions.XZPlane(transform.position) - CurrentTask.EndWaypoint) <= WaypointSensitivity)
                {
                    ArrivedAtEnd = true;
                    _AccTimer = 0;
                    _TurnSpeedTimer = 0;

                    //IE part
                    CargoString = null;


                    CurrentTask.EndIE.CurrentCapacity += CurrentStorage;
                    CurrentTask.EndIE.ReservedCapacity += -CurrentStorage;
                    CurrentStorage = 0f;



                }
            }
            else
            {
                // mark task as completed and deassign task
                //CurrentTask.TaskCompleted = true;
                CurrentTask = null;
            }




        }
        else
        {
            //performing task indicates the coroutine is running
            if (PerformingTask == false && Reconfiguring == false)
            {
                //go to nearest hub - currently  really simple script. 



                float TempDistance;
                float MinDistance = Mathf.Infinity;
                RoverHub Hub = null;
                foreach (InfrastructureElement _IE in FacilityManager.InfrastructureElements)
                {

                    if (_IE.GetComponent<RoverHub>() != null)
                    {
                        TempDistance = Vector3.Magnitude(this.transform.position - _IE.transform.position);
                        if (TempDistance < MinDistance)
                        {
                            MinDistance = TempDistance;
                            Hub = _IE.GetComponent<RoverHub>();
                        }
                    }

                }

                if (Hub != null && Vector3.Magnitude(this.transform.position - Hub.transform.position) > 0.9f)
                {
                    MoveToWaypoint(VectorFunctions.XZPlane(Hub.transform.position));



                }

                if (Vector3.Magnitude(this.transform.position - Hub.transform.position) < 1)
                {
                    AtRoverHub = true;

                }

            }


        }


      //zero out role if required

        if (AtRoverHub == true && Reconfiguring==false)
        {
            if (Role == "MINING" && FacilityManager.RoversToConfigureToMining > 0)
            {
                Role = "";
                GameObject.Destroy(transform.GetChild(1).gameObject);
            }
            if (Role == "HAULING" && FacilityManager.RoversToConfigureToHauling > 0)
            {
                Role = "";
                GameObject.Destroy(transform.GetChild(1).gameObject);
            }
        }


        // role assignment at hub
        if (AtRoverHub == true && Role == "")
        {
            string newrole = "";
            if (FacilityManager.RoversToConfigureToMining > 0)
            {
                newrole = "MINING";
                var Auger = GameObject.Instantiate(MiningAugerPrefab, this.transform);
                FacilityManager.RoversToConfigureToMining--;
            }
            else if (FacilityManager.RoversToConfigureToHauling > 0)
            {
                newrole = "HAULING";
                var Tank = GameObject.Instantiate(StorageTankPrefab, this.transform);
                FacilityManager.RoversToConfigureToHauling--;
            }

            StartCoroutine(ReconfigureRover(1, newrole));
        }
    }


    void Update()
    {
        //setting contexutal texts
        
        if (OwnContextualGameObject != null)
        {

            if (Role == "")
            {
                OwnContextualGameObject.ContextText1 =  " UNASSIGNED ROVER ";
            }
            else
            {
                OwnContextualGameObject.ContextText1 = Role + " ROVER ";
            }
            if (CargoString != "" && CargoString != null)
            {
                OwnContextualGameObject.ContextText2 = "HAULING " + CargoString;

                OwnContextualGameObject.ContextText3 = CurrentStorage.ToString() + "/" + MaxStorage.ToString();
            }
            else
            {
                OwnContextualGameObject.ContextText2 = "";

                OwnContextualGameObject.ContextText3 = "";
            }
        }

        if (PerformingTask == true)
        {
            OwnContextualGameObject.ContextText2 = CurrentTask.TaskText;

            OwnContextualGameObject.ContextText3 = "";
        }


    }


    public void MoveToWaypoint(Vector3 _waypoint)
    {
        //turn to waypoint - from https://answers.unity.com/questions/1663326/smoothly-rotate-object-towards-target-object.html
        
        
        Vector3 targetDirection = -VectorFunctions.XZPlane((transform.position) - _waypoint);
        float _targetDistance = Vector3.Magnitude(targetDirection);

        float _smallTurnAngle = 20f;
        float _stoppingTime = AccTime; // this makes it more smooth

        float _angleToTarget = Vector3.Angle(VectorFunctions.XZPlane(transform.forward), targetDirection);
        if (_angleToTarget > 0.5f)
        {
            Vector3 RotationDirection;
            
            
            
                if (_angleToTarget > _smallTurnAngle)
                {
                    if (_TurnSpeedTimer <= TurnSpeedTime)
                    {

                        RotationDirection = Vector3.RotateTowards(transform.forward, targetDirection, ((_TurnSpeedTimer / TurnSpeedTime)) * TurnSpeed * Time.deltaTime, 0.0f);
                        _TurnSpeedTimer += Time.deltaTime;

                    }
                    else
                    {
                        RotationDirection = Vector3.RotateTowards(transform.forward, targetDirection, TurnSpeed * Time.deltaTime, 0.0f);
                    }
                                       
                    
                }
                else
                {
                    RotationDirection = Vector3.RotateTowards(transform.forward, targetDirection, ((_angleToTarget / _smallTurnAngle) + 0.01f) * TurnSpeed * Time.deltaTime, 0.0f);
                   
                }
            

            transform.rotation = Quaternion.LookRotation(RotationDirection);





        }
        else
        {
            float step;
            if (_AccTimer <= AccTime)
            {
                //building up to full speed
                step = (_AccTimer/AccTime)*MaxMovementSpeed*Time.deltaTime; // calculate distance to move
                _AccTimer += Time.deltaTime;
                
            }
            else
            {
                if (_targetDistance / MaxMovementSpeed >= _stoppingTime)
                {

                    //maxspeed 
                    step = MaxMovementSpeed * Time.deltaTime; // calculate distance to move
                                        

                }
                else
                {

                    //slowing down
                    step = Mathf.Min((((_targetDistance / MaxMovementSpeed) / _stoppingTime) * MaxMovementSpeed + 0.0001f) * Time.deltaTime, MaxMovementSpeed * Time.deltaTime); // calculate distance to move
                                        
                }
               
            }

            transform.position = Vector3.MoveTowards(transform.position, _waypoint, step);




        }


    }

    //enumerator function to perform tasks at points
    IEnumerator PerformTask(float taskTime)
    {
        float _timeCounter = 0;



        while (PerformingTask == true)
        {
            yield return null;
            _timeCounter += Time.deltaTime;

            if (_timeCounter >= taskTime)
            {
                PerformingTask = false;
            }
        }
        
    }

    public IEnumerator ReconfigureRover(float ReconfigureTime, string NewRole)
    {
        float _timeCounter = 0;
        Role = NewRole;
        Reconfiguring = true;
        while (Reconfiguring == true)
        {
            yield return null;
            _timeCounter += Time.deltaTime;

            if (_timeCounter >= ReconfigureTime)
            {
                
                Reconfiguring = false;
            }
            OwnContextualGameObject.ContextText2 = "RECONFIGURING";
        }
    }

 
}

