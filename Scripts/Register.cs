using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Text.RegularExpressions;
using RestSharp; 
using Newtonsoft.Json.Linq;

public class Register : MonoBehaviour
{
    public InputField username;
    public InputField password;
    public InputField passwordConfirm;
    public Text infoText;
    
    private static RestClient client = new RestClient("http://13.72.79.112:4242");
    private string adminToken;

    // Start is called before the first frame update
    void Start()
    {
        infoText.text = "";
        GetAdminToken();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GetAdminToken() {
        var request = new RestRequest("oauth/token", Method.POST)
            .AddParameter("grant_type", "password")
            .AddParameter("username", "admin")
            .AddParameter("password", "admin")
            .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
            IRestResponse response = client.Execute(request);
            
            var obj = JObject.Parse(response.Content);
            adminToken = (string)obj["access_token"];
            adminToken = adminToken.Replace("+", "%2B");
    }

    public void VerifyNewUser(){
        if(password.text != passwordConfirm.text) {
            infoText.text = "Your passwords do not match.";
        } else {
            dynamic j = new JObject();
            j.name = username.text;
            j.password = password.text;
            j.preferredColour = "01FFFF";
            j.role = "ROLE_PLAYER";

            var request = new RestRequest("api/users/" + username.text + "?access_token=" + adminToken, Method.PUT)
                .AddParameter("application/json", j.ToString(), ParameterType.RequestBody)
                .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
            IRestResponse response = client.Execute(request);
            
            string resp = response.Content;
            if(resp != "Player added.") {
                infoText.text = resp;
            } else {
                GoToMM();
            }
        }
    }

    public void GoToMM() {
        SceneManager.LoadScene("MainMenu");
    }
}
