using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContextUIObject : MonoBehaviour
{

    public GameObject ContextualObject;
    private CanvasGroup _canvasGroup;


    public Text ContextText1;
    public Text ContextText2;
    public Text ContextText3;

    private UI _UI;
    private float _mapScale;
   

    

    // Start is called before the first frame update
    void Awake()
    {
        _canvasGroup = this.GetComponent<CanvasGroup>();
        _UI = FindObjectOfType<UI>();
        _mapScale = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>().MapScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (ContextualObject != null)
        {
            Vector3 ScreenPosition = Vector3.zero;
            //positions
            if (_UI.MapModeEnabled == false)
            {
                ScreenPosition = Camera.main.WorldToScreenPoint(ContextualObject.transform.position);
            }
            else
            {


                ScreenPosition = ContextualObject.GetComponentInParent<MapObject>().MapIconInstance.transform.position;
                //added a small offset
                ScreenPosition.y += -15f;
            }


            //ScreenPosition = Camera.main.WorldToScreenPoint(ContextualObject.transform.position);
            this.transform.position = ScreenPosition;
            if (ScreenPosition[2] < 0)
            {
                _canvasGroup.interactable = false;
                _canvasGroup.alpha = 0;
            }
            else
            {
                _canvasGroup.interactable = true;
                _canvasGroup.alpha = 1;
            }

            //text

            ContextText1.text = ContextualObject.GetComponent<ContextualGameObject>().ContextText1;
            ContextText2.text = ContextualObject.GetComponent<ContextualGameObject>().ContextText2;
            ContextText3.text = ContextualObject.GetComponent<ContextualGameObject>().ContextText3;

        }
    }

    public static implicit operator ContextUIObject(GameObject v)
    {
        throw new NotImplementedException();
    }
}
