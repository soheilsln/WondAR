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

    public int ChangeCurrentLevel(string wonderName)
    {
        switch (wonderName)
        {
            case "MACHU PICCHU":
                return 0;
            case "THE COLOSSEUM":
                return 1;
            case "THE GREAT WALL OF CHINA":
                return 2;
            case "PYRAMID OF GIZA":
                return 3;
            case "CHICHEN ITZA":
                return 4;
            case "CHRIST THE REDEEMER":
                return 5;
            case "PETRA":
                return 6;
            case "TAJ MAHAL":
                return 7;
            default:
                return 0;
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
