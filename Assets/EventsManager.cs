using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    public static event Action<List<string>> OnGenerateWords;
    //public static event Action<Building> OnBuildingFinishedEvent;

    public static event Action OnDebugEvent;

    private void Update()
    {
        HandleOnDebugEvent();
    }

    private void HandleOnDebugEvent()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnDebugEvent?.Invoke();
        }
    }

    public static void GenerateWordsEvent(List<string> words)
    {
        OnGenerateWords?.Invoke(words);
    }

    //internal static void BuildingFinishedEvent(Building building)
    //{
    //    OnBuildingFinishedEvent?.Invoke(building);
    //}
}
