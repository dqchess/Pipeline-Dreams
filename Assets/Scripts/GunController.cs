﻿using UnityEngine;
using System.Collections;


	public class GunController : MonoBehaviour {

		public GunDefinition[] guns;
		public int selectedGun;
        
        public bool trigger = false; 

        [System.Serializable]
		public class GunDefinition
		{
			public ArcReactor_Launcher launcher;
			public float rechargeTime;
			public float currentRechargeTimeLeft;
			public bool continuous;
			public KeyCode keycode;
		}

		void Start ()
		{
			//Cursor.lockState = CursorLockMode.Locked;
			//Cursor.visible = false;
		}

		// Update is called once per frame
		void Update () 
		{
        if (trigger) {
            guns[4].launcher.LaunchRay();
            
            trigger = false;
        }
    }
	}
