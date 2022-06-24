using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MouseText : MonoBehaviour
{

    public TextMeshProUGUI MouseText1;
    public TextMeshProUGUI MouseText2;
    public TextMeshProUGUI MouseText3;


    // Start is called before the first frame update
    void Awake()
    {
        MouseText2.text = "LEFT CLICK TO PLACE";
        MouseText3.text = "RIGHT CLICK TO CANCEL";
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Input.mousePosition;
    }
}
