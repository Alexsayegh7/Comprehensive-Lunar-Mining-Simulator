using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploRover : MonoBehaviour
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

    public bool shouldMove = true;
    Transform transformTarget;

    void FixedUpdate()
    {
        if(shouldMove)
            MoveToWaypoint2(VectorFunctions.XZPlane(GameObject.Find("MiningZoneExp").transform.position));
        else
            transform.RotateAround(transformTarget.transform.position, Vector3.up, 35 * Time.deltaTime);
    }

    public void rorateAround(Transform target)
    {
        shouldMove = false;
        transformTarget = target;
    }



    public void MoveToWaypoint2(Vector3 _waypoint)
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
                step = (_AccTimer / AccTime) * MaxMovementSpeed * Time.deltaTime; // calculate distance to move
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

}
