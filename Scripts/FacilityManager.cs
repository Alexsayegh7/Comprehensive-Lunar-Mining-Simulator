using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FacilityManager : MonoBehaviour
{
    public highlighUI highlightRover, highlightExploRover,
        highlightRegolithStorageTank, highlightHRR, highlightBeneficiator, 
        highlightMineFleet, highlightHaulFleet;

    //works as the game manager as well

    
    public List<RoverTask> AvailableRoverTasks = new List<RoverTask>();
    public InfrastructureElement[] InfrastructureElements;

    public audioManager AudioManager;

    //Test Gameobjects
    public Transform StartTest;
    public Transform EndTest;


    //Game manager 
    public float SimulationTimer = 0f;
    public float WaterExported = 0f;
    public float BudgetInK = 400000;


    //Infrastructure Call downs
    public GameObject RoverPrefab;
    public GameObject RegolithStorageTankPrefab;
    public GameObject LandingZonePrefab;
    public GameObject WaterStorageTankPrefab;
    public GameObject HRRPrefab;
    public GameObject BeneficiatorPrefab;


    //Infrastructure Information
    public Rover[] Rovers;
    public int QuedRovers = 0;
    public int InactiveRovers = 0;


    //Rover Fleet Configuration and Managment
    public Slider MiningSlider;
    public float MiningRatio;
    public Slider HaulingSlider;
    public float HaulingRatio;

    public float ActualMiningRatio;
    public float ActualHaulingRatio;

    public int MiningRovers;
    public int HaulingRovers;

    public int RequiredMiningRovers;
    public int RequiredHaulingRovers;

    public int RoversToConfigureToMining;
    public int RoversToConfigureToHauling;

    public Text pauseResumeText;

    public int decisionCount = 1;

    public UI ui;

    public Button confirmButton;

    public GameObject roverWarning;


    // Start is called before the first frame update
    void Awake()
    {

        //Managing Facilities
        InfrastructureElements = FindObjectsOfType<InfrastructureElement>();

        //Rover Fleet Sliders
        MiningSlider.value = 7;
       // HaulingSlider.value = 2f;

        confirmButton.gameObject.SetActive(false);

    }
    public void recordAndSaveUserDecision(string decision,int currenReducingBudget,float balanceBudget)
    {
        if (balanceBudget <= 20000)
            AudioManager.PlayLowBudget();
        //Storage,Workforce,Exploration,Settlement
        PlayerPrefs.SetInt("index", decisionCount);
        string decisionLine = getOrdinal(decisionCount)+" decision: type : "+ decision + " - "
            +ui.TimerText.text+ " - Budget:"+(balanceBudget+currenReducingBudget).ToString()+"-"+currenReducingBudget+"="+balanceBudget.ToString();
        Debug.Log(decisionLine);
        PlayerPrefs.SetString("decisonLine"+decisionCount.ToString(), decisionLine);
        PlayerPrefs.Save();
        decisionCount++;
    }

    public void recordAndSaveUserDecisionSlider(string decision,  float balanceBudget)
    {
            //Storage,Workforce,Exploration,Settlement
            PlayerPrefs.SetInt("index", decisionCount);
            string decisionLine = getOrdinal(decisionCount) + " Decision: Type : " + decision + " - "
                + ui.TimerText.text + " (" + MiningRovers + " Mining - " + HaulingRovers + " Hauling) - Budget:"
                + (balanceBudget).ToString() + "-0=" + balanceBudget.ToString();
            Debug.Log(decisionLine);
            PlayerPrefs.SetString("decisonLine" + decisionCount.ToString(), decisionLine);
            PlayerPrefs.Save();
            decisionCount++;
    
    }

    public string getOrdinal(int num)
    {
        if (num <= 0) return num.ToString();

        switch (num % 100)
        {
            case 11:
            case 12:
            case 13:
                return num + "th";
        }

        switch (num % 10)
        {
            case 1:
                return num + "st";
            case 2:
                return num + "nd";
            case 3:
                return num + "rd";
            default:
                return num + "th";
        }
    }

    public void pauseGame()
    {
        if (pauseResumeText.text == "Resume")
        {
            pauseResumeText.text = "Pause";
            Time.timeScale = 0;
        }
        else
        {
            pauseResumeText.text = "Resume";
            Time.timeScale = 1;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {


        SimulationTimer += Time.deltaTime;

        //Managing Facilities
        InfrastructureElements = FindObjectsOfType<InfrastructureElement>();
        Rovers = FindObjectsOfType<Rover>();
        InactiveRovers = 0;

        MiningRovers = 0;
        HaulingRovers = 0;


        ActualMiningRatio = MiningRovers / Rovers.Length;
        ActualHaulingRatio = HaulingRovers / Rovers.Length;


        //useful stat to keep track of
        //Debug.Log(AvailableRoverTasks.Count);

        RequiredMiningRovers = Mathf.RoundToInt((MiningRatio / 10) * Rovers.Length);
        RequiredHaulingRovers = Rovers.Length - RequiredMiningRovers;

        RoversToConfigureToMining = RequiredMiningRovers - MiningRovers;
        RoversToConfigureToHauling = RequiredHaulingRovers - HaulingRovers;

        foreach (Rover _rover in Rovers)
        {
            if (_rover.HasTask == false)
            {
                InactiveRovers += 1;
            }

            if (_rover.Role == "MINING")
            {
                MiningRovers += 1;
            }
            if (_rover.Role == "HAULING")
            {
                HaulingRovers += 1;
            }

        }

    }




    public void ImportRover()
    {

       /* int _minLength = 10000;
        LandingZone LZ = null;
        LandingZone[] LandingZones = GameObject.FindObjectsOfType<LandingZone>();
        foreach (LandingZone _LZ in LandingZones)
        {
            if (_LZ.ImportQue.Count < _minLength)
            {
                _minLength = _LZ.ImportQue.Count;
                LZ = _LZ;
            }
        }

        LZ.ImportQue.Add(RoverPrefab);
        QuedRovers += 1;*/


        

    }


    public void ChangedMiningRatio()
    {
        MiningRatio = MiningSlider.value;

        //simple, but works with just 2
        HaulingRatio = 10 - MiningRatio;
        HaulingSlider.value = HaulingRatio;

        confirmButton.gameObject.SetActive(true);

    }
    public void ChangedHaulingRatio()
    {
        HaulingRatio = HaulingSlider.value;

        MiningRatio = 10 - HaulingRatio;
        MiningSlider.value = MiningRatio;

        confirmButton.gameObject.SetActive(true);
    }

    public void confirmMiningAndHaulingRatio()
    {
        confirmButton.gameObject.SetActive(false);
        recordAndSaveUserDecisionSlider("Workforce", ui.Budgetink);
        removeAllTaskOfROver();
    }
    public void removeAllTaskOfROver()
    {

        foreach (Rover _rover in Rovers)
        {
            if (!_rover.HasTask)
            {

                _rover.Role = "";
                _rover.PerformingTask = false;
               // _rover.HasTask = false;
                _rover.CurrentTask = null;

                GameObject.Destroy(_rover.transform.GetChild(1).gameObject);
                string newrole = "";
                if (RoversToConfigureToMining > 0)
                {
                    newrole = "MINING";
                    var Auger = GameObject.Instantiate(_rover.MiningAugerPrefab, _rover.transform);
                    RoversToConfigureToMining--;
                }
                else if (RoversToConfigureToHauling > 0)
                {
                    newrole = "HAULING";
                    var Tank = GameObject.Instantiate(_rover.StorageTankPrefab, _rover.transform);
                    RoversToConfigureToHauling--;
                }


                StartCoroutine(_rover.ReconfigureRover(1.0f, newrole));

            }


        }
    }


}


public class RoverTask
{

    public InfrastructureElement StartIE;
    public InfrastructureElement EndIE;

    public string Cargo;
    public float CargoAmount;

    public Vector3 StartWaypoint;
    public Vector3 EndWaypoint;

    public bool TaskAssigned = false;
    public bool TaskCompleted = false;
    //public bool DoIndefinitely = false; 

    //for the Coroutine
    public float TaskTime;
    public string TaskText;

    //rover roles 
    public string RoverRole;

    //constructor
    public RoverTask(InfrastructureElement _start, InfrastructureElement _end, float taskTime, string taskText, string roverRole)
    {
        StartWaypoint = VectorFunctions.XZPlane(_start.transform.position + UnityEngine.Random.insideUnitSphere*_start.InputRadius);
        EndWaypoint = VectorFunctions.XZPlane(_end.transform.position);
        //DoIndefinitely = _ind;
        StartIE = _start;
        EndIE = _end;

        Cargo = StartIE.Output;
        CargoAmount = GlobalConstants.RoverMaxStorage;

        // Modify Reserved Output
        StartIE.ReservedOutput += CargoAmount;


        //Modify Reserved Amount of End
        EndIE.ReservedCapacity += CargoAmount;

        TaskTime = taskTime;
        TaskText = taskText;

        RoverRole = roverRole;

    }

   





}