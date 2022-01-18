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
        TheColosseumPlayer.ReachedDestination += this.StartSecondTask;
        DoorPuzzle.DoorPuzzleSolved += this.StartThirdTask;
    }

    private void OnDestroy()
    {
        Globe.OnBlipsClicked -= this.OnBlipsClicked;
        Cloud.OnCloudsDestroyed -= this.StartSecondTask;
        Artefact.OnAllArtefactsFound -= this.StartThirdTask;
        Puzzle.PuzzleSolved -= this.FinishWonder;
        TheColosseumPlayer.ReachedDestination -= this.StartSecondTask;
        DoorPuzzle.DoorPuzzleSolved -= this.StartThirdTask;
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
        GameManager.instance.ChangeCurrentLevel(currentBlip.name);
        selectWonder.SetActive(true);
        Text wonderName = selectWonder.GetComponentInChildren<Text>();
        wonderName.text = currentBlip.name;
    }

    public void OnSelectWonderButtonClicked()
    {
        GameManager.instance.PlayAudioClip("Start Task");

        selectWonder.SetActive(false);

        switch(GameManager.instance.GetCurrentLevel())
        {
            case 0:
                taskNumber.text = "Task 1";
                taskText.text = "The clouds are covering Machu Picchu, slide them apart to reveal it!";
                break;
            case 1:
                taskNumber.text = "Task 1";
                taskText.text = "Help the bull to jump from obstacles and reach its destination.";
                break;
            default:
                break;
        }

        
        tasks.SetActive(true);
    }

    public void OnScratchClicked(Image scratch)
    {
        scratch.gameObject.SetActive(false);
        startTaskButton.gameObject.SetActive(true);
    }

    public void OnStartTaskButtonClicked()
    {
        GameManager.instance.PlayAudioClip("Arrow Task");

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
        GameManager.instance.PlayAudioClip("Start Task");

        currentTask = 2;

        switch(GameManager.instance.GetCurrentLevel())
        {
            case 0:
                taskNumber.text = "Task 2";
                taskText.text = "There are some lost treasures, Hooray! Start digging using the pickaxe and collect them all.";
                break;
            case 1:
                taskNumber.text = "Task 2";
                taskText.text = "Find your favourite weapon by opening the doors and solve the puzzle.";
                break;
            default:
                break;
        }

        
        startTaskButton.gameObject.SetActive(false);
        scratch.gameObject.SetActive(true);
        tasks.SetActive(true);
    }

    public void StartThirdTask()
    {
        GameManager.instance.PlayAudioClip("Start Task");

        currentTask = 3;

        switch (GameManager.instance.GetCurrentLevel())
        {
            case 0:
                taskNumber.text = "Task 3";
                taskText.text = "Now let us move onto the last and final task. Solve the Puzzle!";
                break;
            case 1:
                taskNumber.text = "Task 3";
                taskText.text = "Now let us move onto the last and final task. Solve the Puzzle!";
                break;
            default:
                break;
        }

        startTaskButton.gameObject.SetActive(false);
        scratch.gameObject.SetActive(true);
        tasks.SetActive(true);
    }

    public void FinishWonder()
    {
        GameManager.instance.PlayAudioClip("Start Task");

        currentTask = 0;
        taskNumber.text = "Tasks Finished";
        taskText.text = "Congratulations! You have successfully completed Level " + 
            (GameManager.instance.GetCurrentLevel() + 1)  + ". Flip the book to the first page.";
        startTaskButton.gameObject.SetActive(true);
        scratch.gameObject.SetActive(false);
        tasks.SetActive(true);
    }

}
