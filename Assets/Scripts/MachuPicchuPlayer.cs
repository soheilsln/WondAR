using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachuPicchuPlayer : MonoBehaviour
{
    private Animator animator;
    private GameObject destination;

    public static event Action ReachedDestination;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Artefact.OnGoToClicked += GoToPosition;
    }

    private void OnDestroy()
    {
        Artefact.OnGoToClicked -= GoToPosition;
    }

    private void GoToPosition(Collider collider)
    {
        Vector3 location = new Vector3(collider.transform.position.x, transform.position.y,
            collider.transform.position.z);
        Vector3 moveVector = location - transform.position;
        location = transform.position + 0.7f * moveVector; // moving close to the location
        destination = new GameObject();
        destination.transform.position = location;
        destination.transform.parent = this.transform.parent;
        StartCoroutine(MoveToLocation(destination.transform, 2f));
    }

    private IEnumerator MoveToLocation(Transform location, float duration)
    {
        float time = 0f;
        Vector3 startPosition = transform.localPosition;

        transform.LookAt(location.position);
        animator.SetFloat("Walk", 1f);

        while (time < duration)
        {
            transform.localPosition = Vector3.Lerp(startPosition, location.localPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = location.localPosition;
        Destroy(destination);

        animator.SetFloat("Walk", 0f);

        if (ReachedDestination != null)
            ReachedDestination();
    }
}
