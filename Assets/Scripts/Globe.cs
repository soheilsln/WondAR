using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globe : MonoBehaviour
{
    public float rotattionSpeed = 100f;
    public Transform canvas;

    private float startingTouchPosition;

    private void Update()
    {
        if (Input.touchCount > 0)
            RotateOnTouch();
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
                transform.Rotate(Vector3.up, -rotattionSpeed * Time.deltaTime);
            }
            else if (startingTouchPosition < touch.position.x)
            {
                transform.Rotate(Vector3.up, rotattionSpeed * Time.deltaTime);
            }
        }

        foreach (Transform child in canvas)
        {
            child.transform.LookAt(GameManager.instance.ARCamera.transform);
        }

    }

}
