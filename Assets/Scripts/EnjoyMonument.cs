using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnjoyMonument : MonoBehaviour
{
    public GameObject enjoyMonumentUI;

    private Button captureButton;
    private Button continueButton;

    public static event Action ContinueClicked;

    private void Awake()
    {
        captureButton = enjoyMonumentUI.transform.GetChild(0).GetComponent<Button>();
        continueButton = enjoyMonumentUI.transform.GetChild(1).GetComponent<Button>();
    }

    private void Start()
    {
        enjoyMonumentUI.SetActive(true);
        captureButton.onClick.AddListener(OnCaptureButtonClicked);
        continueButton.onClick.AddListener(OnContinueButtonClicked);
    }

    public void OnCaptureButtonClicked()
    {
        ScreenCapture.CaptureScreenshot(Application.persistentDataPath + "/WondARCapture" + 
            GameManager.instance.currentScreenShot + ".png");
        GameManager.instance.currentScreenShot++;
        GameManager.instance.PlayAudioClip("ScreenShot");
    }

    public void OnContinueButtonClicked()
    {
        enjoyMonumentUI.SetActive(false);
        if (ContinueClicked != null)
            ContinueClicked();

        captureButton.onClick.RemoveAllListeners();
        continueButton.onClick.RemoveAllListeners();
    }
}
