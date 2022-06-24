using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    public int countdowntime;
    public Text countdowndisplay;

    public GameObject gameOverPannel;

    public FacilityManager facilityManager;

    private void Start()
    {
        facilityManager = GetComponent<FacilityManager>();
        StartCoroutine(CountdownToStart());
    }
    
        IEnumerator CountdownToStart()
        {
            while(countdowntime > 0)
            {
                countdowndisplay.text = countdowntime.ToString();

                yield return new WaitForSeconds(1f);

                countdowntime--;
                if (countdowntime <= 100 )
                    facilityManager.AudioManager.PlayLowTime();

            }
            countdowndisplay.text = "Go ";

            yield return new WaitForSeconds(1f);

            countdowndisplay.gameObject.SetActive(false);
            gameOverPannel.transform.SetAsLastSibling();
            gameOverPannel.SetActive(true);

        }
        
}      

