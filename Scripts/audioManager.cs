using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioManager : MonoBehaviour
{

    public AudioClip lowBudget, lowWater, LowTime;
    public AudioSource audioSource;

    bool isPlayedBudget, isPlayedTime;
    // Start is called before the first frame update
    void Start()
    {
        isPlayedBudget= isPlayedTime = false;
    }

    public void PlayLowBudget()
    {
        if (!isPlayedBudget)
        {
            audioSource.PlayOneShot(lowBudget);
            isPlayedBudget = true;
        }
           
    }
    public void PlayLowTime()
    {
        if (!isPlayedTime)
        {
          audioSource.PlayOneShot(LowTime);
            isPlayedTime = true;
        }
    }
    public void PlayLowWater()
    {
        audioSource.PlayOneShot(lowWater);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
