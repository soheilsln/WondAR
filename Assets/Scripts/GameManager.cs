using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Cameras")]
    public Camera mainCamera;
    public Camera ARCamera;

    [Header("Vuforia Targets")]
    public ImageTargetBehaviour globe;
    public List<ImageTargetBehaviour> targets;

    private string currentTarget;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        //SwitchCameras();
    }

    public void SwitchCameras()
    {
        if (mainCamera.gameObject.activeSelf)
        {
            mainCamera.gameObject.SetActive(false);
            ARCamera.gameObject.SetActive(true);
        }
        else if (ARCamera.gameObject.activeSelf)
        {
            ARCamera.gameObject.SetActive(false);
            mainCamera.gameObject.SetActive(true);
        }
    }

    public void ChangeCurrentTarget(string wonderName)
    {
        currentTarget = wonderName;
        globe.gameObject.SetActive(false);

        foreach (ImageTargetBehaviour target in targets)
        {
            if (target.gameObject.name == currentTarget)
            {
                target.gameObject.SetActive(true);
            }
            else
            {
                target.gameObject.SetActive(false);
            }
        }
    }

    public void ChangeTargetToGlobe()
    {
        foreach (ImageTargetBehaviour target in targets)
        {
            target.gameObject.SetActive(false);
        }
        globe.gameObject.SetActive(true);
    }
}
