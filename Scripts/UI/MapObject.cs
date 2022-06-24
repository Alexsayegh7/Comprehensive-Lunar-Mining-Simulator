using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    // Attach to Objects that have a Map Icon

    public GameObject MapIconPrefab;
    public GameObject Map;

    public GameObject MapIconInstance;

    void Awake()
    {
        Map = GameObject.FindGameObjectWithTag("Map");
        //spawn Map Object
        GameObject _mapIcon = Instantiate(MapIconPrefab);
        //_map
        _mapIcon.transform.parent = Map.transform;
        _mapIcon.GetComponent<MapIcon>().ReferenceTransform = this.transform;

        MapIconInstance = _mapIcon;

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
