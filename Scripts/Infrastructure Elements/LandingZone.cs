using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingZone : InfrastructureElement
{

    public GameObject Spacecraft;

    public bool isExtraSettlement = false;

    public bool SCLanded = true;
    public bool SCFull = false;
    public bool Transferring = false;

    public float TransferDuration;
    public float TransferTimer = 0f;
    public float LZHeight;
    
    public float SCAcc = 2;

    //Import Stuff
    //public bool CanImport = false;
    //public List<GameObject> ImportQue = new List<GameObject>();

    public GameObject[] ContextualUiGameObjects { get; private set; }

    void Awake()
    {
        FacilityManager = GameObject.FindGameObjectWithTag("FacilityManager");
        LZHeight = Spacecraft.transform.position.y;

        Input = "WATER";

        Output = "NONE";

        UIName = "SETTLEMENT";

        OwnContextualGameObject = GetComponentInChildren<ContextualGameObject>();

        InputPriority = 1;
    }

    //need to overwrite in this case
    void FixedUpdate()
    {
        
        if (CurrentCapacity <= 0)
        {
            Destroy(OwncontextUIObject);
            Destroy(OwnContextualGameObject);
            Destroy(GetComponent<MapObject>().MapIconInstance);
            Destroy(gameObject);
                SCFull = true;
            Transferring = true;
        }
        else
        {
            SCFull = false;
        }

        if (CurrentCapacity == MaxCapacity)
        {
            Spacecraft.GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            Spacecraft.GetComponent<Renderer>().material.color = Color.grey;
        }

        Transfer();

    }

    void Subtract()
    {
        CurrentCapacity -=5;
        if (CurrentCapacity <= 100 && CurrentCapacity >= 95)
            FacilityManager.GetComponent<FacilityManager>().AudioManager.PlayLowWater();
    }

    void Start()
    {
        InvokeRepeating("Subtract", 5f, 2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name + other.name.Contains("Rover"));
        if (other.name.Contains("Rover"))
        {
            if (other.gameObject.GetComponent<Rover>().Role == "HAULING")
            {
                FacilityManager.GetComponent<FacilityManager>().WaterExported += other.gameObject.GetComponent<Rover>().CurrentStorage;
            }
        }
    }




    void Update()
    {
        OwnContextualGameObject.ContextText1 = UIName;

        if (Transferring == true)
        {
            OwnContextualGameObject.ContextText2 = "NO WATER LEFT";
        }
        else
        {

            OwnContextualGameObject.ContextText2 = "WATER IS NEEDED";
        }

        OwnContextualGameObject.ContextText3 = Input + ": " + CurrentCapacity.ToString() + "/" + MaxCapacity.ToString();

       Transfer();

    }

    public void Transfer()
    {

        float _tempVelocity = 0;
        if (Transferring == true)
        {
            AcceptsInputs = false;
            //CanImport = true;
            //TransferTimer += Time.deltaTime;

            //if (TransferTimer <= TransferDuration)
            //{
            //if (TransferTimer <= TransferDuration * 0.5f)
            //{
            //TransferTimer += Time.deltaTime;

            float _x = Spacecraft.transform.position.x;
            float _y = LZHeight + SCAcc * Mathf.Pow(TransferTimer, 2);
            float _z = Spacecraft.transform.position.z;

            Spacecraft.transform.position = new Vector3(_x, _y, _z);
            SCLanded = false;
            //_tempVelocity = TransferTimer * SCAcc;

        }
                //else
                //{
                    //float _x = Spacecraft.transform.position.x;
                    //float _y = LZHeight - _tempVelocity * (TransferTimer - TransferDuration * 0.5f)  + SCAcc * Mathf.Pow(TransferTimer - TransferDuration, 2);
                    //float _z = Spacecraft.transform.position.z;

                   //Spacecraft.transform.position = new Vector3(_x, _y, _z);
                    //FacilityManager.GetComponent<FacilityManager>().WaterExported += CurrentCapacity;
                    //CurrentCapacity = 0f;

                //}
            //}
            //else
            //{

                //Transferring = false;
                
                //TransferTimer = 0f;

                //float _x = Spacecraft.transform.position.x;
                //float _y = LZHeight;
                //float _z = Spacecraft.transform.position.z;

                //Spacecraft.transform.position = new Vector3(_x, _y, _z);

                //if (ImportQue.Count >= 1)
               //{
                    //GameObject.Instantiate(ImportQue[0], this.transform.position + Vector3.up*0.5f,this.transform.rotation);
                    //ImportQue.RemoveAt(0);
                    //FacilityManager.GetComponent<FacilityManager>().QuedRovers += -1;
                //}

         

                //CanImport = false;
            //}




        //}
        else
        {
            
            AcceptsInputs = true;
        }

    }



}
