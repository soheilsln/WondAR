using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float dragSpeed = 1000f;

    private bool isDragBegin = false;
    private Collider currentCloud;
    private float startingTouchPosition;
    private bool isCloudsDestroyed = false;

    public static event Action OnCloudsDestroyed;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Drag();
        }
    }

    private void Drag()
    {
        Touch touch = Input.GetTouch(0);

        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray raycast = GameManager.instance.ARCamera.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                if (raycastHit.collider.CompareTag("Cloud"))
                {
                    isDragBegin = true;
                    currentCloud = raycastHit.collider;
                    startingTouchPosition = touch.position.x;
                }
            }
        }
        else if (Input.GetTouch(0).phase == TouchPhase.Moved && isDragBegin)
        {
            if (startingTouchPosition > touch.position.x)
            {
                currentCloud.GetComponent<Rigidbody>().velocity = new Vector3(-dragSpeed, 0, 0);
            }
            else if (startingTouchPosition < touch.position.x)
            {
                currentCloud.GetComponent<Rigidbody>().velocity = new Vector3(dragSpeed, 0, 0);
            }
            StartCoroutine(DestroyCloud(currentCloud.gameObject));
        }
        else if (Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            isDragBegin = false;
            currentCloud = null;
        }
    }

    private IEnumerator DestroyCloud(GameObject cloud)
    {
        yield return new WaitForSeconds(2f);
        Destroy(cloud);

        if (transform.childCount == 0 && !isCloudsDestroyed)
        {
            if (OnCloudsDestroyed != null)
            {
                OnCloudsDestroyed();
                isCloudsDestroyed = true;
            }
        }
    }
}
