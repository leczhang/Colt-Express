using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour{
    public void Host(){
        SceneManager.LoadScene("HostGame"); // load all scenes named "HostGame"
    }

    public void Join(){
        SceneManager.LoadScene("JoinGame"); // load all scenes named "JoinGame"
    }

    public void Quit(){
        Application.Quit(); // quit the game
    }
}

