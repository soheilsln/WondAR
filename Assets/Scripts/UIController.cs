using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject splashScreen;
    public GameObject selectWonder;
    public GameObject tasks;
    public Button startTaskButton;

    private GameObject currentBlip;

    private void Awake()
    {
        splashScreen.gameObject.SetActive(true);
    }

    private void Start()
    {
        Globe.OnBlipsClicked += this.OnBlipsClicked;
        Cloud.OnCloudsDestroyed += this.StartSecondTask;
    }

    private void OnDestroy()
    {
        Globe.OnBlipsClicked -= this.OnBlipsClicked;
        Cloud.OnCloudsDestroyed -= this.StartSecondTask;
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
        GameManager.instance.ChangeCurrentWonder(currentBlip.name);
    }

    public void StartSecondTask()
    {
        Debug.Log("Clouds Finished");
    }

}
