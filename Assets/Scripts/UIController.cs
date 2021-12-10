using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject splashScreen;
    public GameObject selectWonder;
    public GameObject tasks;

    private GameObject currentBlip;

    private void Awake()
    {
        splashScreen.gameObject.SetActive(true);
    }

    private void Start()
    {
        Globe.OnBlipsClicked += this.OnBlipsClicked;
    }

    private void OnDestroy()
    {
        Globe.OnBlipsClicked -= this.OnBlipsClicked;
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
        GameManager.instance.ChangeCurrentWonder(currentBlip.name);
        selectWonder.SetActive(false);
    }

}
