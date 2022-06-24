using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    // Start is called before the first frame update


    public bool MapModeEnabled = false;
    public Button MapModeButton;
    public GameObject Map;
    public FacilityManager FacilityManager;

    public Canvas Canvas;
    public Camera Cam;

    public GameObject ContextUIObjectPrefab;
    public GameObject[] ContextualGameObjects;

    public Text TimerText;
    private int _minutes = 0;
    private int _seconds = 0;

    public Text WaterExportedText;
    public TextMeshProUGUI RoverFleetInfoText;

    public TextMeshProUGUI MiningRoversText;
    public TextMeshProUGUI HaulingRoversText;

//ADDED PARTS
    public Text BudgetinkText;
    public float Budgetink = 400000;



    //Imports
    

    public bool MousePlace = false;
    public GameObject MouseTextPrefab;
    public string ObjectPlaceString = "";

    public Text txt;

    public Toggle audioToggle, roverCameraToggle;
    bool findChangeCam = false;
    void Awake()
    {
        Canvas = this.GetComponent<Canvas>();
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("camera") == 1)
        {
            roverCameraToggle.gameObject.SetActive(true);
            roverCameraToggle.onValueChanged.AddListener(cameraVisualQueueToggleFun);
        }
        else
        {
            roverCameraToggle.gameObject.SetActive(false);
        }
        audioToggle.onValueChanged.AddListener(audioOnOffToggleFun);
    }


    void audioOnOffToggleFun(bool audio)
    {
        Debug.Log("Todo add logic to on off audio");
    }
    void cameraVisualQueueToggleFun(bool value)
    {
        findChangeCam = value;
    }

    void findChaneCamFun()
    {
        if (findChangeCam)
        {
            var rovers = FindObjectsOfType<Rover>();
            foreach (Rover r in rovers)
            {
                if (r.HasTask)
                {
                    r.Owncam.SetActive(true);
                    break;
                }
                else
                    r.Owncam.SetActive(false);
               
               
            }
            Cam.gameObject.SetActive(false);
        }
        else
        {
            Cam.gameObject.SetActive(true);
            var rovers = FindObjectsOfType<Rover>();
            foreach (Rover r in rovers)
            {  
               if(r.Owncam!=null)
                r.Owncam.SetActive(false); 
            }
        }
    }

    void showToast(string text,
        int duration)
    {
        StartCoroutine(showToastCOR(text, duration));
    }

    private IEnumerator showToastCOR(string text,
        int duration)
    {
        Color orginalColor = txt.color;

        txt.text = text;
        txt.enabled = true;

        //Fade in
        yield return fadeInAndOut(txt, true, 0.5f);

        //Wait for the duration
        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            yield return null;
        }

        //Fade out
        yield return fadeInAndOut(txt, false, 0.5f);

        txt.enabled = false;
        txt.color = orginalColor;
    }

    IEnumerator fadeInAndOut(Text targetText, bool fadeIn, float duration)
    {
        //Set Values depending on if fadeIn or fadeOut
        float a, b;
        if (fadeIn)
        {
            a = 0f;
            b = 1f;
        }
        else
        {
            a = 1f;
            b = 0f;
        }

        Color currentColor = Color.clear;
        float counter = 0f;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(a, b, counter / duration);

            targetText.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            yield return null;
        }
    }

    bool checkIfUserHaveBudget(int currentExpense)
    {
        if (Budgetink >= currentExpense)
            return true;
        else
            return false;
    }

    // Update is called once per frame
    void Update()
    {
        findChaneCamFun();
        if (MapModeEnabled)
        {
            Map.GetComponent<CanvasGroup>().alpha = 1;
            Map.GetComponent<CanvasGroup>().interactable = true;
            Map.GetComponent<CanvasGroup>().blocksRaycasts = true;

        }
        else
        {
            Map.GetComponent<CanvasGroup>().alpha = 0;
            Map.GetComponent<CanvasGroup>().interactable = false;
            Map.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }


        //placing infrastructure elements with mouse
        if (MousePlace == true)
        {
            if (GetComponentInChildren<MouseText>() == null)
            {
                var MouseText = GameObject.Instantiate(MouseTextPrefab, this.transform);
                
                if (ObjectPlaceString == "REGOLITH_TANK")
                {
                    MouseText.GetComponent<MouseText>().MouseText1.text = "REGOLITH STORAGE TANK";
                }
                
                
            }

            // left click
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (ObjectPlaceString == "REGOLITH_TANK")
                {
                    if (checkIfUserHaveBudget(20000))
                    {
                        RaycastHit hit;
                        Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
                        if (Physics.Raycast(ray, out hit))
                        {
                            Vector3 point = new Vector3(hit.point.x, 1, hit.point.z);
                            var tank = GameObject.Instantiate(FacilityManager.RegolithStorageTankPrefab, point, Quaternion.identity);
                        }
                        Budgetink -= 20000;
                        FacilityManager.recordAndSaveUserDecision("Storage-REGOLITH_TANK", 20000, Budgetink);
                        BudgetinkText.text = "BUDGET IN K: " + Budgetink.ToString();

                    }
                    //ADDED PARTS
                    else
                    {
                        showToast("Out of Budget", 2);
                    }

                    
                }
               
                if (ObjectPlaceString == "WATER_TANK" )
                {
                    if (checkIfUserHaveBudget(8000))
                    {
                        RaycastHit hit;
                        Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
                        if (Physics.Raycast(ray, out hit))
                        {
                            Vector3 point = new Vector3(hit.point.x, 1, hit.point.z);
                            var tank = GameObject.Instantiate(FacilityManager.WaterStorageTankPrefab, point, Quaternion.identity);
                        }

                        Budgetink -= 8000;
                        FacilityManager.recordAndSaveUserDecision("Storage-WATER_TANK", 8000, Budgetink);
                        BudgetinkText.text = "BUDGET IN K: " + Budgetink.ToString();

                    }
                    else
                    {
                        showToast("Out of Budget", 2);
                    }

                }
              
                if (ObjectPlaceString == "LANDING_ZONE")
                {
                    RaycastHit hit;
                    Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit))
                    {
                        Vector3 point = new Vector3(hit.point.x, 1, hit.point.z);
                        var tank = GameObject.Instantiate(FacilityManager.LandingZonePrefab, point, Quaternion.identity);
                    }
                    //Settlement will be free so it's zero
                    Budgetink -= 0;
                    FacilityManager.recordAndSaveUserDecision("Settlement", 0, Budgetink);
                    BudgetinkText.text = "BUDGET IN K: " + Budgetink.ToString();

                }
                if (ObjectPlaceString == "HRR"  )
                {
                    if (checkIfUserHaveBudget(80000))
                    {
                        RaycastHit hit;
                        Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
                        if (Physics.Raycast(ray, out hit))
                        {
                            Vector3 point = new Vector3(hit.point.x, 1, hit.point.z);
                            var tank = GameObject.Instantiate(FacilityManager.HRRPrefab, point, Quaternion.identity);
                        }

                        Budgetink -= 80000;
                        FacilityManager.recordAndSaveUserDecision("Storage-HRR", 80000, Budgetink);
                        BudgetinkText.text = "BUDGET IN K: " + Budgetink.ToString();
                    }
                    else
                    {
                        showToast("Out of Budget", 2);
                    }
                }
                
                if (ObjectPlaceString == "BENEFICIATOR"  )
                {
                    if (checkIfUserHaveBudget(30000))
                    {
                        RaycastHit hit;
                        Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
                        if (Physics.Raycast(ray, out hit))
                        {
                            Vector3 point = new Vector3(hit.point.x, 1, hit.point.z);
                            var tank = GameObject.Instantiate(FacilityManager.BeneficiatorPrefab, point, Quaternion.identity);
                        }
                        Budgetink -= 30000;
                        FacilityManager.recordAndSaveUserDecision("Storage-BENEFICIATOR", 30000, Budgetink);
                        BudgetinkText.text = "BUDGET IN K: " + Budgetink.ToString();
                    }
                    else
                    {
                        showToast("Out of Budget", 2);
                    }
                }
                MousePlace = false;
                GameObject.Destroy(GetComponentInChildren<MouseText>().gameObject);
                ObjectPlaceString = "";
            }


            // right click 
            if (Input.GetKey(KeyCode.Mouse1))
            {
                MousePlace = false;
                GameObject.Destroy(GetComponentInChildren<MouseText>().gameObject);
                ObjectPlaceString = "";
            }

        }

    }


    void FixedUpdate()
    {
        ContextualGameObjects = GameObject.FindGameObjectsWithTag("ContextualGameObject");

        //context UI object spawner 
        foreach (GameObject _contextGameObject in ContextualGameObjects)
        {
            if (_contextGameObject.GetComponent<ContextualGameObject>().UIObjectInstantiated == false)
            {
                GameObject _contextUIObject = Instantiate(ContextUIObjectPrefab);
                _contextUIObject.transform.parent = Canvas.transform;
                _contextUIObject.GetComponent<ContextUIObject>().ContextualObject = _contextGameObject;

                if (_contextGameObject.transform.parent.name.Contains("Settlement"))
                {
                    _contextGameObject.transform
                        .parent.gameObject.GetComponent<LandingZone>().OwncontextUIObject= _contextUIObject;
                }
                if (_contextGameObject.transform.parent.name.Contains("Mining"))
                {
                    _contextGameObject.transform
                        .parent.gameObject.GetComponent<MiningZone>().OwncontextUIObject = _contextUIObject;
                }

                _contextGameObject.GetComponent<ContextualGameObject>().UIObjectInstantiated = true;
            }
        }






        float _timeInSeconds = FacilityManager.SimulationTimer;

            
        _seconds = Mathf.RoundToInt((_timeInSeconds- _minutes*60)-0.5f);

        if (_seconds == 60)
        {
            _minutes += 1;
            _seconds = 0;
        }

        string _secondFormat = "";
        if (_seconds < 10)
        {
            _secondFormat = "0";
        }


        TimerText.text = "TIME: " + _minutes.ToString() + ":" + _secondFormat + _seconds.ToString();

        //Water Exported Text

        WaterExportedText.text = "WATER EXPORTED: " + FacilityManager.WaterExported.ToString();

        if (FacilityManager.InactiveRovers == 0)
        {
            FacilityManager.highlightRover.StartHighlighting();
        }
        //Infrastructure Info
        RoverFleetInfoText.text = "ROVERS| " + "TOTAL: " + FacilityManager.Rovers.Length + " | INACTIVE: " + FacilityManager.InactiveRovers;// + " | QUED: " + FacilityManager.QuedRovers;

        //RoverFleet
        MiningRoversText.text = FacilityManager.MiningRovers.ToString() + " (" + FacilityManager.RequiredMiningRovers.ToString() + ")";
        HaulingRoversText.text = FacilityManager.HaulingRovers.ToString() + " (" + FacilityManager.RequiredHaulingRovers.ToString() + ")";


    }
    public void rovercost(bool isExplo)
    {
        if (checkIfUserHaveBudget(50000))
        {

            Budgetink -= 50000;
            if (!isExplo)
                FacilityManager.recordAndSaveUserDecision("Workforce", 50000, Budgetink);
            else
                FacilityManager.recordAndSaveUserDecision("Exploration", 50000, Budgetink);
            BudgetinkText.text = "BUDGET IN K: " + Budgetink.ToString();
        }
        else
        {
            showToast("Out of Budget", 2);
        }


    }



    public void MapModeButtonPress()
    {
        MapModeEnabled = !MapModeEnabled;
    }

    public void PlaceRegolithStorageTankButtonPress()
    {
        MousePlace = true;
        ObjectPlaceString = "REGOLITH_TANK";
    }
    public void PlaceWaterStorageTankButtonPress()
    {
        MousePlace = true;
        ObjectPlaceString = "WATER_TANK";
    }
    public void PlaceLandingZonePress()
    {
        MousePlace = true;
        ObjectPlaceString = "LANDING_ZONE";
    }
    public void PlaceHRRPress()
    {
        MousePlace = true;
        ObjectPlaceString = "HRR";
    }
    public void PlaceBeneficiatorPress()
    {
        MousePlace = true;
        ObjectPlaceString = "BENEFICIATOR";
    }




}
