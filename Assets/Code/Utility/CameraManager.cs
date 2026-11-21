using System;
using UnityEngine;

/// <summary>
/// Camera manager is a singleton for managing camera movement and zooming.
/// </summary>
public class CameraManager : MonoBehaviour
{
    private static UserInputActions inputActions;
    private static CameraManager instance;

    [SerializeField] private Camera mainCamera;

    [Range(0f, 1f)]
    [SerializeField] private float sensitivity = 0.03f; //User defined sensitivity.

    private const float sensitivityScalar = 0.2f; //Massively scale down sensitivity.

    private float cameraZoom = 5f;
    [SerializeField] private float cameraZoomStep = 0.2f;
    [SerializeField] private float maxZoom = 1f;
    [SerializeField] private float minZoom = 15f;

    void Start()
    {
        //Singleton
        if(instance == null)
        {
            CameraManager.instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }

        //Set input actions to active
        inputActions = new UserInputActions();
        inputActions.Enable();
    }

    void Update()
    {
        //Protect against null exceptions.
        if(mainCamera != null)
        {
            if(inputActions != null)
            {
                MoveCamera();
                ZoomCamera();
            }
            else
            {
                Debug.LogError("Input actions not initiated! Did you forget a new call?");
            }
        }
        else
        {
            Debug.LogError("Camera not attached to camera manager script. Add this in the inspector!");
        }
    }

    /// <summary>
    /// A method to move the cameras position based on the mouse move key and the mouse movement itself via delta.
    /// This method should only be called by this class.
    /// CAUTION: This method can throw a null reference when camera or input actions are null.
    /// </summary>
    private void MoveCamera()
    {
        //Get user input from input actions.
        bool movingCamera = inputActions.UserInputActionMap.CameraMoveMouseKey.ReadValue<float>() > 0f;
        Vector2 moveDir = inputActions.UserInputActionMap.CameraMouseDelta.ReadValue<Vector2>();

        //Move camera based on if key down.
        if (movingCamera)
        {
            mainCamera.transform.position += new Vector3(-moveDir.x, -moveDir.y, 0f) * sensitivity * sensitivityScalar;
        }
    }

    /// <summary>
    /// A method to zoom the camera based on the mouse scroll value.
    /// This method should only be called by this class.
    /// CAUTION: This method can throw a null reference when camera or input actions are null.
    /// </summary>
    private void ZoomCamera()
    {
        Vector2 scroll = inputActions.UserInputActionMap.Scroll.ReadValue<Vector2>(); //Use y val only.

        if (scroll != Vector2.zero)
        {
            float newCameraZoom = cameraZoom;

            if (scroll.y < 0)
            {
                newCameraZoom += cameraZoomStep;
            }
            else if(scroll.y > 0)
            {
                newCameraZoom -= cameraZoomStep;
            }

            newCameraZoom = Mathf.Clamp(newCameraZoom, maxZoom, minZoom);
            cameraZoom = newCameraZoom;
            mainCamera.orthographicSize = cameraZoom;
        }
    }
}
