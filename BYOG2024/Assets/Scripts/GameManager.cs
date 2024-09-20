using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] 
    private QuickTimeEvent qte;

    private void Start()
    {
        qte.StartQTE();
        qte.onQTECompleted += () =>
        {
            Debug.Log("COMPLETED");
        };
    }
}
