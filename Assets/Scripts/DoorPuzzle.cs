using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorPuzzle : MonoBehaviour
{

    private bool showedAnswers = false;
    private bool[] isOpened;
    private bool[] isSolved;
    private List<Transform> doors;
    private int[] answers;

    private void Start()
    {
        isOpened = new bool[6] { false, false, false, false, false, false };
        isSolved = new bool[6] { false, false, false, false, false, false };
        answers = new int[6] { 4, 5, 3, 2, 0, 1 };
        doors = new List<Transform>();
        foreach (Transform door in transform)
        {
            if (door.name.Contains("ClosedDoor"))
            {
                doors.Add(door);
            }
        }
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            StartCoroutine(OnClick());
        }

        ButtonsLookAtCamera();

    }

    private IEnumerator OnClick()
    {
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray raycast = GameManager.instance.ARCamera.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                if (raycastHit.collider.CompareTag("WorldButton"))
                {
                    if (showedAnswers && raycastHit.collider.name.Contains("ClosedDoor"))
                    {
                        for (int i = 0; i < doors.Count; i++)
                        {
                            if (!isOpened[i] && raycastHit.transform.name.EndsWith((i + 1).ToString()))
                            {
                                doors[i].GetChild(1).GetComponent<Animator>().SetTrigger("Open");
                                yield return new WaitForSeconds(1f);
                                doors[i].GetChild(2).gameObject.SetActive(true);
                                isOpened[i] = true;
                                StartCoroutine(CheckAnswer(i));
                            }
                        }
                    }
                    else if (raycastHit.collider.name == "Start")
                    {
                        raycastHit.transform.gameObject.SetActive(false);
                        StartCoroutine(ShowAnswers());
                    }
                }
            }
        }
    }

    private IEnumerator ShowAnswers()
    {
        foreach (Transform door in doors)
        {
            door.GetChild(1).GetComponent<Animator>().SetTrigger("Open");
        }
        yield return new WaitForSeconds(1f);
        foreach (Transform door in doors)
        {
            door.GetChild(2).gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(5f);
        foreach (Transform door in doors)
        {
            door.GetChild(1).GetComponent<Animator>().SetTrigger("Close");
        }
        yield return new WaitForSeconds(0.4f);
        foreach (Transform door in doors)
        {
            door.GetChild(2).gameObject.SetActive(false);
        }
        showedAnswers = true;
    }

    private void ButtonsLookAtCamera()
    {
        transform.GetChild(0).LookAt(GameManager.instance.ARCamera.transform);
    }

    private IEnumerator CheckAnswer(int doorNumber)
    {
        int openedCount = 0;
        for (int i = 0; i < isOpened.Length; i++)
        {
            if (isOpened[i])
                openedCount++;
        }

        if (openedCount % 2 == 0 && !isSolved[doorNumber])
        {
            if (isOpened[answers[doorNumber]])
            {
                isSolved[doorNumber] = true;
                isSolved[answers[doorNumber]] = true;
                yield return new WaitForSeconds(0.7f);
                doors[doorNumber].GetChild(0).GetComponent<Image>().color = Color.green;
                doors[answers[doorNumber]].GetChild(0).GetComponent<Image>().color = Color.green;
            }
            else
            {
                for (int i = 0; i < isOpened.Length; i++)
                {
                    if (!isSolved[i] && isOpened[i])
                    {
                        yield return new WaitForSeconds(1f);
                        doors[i].GetChild(1).GetComponent<Animator>().SetTrigger("Close");
                        yield return new WaitForSeconds(0.4f);
                        doors[i].GetChild(2).gameObject.SetActive(false);
                        isOpened[i] = false;
                    }
                }
            }
        }
    }

}
