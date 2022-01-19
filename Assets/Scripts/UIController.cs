using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject splashScreen;
    public GameObject selectWonder;
    public GameObject FlipTheBook;

    [Header("Tasks")]
    public GameObject tasks;
    public Button startTaskButton;
    public Text taskNumber;
    public Text taskText;
    public Image scratch;
    public Image character;

    [Header("Task Information")]
    public GameObject taskInformation;
    public Text taskInformationText;
    public Image characterInformation;

    [Header("Characters")]
    public Sprite[] characters;

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
        Cloud.OnCloudsDestroyed += this.ShowTaskInformation;
        Artefact.OnAllArtefactsFound += this.ShowTaskInformation;
        Puzzle.PuzzleSolved += this.ShowTaskInformation;
        TheColosseumPlayer.ReachedDestination += this.ShowTaskInformation;
        DoorPuzzle.DoorPuzzleSolved += this.ShowTaskInformation;
        EnjoyMonument.ContinueClicked += BackToGlobe;
    }

    private void OnDestroy()
    {
        Globe.OnBlipsClicked -= this.OnBlipsClicked;
        Cloud.OnCloudsDestroyed -= this.ShowTaskInformation;
        Artefact.OnAllArtefactsFound -= this.ShowTaskInformation;
        Puzzle.PuzzleSolved -= this.ShowTaskInformation;
        TheColosseumPlayer.ReachedDestination -= this.ShowTaskInformation;
        DoorPuzzle.DoorPuzzleSolved -= this.ShowTaskInformation;
        EnjoyMonument.ContinueClicked -= BackToGlobe;
    }

    public void OnScanButtonClicked()
    {
        splashScreen.gameObject.SetActive(false);
        GameManager.instance.SwitchCameras();
    }

    private void OnBlipsClicked(Collider collider)
    {
        currentBlip = collider.transform.parent.gameObject;
        GameManager.instance.ChangeCurrentLevel(currentBlip.name);

        currentTask = 1;
        selectWonder.SetActive(true);
        Text wonderName = selectWonder.GetComponentInChildren<Text>();
        wonderName.text = currentBlip.name;

        if (GameManager.instance.GetCurrentLevel() < 2)
        {
            character.gameObject.SetActive(true);
            characterInformation.gameObject.SetActive(true);
            character.sprite = characters[GameManager.instance.GetCurrentLevel()];
            characterInformation.sprite = characters[GameManager.instance.GetCurrentLevel()];
        }
        else
        {
            character.gameObject.SetActive(false);
            characterInformation.gameObject.SetActive(false);
        }
    }

    public void OnSelectWonderButtonClicked()
    {
        GameManager.instance.PlayAudioClip("Start Task");

        selectWonder.SetActive(false);

        switch (GameManager.instance.GetCurrentLevel())
        {
            case 0:
                taskNumber.text = "Task 1";
                taskText.text = "The clouds are covering Machu Picchu, slide them apart to reveal it!";
                break;
            case 1:
                taskNumber.text = "Task 1";
                taskText.text = "Help the bull to jump the hurdles and reach its destination.";
                break;
            default:
                FinishWonder();
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
        StartCoroutine(ShowFlipTheBook());
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

    public void ShowTaskInformation()
    {
        taskInformation.SetActive(true);

        switch (GameManager.instance.GetCurrentLevel())
        {
            case 0:
                if (currentTask == 1)
                    taskInformationText.text = "1.\tMachu Picchu is located deep in the beautiful cloud forest The cloud " +
                        "forest has a large variety of plants, insects, animals, as well as over 300 species of orchids.\n" +
                        "2.\tBuilt under the Inca Empire, it was a beautiful city that was abandoned hundred years later.";
                else if (currentTask == 2)
                    taskInformationText.text = "1.\tMachu Picchu was hidden and undiscovered for centuries, making it one of" +
                        " the best-preserved Inca settlements and an archaeological treasure.\n2.\tA lot of hidden treasures" +
                        " have been dug up from the sites especially ceramic vessels, silver statues, jewellery, and human" +
                        " bones.";
                else if (currentTask == 3)
                    taskInformationText.text = "1.\tIn the Quechua Peru language, “Machu Picchu” means “Old Peak” or “Old" +
                        " Mountain.”\n2.\tThe city is almost 8000 feet above sea level.\n3.\tMachu Picchu is made up of more" +
                        " than 150 buildings ranging from baths and houses to temples and sanctuaries.";
                break;
            case 1:
                if (currentTask == 1)
                    taskInformationText.text = "1.\tThe Colosseum is in Rome, Italy and is an ancient amphitheatre that was" +
                        " built by the great emperor Titus Vespasian.\n2.\tAn amphitheatre is an open circular or oval " +
                        "building with a space in the centre surrounded by a lot of seats for spectators, to watch " +
                        "sporting events.";
                else if (currentTask == 2)
                    taskInformationText.text = "1.\tSome people were given special seating including senators. To become a" +
                        " Roman Senate, a person had to meet high criteria; this included being rich, more than 30 years" +
                        " of age and having served in a very high position of authority.";
                else if (currentTask == 3)
                    taskInformationText.text = "1.\tMany types of events were held in the Colosseum. In the early years, " +
                        "Romans filled the amphitheatre with water and staged sea battles and re-enactments of victorious " +
                        "sea battles. Wild animals were featured in both gladiatorial matches and in staged hunts.";
                break;
            default:
                break;
        }
    }

    public void OnContinueButtonTaskInformationClicked()
    {
        taskInformation.SetActive(false);

        if (currentTask == 1)
            StartSecondTask();
        else if (currentTask == 2)
            StartThirdTask();
        else if (currentTask == 3)
            FinishWonder();
    }

    public void StartSecondTask()
    {
        GameManager.instance.PlayAudioClip("Start Task");

        currentTask = 2;

        switch (GameManager.instance.GetCurrentLevel())
        {
            case 0:
                taskNumber.text = "Task 2";
                taskText.text = "There are some lost treasures, Hooray! Start digging using the pickaxe and collect them all.";
                break;
            case 1:
                taskNumber.text = "Task 2";
                taskText.text = "Let us see how well you memorize. Find your favourite weapon by opening the doors and solve" +
                    " the puzzle.";
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
                taskText.text = "Get ready for the battle. Let us move onto the last and final task. Suit up the knight!";
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

        currentTask = 4;
        taskNumber.text = "Tasks Finished";
        taskText.text = "Congratulations! You have successfully completed Level " +
            (GameManager.instance.GetCurrentLevel() + 1) + ". You can now enjoy the monument.";
        startTaskButton.gameObject.SetActive(true);
        scratch.gameObject.SetActive(false);
        tasks.SetActive(true);
    }

    public void BackToGlobe()
    {
        GameManager.instance.PlayAudioClip("Start Task");

        currentTask = 0;
        taskNumber.text = "Back To Globe";
        taskText.text = "Flip to the first page.";
        startTaskButton.gameObject.SetActive(true);
        scratch.gameObject.SetActive(false);
        tasks.SetActive(true);
    }

    private IEnumerator ShowFlipTheBook()
    {
        FlipTheBook.SetActive(true);
        yield return new WaitForSeconds(3f);
        FlipTheBook.SetActive(false);
    }
}
