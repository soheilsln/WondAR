using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Camera ARCamera;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

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
}
