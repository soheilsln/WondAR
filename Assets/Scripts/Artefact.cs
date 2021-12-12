using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Artefact : MonoBehaviour
{
    private Transform currentArtefact;

    public static event Action<Collider> OnGoToClicked;

    private void Start()
    {
        MachuPicchuPlayer.ReachedDestination += EnableDigButton;
    }

    private void OnDestroy()
    {
        MachuPicchuPlayer.ReachedDestination -= EnableDigButton;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            OnClick();
        }

        ButtonsLookAtCamera();
    }

    private void ButtonsLookAtCamera()
    {
        foreach (Image button in GetComponentsInChildren<Image>())
        {
            button.transform.LookAt(GameManager.instance.ARCamera.transform);
        }
    }

    private void OnClick()
    {
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray raycast = GameManager.instance.ARCamera.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                if (raycastHit.collider.CompareTag("WorldButton"))
                {
                    if (raycastHit.collider.name == "GoToButton")
                    {
                        currentArtefact = raycastHit.transform.parent;
                        if (OnGoToClicked != null)
                            OnGoToClicked(raycastHit.collider);
                    }
                    else if(raycastHit.collider.name == "DigButton")
                    {

                    }
                }
            }
        }
    }

    private void EnableDigButton()
    {
        if(currentArtefact != null)
        {
            currentArtefact.GetChild(0).gameObject.SetActive(false);
            currentArtefact.GetChild(1).gameObject.SetActive(true);
        }
    }

}
