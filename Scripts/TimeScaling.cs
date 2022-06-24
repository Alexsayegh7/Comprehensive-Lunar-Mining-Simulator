using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeScaling : MonoBehaviour
{
    //script to manage time accerlation and deacceleration

    public float TimeScale = 1;

    public float FixedTimeStep = 0.02f;

    public float MaxFixedTimeStep;

    public float DefaultFixedTimeStep = 0.02f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = TimeScale;
        Time.fixedDeltaTime = FixedTimeStep;
    }


    public void ChangeTimeScale(Dropdown _dropdown)
    {
        float _timeScale = 1;

        _timeScale = Mathf.Pow(2f, _dropdown.value);


        TimeScale = _timeScale;
        FixedTimeStep = TimeScale * DefaultFixedTimeStep;

        if (FixedTimeStep > MaxFixedTimeStep)
        {
            FixedTimeStep = MaxFixedTimeStep;
        }

    }

}
