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
    public List<ImageTargetBehaviour> wonders;

    private string currentWonder;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        SwitchCameras();
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

    public void ChangeCurrentWonder(string wonderName)
    {
        currentWonder = wonderName;
        globe.gameObject.SetActive(false);

        foreach (ImageTargetBehaviour wonder in wonders)
        {
            if (wonder.gameObject.name == currentWonder)
            {
                wonder.gameObject.SetActive(true);
            }
            else
            {
                wonder.gameObject.SetActive(false);
            }
        }
    }
}
