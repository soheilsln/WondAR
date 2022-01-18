using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Cameras")]
    public Camera mainCamera;
    public Camera ARCamera;

    [Header("Audios")]
    public List<AudioClip> audios;

    private ImageTargetBehaviour globe;
    private List<ImageTargetBehaviour> targets;
    private AudioSource audioSourse;
    private Dictionary<string, AudioClip> audiosDic;

    private string currentTarget;
    private int currentLevel;

    [HideInInspector]
    public int currentScreenShot = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        currentLevel = 0;

        globe = ARCamera.transform.GetChild(0).GetComponent<ImageTargetBehaviour>();
        targets = new List<ImageTargetBehaviour>();
        foreach(Transform target in ARCamera.transform)
        {
            targets.Add(target.GetComponent<ImageTargetBehaviour>());
        }
        targets.Remove(targets[0]);

        audioSourse = GetComponent<AudioSource>();
        audiosDic = new Dictionary<string, AudioClip>();
        foreach(AudioClip audio in audios)
        {
            audiosDic.Add(audio.name, audio);
        }
    }

    public void SwitchCameras()
    {
        if (mainCamera.gameObject.activeSelf)
        {
            mainCamera.gameObject.SetActive(false);
            ARCamera.gameObject.SetActive(true);
        }
        else if (ARCamera.gameObject.activeSelf)
        {
            ARCamera.gameObject.SetActive(false);
            mainCamera.gameObject.SetActive(true);
        }
    }

    public void ChangeCurrentTarget(string wonderName)
    {
        currentTarget = wonderName;
        globe.gameObject.SetActive(false);

        foreach (ImageTargetBehaviour target in targets)
        {
            if (target.gameObject.name == currentTarget)
            {
                target.gameObject.SetActive(true);
            }
            else
            {
                target.gameObject.SetActive(false);
            }
        }
    }

    public void ChangeCurrentLevel(string wonderName)
    {
        switch (wonderName)
        {
            case "MACHU PICCHU":
                currentLevel = 0;
                break;
            case "THE COLOSSEUM":
                currentLevel = 1;
                break;
            case "THE GREAT WALL OF CHINA":
                currentLevel = 2;
                break;
            case "PYRAMID OF GIZA":
                currentLevel = 3;
                break;
            case "CHICHEN ITZA":
                currentLevel = 4;
                break;
            case "CHRIST THE REDEEMER":
                currentLevel = 5;
                break;
            case "PETRA":
                currentLevel = 6;
                break;
            case "TAJ MAHAL":
                currentLevel = 7;
                break;
            default:
                break;
        }
    }

    public void ChangeTargetToGlobe()
    {
        foreach (ImageTargetBehaviour target in targets)
        {
            target.gameObject.SetActive(false);
        }
        globe.gameObject.SetActive(true);
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public void PlayAudioClip(string audioName)
    {
        audioSourse.clip = audiosDic[audioName];
        audioSourse.Play();
    }

}
