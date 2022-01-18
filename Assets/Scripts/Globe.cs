using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Globe : MonoBehaviour
{
    public float rotattionSpeed = 100f;
    public Transform canvas;

    private float startingTouchPosition;

    public static event Action<Collider> OnBlipsClicked;

    public Sprite[] blipsSprites;

    private void Start()
    {
        UIController.OnTasksFinished += UpdateBlips;
    }

    private void OnDestroy()
    {
        UIController.OnTasksFinished -= UpdateBlips;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            RotateOnTouch();
            OnClick();
        }

        CanvasLookAtCamera();
    }

    private void RotateOnTouch()
    {

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            startingTouchPosition = touch.position.x;
        }
        else if (touch.phase == TouchPhase.Moved)
        {
            if (startingTouchPosition > touch.position.x)
            {
                transform.Rotate(Vector3.up, rotattionSpeed * Time.deltaTime);
            }
            else if (startingTouchPosition < touch.position.x)
            {
                transform.Rotate(Vector3.up, -rotattionSpeed * Time.deltaTime);
            }
        }

    }

    private void CanvasLookAtCamera()
    {
        foreach (Transform child in canvas)
        {
            child.transform.LookAt(GameManager.instance.ARCamera.transform);
        }
    }

    private void OnClick()
    {
        if(Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray raycast = GameManager.instance.ARCamera.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                if (raycastHit.collider.CompareTag("Blip"))
                {
                    if (OnBlipsClicked != null)
                        OnBlipsClicked(raycastHit.collider);
                    GameManager.instance.PlayAudioClip("Map Click");
                }
            }
        }
    }

    private void UpdateBlips(int level)
    {
        if (level < blipsSprites.Length)
        {
            canvas.GetChild(level).GetComponent<Image>().sprite = blipsSprites[level];
            canvas.GetChild(level).GetChild(0).gameObject.SetActive(true);
        }
    }

}
