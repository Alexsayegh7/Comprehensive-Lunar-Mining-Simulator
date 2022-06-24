using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MenuManager : MonoBehaviour
{
    public Button startGameBtn;
    public Button settingBtn;
    public Button quitGameBtn;
    public Button changeCameraPosBtn;
    public Toggle audioOnOffToggle,cameraVisualQueueToggle;
    public Button goBackBtn;

    public GameObject menuPanel, settingPanel;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("camera") == 1)
        {
            cameraVisualQueueToggle.isOn = true;
        }
        else
        {
            cameraVisualQueueToggle.isOn = false;
        }

        startGameBtn.onClick.AddListener(startGame);
        settingBtn.onClick.AddListener(settingBtnFun);
        quitGameBtn.onClick.AddListener(quitGameBtnFun);
        changeCameraPosBtn.onClick.AddListener(changeCameraPosBtnFun);
        audioOnOffToggle.onValueChanged.AddListener(audioOnOffToggleFun);
        goBackBtn.onClick.AddListener(goBackBtnFun);
        cameraVisualQueueToggle.onValueChanged.AddListener(cameraVisualQueueToggleFun);
    }

    

    void startGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    void settingBtnFun()
    {
        menuPanel.SetActive(false);
        settingPanel.SetActive(true);
    }
    void quitGameBtnFun()
    {
        Application.Quit();
    }
    void changeCameraPosBtnFun()
    {
        Debug.Log("Todo add logic to change camera position");
    }
    void audioOnOffToggleFun(bool audio)
    {
        Debug.Log("Todo add logic to on off audio");
    }
    void cameraVisualQueueToggleFun(bool value)
    {
        PlayerPrefs.SetInt("camera",value?1:0);
    }
    void goBackBtnFun()
    {
        menuPanel.SetActive(true);
        settingPanel.SetActive(false);
    }
}
