using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{

    public Camera Cam;
    public Vector3 CameraTranslationVector = Vector3.zero;

    public float CameraSpeed;
    public float CameraMaxSpeed;


    // Start is called before the first frame update
    void Awake()
    {
        Cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        //Toggle UI Layer 
        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleUILayer();
            Debug.Log("true");
        }
        */

        //camera panning
        if (Input.GetMouseButton(0))
        {
            float YawAngle = 0;
            float PitchAngle = 0;

            YawAngle = Input.GetAxis("Mouse X");
            PitchAngle = Input.GetAxis("Mouse Y");

            transform.RotateAround(this.transform.position, Vector3.up, YawAngle);
            transform.RotateAround(this.transform.position, this.transform.right, -PitchAngle);

        }


        //camera translation
        Vector3 CameraForwardVector = Vector3.Normalize(VectorFunctions.XZPlane(transform.forward));

        if (Input.GetKey(KeyCode.W))
        {
            //CameraTranslationVector += CameraForwardVector  * Time.unscaledDeltaTime* CameraSpeed;
            this.transform.position += CameraForwardVector * Time.unscaledDeltaTime * CameraSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            //CameraTranslationVector += Vector3.Cross(CameraForwardVector,Vector3.up) * Time.unscaledDeltaTime * CameraSpeed;
            this.transform.position += Vector3.Cross(CameraForwardVector, Vector3.up) * Time.unscaledDeltaTime * CameraSpeed; ;
        }
        if (Input.GetKey(KeyCode.S))
        {
            //CameraTranslationVector += -CameraForwardVector * Time.unscaledDeltaTime * CameraSpeed;
            this.transform.position += -CameraForwardVector * Time.unscaledDeltaTime * CameraSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            //CameraTranslationVector += -Vector3.Cross(CameraForwardVector, Vector3.up) * Time.unscaledDeltaTime * CameraSpeed;
            this.transform.position += -Vector3.Cross(CameraForwardVector, Vector3.up) * Time.unscaledDeltaTime * CameraSpeed;
        }

        

        /*

        Vector3 CameraDeaccelerationVector = Vector3.Normalize(CameraTranslationVector) * 0.5f * Time.unscaledDeltaTime * CameraSpeed;
        

        if (Vector3.Magnitude(CameraTranslationVector) <= Vector3.Magnitude(CameraDeaccelerationVector))
        {
            CameraTranslationVector = Vector3.zero;
        }
        else
        {
            CameraTranslationVector += -CameraDeaccelerationVector;
            //Debug.Log("true");
        }


        if (Vector3.Magnitude(CameraTranslationVector) > CameraMaxSpeed * Time.unscaledDeltaTime)
        {
            CameraTranslationVector = Vector3.Normalize(CameraTranslationVector) * CameraMaxSpeed * Time.unscaledDeltaTime;
        }


        this.transform.position += CameraTranslationVector;
        */
    }

    private void ToggleUILayer()
    {
        Cam.cullingMask ^= 1 << LayerMask.NameToLayer("ContextUIObjects");
    }


}
