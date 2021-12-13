using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Puzzle : MonoBehaviour
{
    public float speed = 0.5f;

    private bool isDragBegin = false;
    private Collider currentPiece;
    private List<Transform> pieceLocations;
    private List<Transform> pieces;

    public static event Action PuzzleSolved;

    private void Start()
    {
        pieceLocations = new List<Transform>();
        pieces = new List<Transform>();
        foreach (Transform child in transform.GetChild(0))
        {
            pieceLocations.Add(child);
        }
        for (int i = 1; i < transform.childCount; i++)
        {
            pieces.Add(transform.GetChild(i));
        }
    }

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
                if (raycastHit.collider.CompareTag("Puzzle"))
                {
                    isDragBegin = true;
                    currentPiece = raycastHit.collider;
                }
            }
        }
        else if (Input.GetTouch(0).phase == TouchPhase.Moved && isDragBegin)
        {
            currentPiece.transform.position = new Vector3(currentPiece.transform.position.x + touch.deltaPosition.x * speed,
                currentPiece.transform.position.y, currentPiece.transform.position.z + touch.deltaPosition.y * speed);
        }
        else if (Input.GetTouch(0).phase == TouchPhase.Ended && isDragBegin)
        {
            isDragBegin = false;
            AttachToPuzzle();
            currentPiece = null;
            SolveCheck();
        }
    }

    private void AttachToPuzzle()
    {
        foreach (Transform location in pieceLocations)
        {
            if (Vector3.Distance(location.position, currentPiece.transform.position) < 60f)
            {
                currentPiece.transform.position = location.position;
                return;
            }
        }
    }

    private void SolveCheck()
    {
        int solved = 0;
        for (int i = 0; i < pieces.Count; i++)
        {
            if(Vector3.Distance(pieces[i].position,pieceLocations[i].position) < 1f)
            {
                solved++;
            }
        }

        if(solved == pieces.Count)
        {
            if (PuzzleSolved != null)
                PuzzleSolved();
        }
    }
}
