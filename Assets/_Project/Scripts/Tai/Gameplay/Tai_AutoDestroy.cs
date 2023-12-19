using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tai_AutoDestroy : MonoBehaviour
{
    public float timerDestroy = 0.5f;

    private void Start()
    {
        Destroy(gameObject, timerDestroy);
    }
}
