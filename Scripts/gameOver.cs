using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class gameOver : MonoBehaviour
{
    // Start is called before the first frame update

    public Text scoreText;

    [HideInInspector]
    public FacilityManager FacilityManager;

    public GameObject decisionLinePrefab;

    public Transform content;

    public Text gameOverWaterExported;

    private int score = 0;
    void Start()
    {
        FacilityManager = FindObjectOfType<FacilityManager>();
        calculateScore();
        gameOverWaterExported.text = "WATER EXPORTED: " + FacilityManager.WaterExported.ToString();
    }

    public void calculateScore()
    {

        foreach (InfrastructureElement infrastructureElement in FacilityManager.InfrastructureElements)
        {
            if (infrastructureElement.GetComponent<LandingZone>() != null)
            {
                if(infrastructureElement.GetComponent<LandingZone>().isExtraSettlement)
                    score += 20;
                else
                    score += 10;
            }
        }

        scoreText.text = score.ToString();

        for (int i = 1; i <= FacilityManager.decisionCount; ++i)
        { 
            string decisionLine = PlayerPrefs.GetString("decisonLine"+i);
            GameObject go=GameObject.Instantiate(decisionLinePrefab,content);
            go.GetComponentInChildren<Text>().text = decisionLine;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
