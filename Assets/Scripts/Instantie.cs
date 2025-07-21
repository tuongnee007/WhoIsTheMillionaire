using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Instantie : MonoBehaviour
{
    public GameObject timerPrefab;
    private void Start()
    {
        if (CountDownTimer.Instance == null)
        {
            Instantiate(timerPrefab);
        }
    }
}
