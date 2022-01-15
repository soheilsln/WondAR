using System;
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

    public static event Action<int> OnTasksFinished;

    private void Awake()
    {
        splashScreen.gameObject.SetActive(true);
    }

    private void Start()
    {
        Globe.OnBlipsClicked += this.OnBlipsClicked;
        Cloud.OnCloudsDestroyed += this.StartSecondTask;
        Artefact.OnAllArtefactsFound += this.StartThirdTask;
        Puzzle.PuzzleSolved += this.FinishWonder;
    }

    private void OnDestroy()
    {
        Globe.OnBlipsClicked -= this.OnBlipsClicked;
        Cloud.OnCloudsDestroyed -= this.StartSecondTask;
        Artefact.OnAllArtefactsFound -= this.StartThirdTask;
        Puzzle.PuzzleSolved -= this.FinishWonder;
    }

    public void OnScanButtonClicked()
    {
        splashScreen.gameObject.SetActive(false);
        GameManager.instance.SwitchCameras();
    }

    private void OnBlipsClicked(Collider collider)
    {
        currentTask = 1;
        currentBlip = collider.transform.parent.gameObject;
        selectWonder.SetActive(true);
        Text wonderName = selectWonder.GetComponentInChildren<Text>();
        wonderName.text = currentBlip.name;
    }

    public void OnSelectWonderButtonClicked()
    {
        selectWonder.SetActive(false);
        taskNumber.text = "Task 1";
        taskText.text = "The clouds are covering Machu Picchu, slide them apart to reveal it!";
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
        if (currentTask == 0)
        {
            GameManager.instance.ChangeTargetToGlobe();
            if (OnTasksFinished != null)
                OnTasksFinished(GameManager.instance.GetCurrentLevel() + 1);
        }
        else
        {
            GameManager.instance.ChangeCurrentTarget(currentBlip.name + " Task " + currentTask);
            GameManager.instance.ChangeCurrentLevel(currentBlip.name);
        }
    }

    public void StartSecondTask()
    {
        currentTask = 2;
        taskNumber.text = "Task 2";
        taskText.text = "There are some lost treasures, Hooray! Start digging using the pickaxe and collect them all.";
        startTaskButton.gameObject.SetActive(false);
        scratch.gameObject.SetActive(true);
        tasks.SetActive(true);
    }

    public void StartThirdTask()
    {
        currentTask = 3;
        taskNumber.text = "Task 3";
        taskText.text = "Now let us move onto the last and final task. Solve the Puzzle!";
        startTaskButton.gameObject.SetActive(false);
        scratch.gameObject.SetActive(true);
        tasks.SetActive(true);
    }

    public void FinishWonder()
    {
        currentTask = 0;
        taskNumber.text = "Tasks Finished";
        taskText.text = "Congratulations! You have successfully completed Level 1. Flip the book to the first page.";
        startTaskButton.gameObject.SetActive(true);
        scratch.gameObject.SetActive(false);
        tasks.SetActive(true);
    }

}
