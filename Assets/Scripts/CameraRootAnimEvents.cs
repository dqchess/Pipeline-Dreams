﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraRootAnimEvents : MonoBehaviour
{
    public event Action OnCameraWarp;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void InvokeOnCameraWarp() {
        OnCameraWarp();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
