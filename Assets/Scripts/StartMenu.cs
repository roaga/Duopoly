using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void Play1PlayerGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        StaticData.twoPlayers = false;

    }
    public void Play2PlayerGame(){
        SceneManager.LoadScene("2-PlayerInstruct");
        StaticData.twoPlayers = true;
        
    }

    public void QuitGame(){
        Debug.Log("Quit application");
        Application.Quit();
    }
}
