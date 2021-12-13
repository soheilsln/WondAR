using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Artefact : MonoBehaviour
{
    public int totalDigNumber = 3;

    private Transform currentArtefact;
    private GameObject currentArtefactObject;
    private List<GameObject> artefacts;
    private int currentDigNumber = 0;
    private int currentArtefactNumber = 0;

    public static event Action<Collider> OnGoToClicked;
    public static event Action OnAllArtefactsFound;

    private void Start()
    {
        MachuPicchuPlayer.ReachedDestination += EnableDigButton;
        currentDigNumber = 0;
        currentArtefactNumber = 0;

        artefacts = new List<GameObject>();
        foreach (Transform child in transform)
        {
            artefacts.Add(child.gameObject);
        }
        currentArtefact = artefacts[currentArtefactNumber].transform;
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
                        if (OnGoToClicked != null)
                            OnGoToClicked(raycastHit.collider);
                        raycastHit.collider.gameObject.SetActive(false);
                    }
                    else if (raycastHit.collider.name == "DigButton")
                    {
                        if (currentArtefactObject != null)
                        {
                            currentArtefactObject.GetComponentInChildren<ParticleSystem>().Play();
                            currentDigNumber++;
                        }

                        if (currentDigNumber == totalDigNumber)
                        {
                            currentArtefact.GetChild(1).gameObject.SetActive(false);
                            currentArtefactObject.GetComponent<MeshRenderer>().enabled = true;
                            currentDigNumber = 0;
                            currentArtefactNumber++;

                            if (currentArtefactNumber < artefacts.Count)
                            {
                                EnableGoToButton();
                            }
                            else
                            {
                                if (OnAllArtefactsFound != null)
                                    OnAllArtefactsFound();
                            }
                        }
                    }
                }
            }
        }
    }

    private void EnableDigButton()
    {
        if (currentArtefact != null)
        {
            currentArtefact.GetChild(1).gameObject.SetActive(true);
            currentArtefactObject = currentArtefact.GetChild(2).gameObject;
            currentArtefactObject.GetComponent<MeshRenderer>().enabled = false;
            currentArtefactObject.SetActive(true);
        }
    }

    private void EnableGoToButton()
    {
        currentArtefact = artefacts[currentArtefactNumber].transform;
        currentArtefact.GetChild(0).gameObject.SetActive(true);
    }



}
