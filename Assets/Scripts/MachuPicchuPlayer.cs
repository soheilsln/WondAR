using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachuPicchuPlayer : MonoBehaviour
{
    private Animator animator;

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
        Vector3 position = new Vector3(collider.transform.position.x, transform.position.y,
            collider.transform.position.z);
        StartCoroutine(MoveToLocation(position, 3f));
    }

    private IEnumerator MoveToLocation(Vector3 location, float duration)
    {
        float time = 0f;
        Vector3 startPosition = transform.position;

        transform.LookAt(location);
        animator.SetFloat("Walk", 1f);

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, location, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = location;

        animator.SetFloat("Walk", 0f);

        if (ReachedDestination != null)
            ReachedDestination();
    }
}
