﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Control : MonoBehaviour {

    public static bool isPaused=false;
    public GameObject pausePanel;


	// Use this for initialization
	void Start () {
		
	}
	
    

	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
	}

    public void TogglePause()
    {        

        pausePanel.SetActive(!isPaused);
        isPaused = !isPaused; 
        Time.timeScale = isPaused==false ? 1:0;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    
}