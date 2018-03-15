using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_FunctionManager : MonoBehaviour
{

	// Use this for initialization
	

    public void ExitGame()
    {        
        Application.Quit();
    }

    public void ChangeScene(int index)
    {
        SceneManager.LoadScene(index);
    }
    


}
