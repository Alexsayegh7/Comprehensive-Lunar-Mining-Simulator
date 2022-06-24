using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapIcon : MonoBehaviour
{

    //transform of object it is representing
    public GameObject Map;
    public Transform ReferenceTransform;
    private float _mapScale;
    
    void Awake()
    {
        Map = GameObject.FindGameObjectWithTag("Map");
        _mapScale = Map.GetComponent<Map>().MapScale;
    }

    // Update is called once per frame
    void Update()
    {

        this.GetComponent<RectTransform>().localPosition = new Vector3(ReferenceTransform.position.x/_mapScale, ReferenceTransform.position.z/_mapScale, 0f);


    }
}
