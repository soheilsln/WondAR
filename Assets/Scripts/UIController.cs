using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject splashScreen;
    public GameObject selectWonder;

    [Header("Tasks")]
    public GameObject tasks;
    public Button startTaskButton;
    public Text taskNumber;
    public Text taskText;
    public Image scratch;

    private GameObject currentBlip;
    private int currentTask = 1;

    private void Awake()
    {
        splashScreen.gameObject.SetActive(true);
        currentTask = 1;
    }

    private void Start()
    {
        Globe.OnBlipsClicked += this.OnBlipsClicked;
        Cloud.OnCloudsDestroyed += this.StartSecondTask;
        Artefact.OnAllArtefactsFound += this.StartThirdTask;
    }

    private void OnDestroy()
    {
        Globe.OnBlipsClicked -= this.OnBlipsClicked;
        Cloud.OnCloudsDestroyed -= this.StartSecondTask;
        Artefact.OnAllArtefactsFound -= this.StartThirdTask;
    }

    public void OnScanButtonClicked()
    {
        splashScreen.gameObject.SetActive(false);
        GameManager.instance.SwitchCameras();
    }

    private void OnBlipsClicked(Collider collider)
    {
        currentBlip = collider.transform.parent.gameObject;
        selectWonder.SetActive(true);
        Text wonderName = selectWonder.GetComponentInChildren<Text>();
        wonderName.text = currentBlip.name;
    }

    public void OnSelectWonderButtonClicked()
    {
        selectWonder.SetActive(false);
        taskNumber.text = "Task 01";
        taskText.text = "Swipe The Clouds To Reveal Machu Picchu!";
        tasks.SetActive(true);
    }

    public void OnScratchClicked(Image scratch)
    {
        scratch.gameObject.SetActive(false);
        startTaskButton.gameObject.SetActive(true);
    }

    public void OnStartTaskButtonClicked()
    {
        tasks.SetActive(false);
        GameManager.instance.ChangeCurrentTarget(currentBlip.name + " Task " + currentTask);
    }

    public void StartSecondTask()
    {
        currentTask = 2;
        taskNumber.text = "Task 02";
        taskText.text = "Dig The Artefacts From The Ground!";
        startTaskButton.gameObject.SetActive(false);
        scratch.gameObject.SetActive(true);
        tasks.SetActive(true);
    }

    public void StartThirdTask()
    {
        currentTask = 3;
        taskNumber.text = "Task 03";
        taskText.text = "Solve The Puzzle!";
        startTaskButton.gameObject.SetActive(false);
        scratch.gameObject.SetActive(true);
        tasks.SetActive(true);
    }

}
