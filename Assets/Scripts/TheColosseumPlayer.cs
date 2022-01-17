using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TheColosseumPlayer : MonoBehaviour
{
    private Animator animator;
    private GameObject destination;
    private Vector3 startLocalPosition;
    private bool hitWall = false;
    private bool isJumping = false;
    private int currentWall = 0;

    public GameObject walls;
    public GameObject canvas;

    public static event Action ReachedDestination;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        startLocalPosition = transform.localPosition;
    }

    private void OnDestroy()
    {

    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            OnClick();
        }

        ButtonsLookAtCamera();
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
                    if (raycastHit.collider.name == "StartButton")
                    {
                        StartRun();
                        raycastHit.collider.gameObject.SetActive(false);
                        canvas.transform.GetChild(currentWall + 1).gameObject.SetActive(true);
                    }
                    else if (raycastHit.collider.name == "JumpButton")
                    {
                        isJumping = true;
                        raycastHit.collider.gameObject.SetActive(false);
                        animator.SetTrigger("Jump");
                    }
                }
            }
        }
    }

    private void StartRun()
    {
        GoToPosition(walls.transform.GetChild(currentWall).GetComponent<Collider>());
    }

    private void GoToPosition(Collider collider)
    {
        Vector3 location = new Vector3(collider.transform.position.x, transform.position.y,
            collider.transform.position.z);
        destination = new GameObject();
        destination.transform.position = location;
        destination.transform.parent = this.transform.parent;
        StartCoroutine(MoveToLocation(destination.transform, 1.5f));
    }

    private IEnumerator MoveToLocation(Transform location, float duration)
    {
        float time = 0f;
        Vector3 startPosition = transform.localPosition;

        transform.LookAt(location.position);
        animator.SetFloat("Walk", 1f);
        animator.SetFloat("Run", 1f);

        while (time < duration && !hitWall)
        {
            transform.localPosition = Vector3.Lerp(startPosition, location.localPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        Destroy(destination);
        if (!hitWall)
        {
            transform.localPosition = location.localPosition;

            animator.SetFloat("Run", 0f);
            animator.SetFloat("Walk", 0f);

            currentWall++;
            if (currentWall < (canvas.transform.childCount - 1))
            {
                canvas.transform.GetChild(currentWall + 1).gameObject.SetActive(true);
            }

            if (currentWall < canvas.transform.childCount)
                StartRun();

            if (currentWall == canvas.transform.childCount)
            {
                if (ReachedDestination != null)
                    ReachedDestination();
            }
        }
    }

    private void ButtonsLookAtCamera()
    {
        foreach (Image button in canvas.GetComponentsInChildren<Image>())
        {
            button.transform.LookAt(GameManager.instance.ARCamera.transform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Wall"))
        {
            if (!isJumping)
                StartCoroutine(ResetPlayer());
            else
                isJumping = false;
        }
    }

    private IEnumerator ResetPlayer()
    {
        hitWall = true;
        currentWall = 0;
        animator.SetFloat("Run", 0f);
        animator.SetFloat("Walk", 0f);
        transform.localPosition = startLocalPosition;

        canvas.transform.GetChild(0).gameObject.SetActive(true);
        for (int i = 1; i < canvas.transform.childCount; i++)
        {
            canvas.transform.GetChild(i).gameObject.SetActive(false);
        }

        yield return new WaitForEndOfFrame();
        hitWall = false;
    }

}
