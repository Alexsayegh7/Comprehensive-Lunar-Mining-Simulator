using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class highlighUI : MonoBehaviour
{
    // Start is called before the first frame update
    Outline outline;
    bool started=false;

    bool globalAllowedForVisualQueue = false;
    void Start()
    {
        if (PlayerPrefs.GetInt("camera") == 1)
        {
            globalAllowedForVisualQueue = true;
        }
        else
        {
            globalAllowedForVisualQueue = false;
        }
        outline = GetComponent<Outline>();
        
    }

    public void StartHighlighting()
    {
        if (globalAllowedForVisualQueue)
        {
            started = true;
            StartCoroutine(highlight());
            StartCoroutine(highlightStop());
        }
    }

    public void StopHighlighting()
    {
        started = false;
        StopCoroutine(highlight());
        outline.enabled = false;
    }

    IEnumerator highlightStop()
    {
        yield return new WaitForSeconds(5f);
        StopHighlighting();
    }

    // Update is called once per frame
    IEnumerator highlight()
    {
        while (started)
        {
            if (outline.enabled)
                outline.enabled = false;
            else
                outline.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }

    }
}
