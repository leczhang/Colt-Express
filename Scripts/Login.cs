using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Text.RegularExpressions;
using RestSharp; 
using Newtonsoft.Json.Linq;

public class Login : MonoBehaviour {
    public InputField username; // used to store user info
    public InputField password; 
    public Text fText; // used to put the username+password to the screen (for testing purposes)

    public static RestClient client = new RestClient("http://13.72.79.112:4242");
    public static string token;
    // public static string username;
    // public static string password;

    public void VerifyUser(){
        Debug.Log("what?");
        var request = new RestRequest("oauth/token", Method.POST)
            .AddParameter("grant_type", "password")
            .AddParameter("username", username.text)
            .AddParameter("password", password.text)
            .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
        IRestResponse response = client.Execute(request);
        
        try {
            var obj = JObject.Parse(response.Content);
             token = (string)obj["access_token"];
            token = token.Replace("+", "%2B");
            PlayerPrefs.SetString("token", token);
            PlayerPrefs.SetString("username", username.text);
            PlayerPrefs.Save();
            //fText.text = token;
            GoToWR();
        } catch (Exception e) {
            fText.text = "Invalid username or password";
        }
    }

    public void GoToWR(){       
        SceneManager.LoadScene("WaitingRoom");
    }

    public void GoToMM() {
        SceneManager.LoadScene("MainMenu");
    }

}
