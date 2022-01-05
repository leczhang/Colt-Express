using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour{
    public void Login(){
        SceneManager.LoadScene("Login"); // load all scenes named "Game"
    }

    public void Register(){
        SceneManager.LoadScene("Register"); // load all scenes named "Game"
    }

    public void QuitGame(){
        Application.Quit(); // quit the game
    }
}