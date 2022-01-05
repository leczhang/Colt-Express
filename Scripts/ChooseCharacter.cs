using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using RestSharp;
using Newtonsoft.Json.Linq;
using System.Linq;


using Sfs2X;
using Sfs2X.Logging;
using Sfs2X.Util;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using System.Reflection;
using Sfs2X.Protocol.Serialization;

using model;

public class ChooseCharacter : MonoBehaviour
{

    /*static ChooseCharacter ccInstance;
    public static ChooseCharacter Instance
    {
        get
        {
            if (ccInstance == null){
                GameObject go = new GameObject();
                mInstance = go.AddComponent<ChooseCharacter>();
            }
            return ccInstance;
        }
    }*/


    // debugging variables
    // public Text selected;
    // public static string debugText;
    //public Button button;

    public Text display;
    public Text info;

    private static bool alreadyCalled;

    private static RestClient client = new RestClient("http://13.72.79.112:4242");

    //SAVE THE CHOSEN CHARACTER IN THIS STRING SO IT CAN BE USED BY GAMEMANAGER
    public static string character;

    // BOOLEANS FOR CHARACTER AVAILABILITY
    public bool BelleIsAvailable; 
    public bool CheyenneIsAvailable; 
    public bool TucoIsAvailable; 
    public bool DjangoIsAvailable; 
    public bool DocIsAvailable; 
    public bool GhostIsAvailable;

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(800, 600, true);
        // testing 
            //  var foundButtonObjects = FindObjectsOfType<Button>();
            //     foreach(Button btn in foundButtonObjects){
            //         if(btn.name == "TucoBtn"){
            //             btn.interactable = false; 
            //         }
            //        //  btn.interactable = false; 
            //     }
        //////
        character = "";
        alreadyCalled = false;

        info.text = "You will be brought to the game once all " + WaitingRoom.numPlayers + " players have chosen a character!";
        display.text = "";

        BelleIsAvailable = true;
        CheyenneIsAvailable = true; 
        TucoIsAvailable = true; 
        DjangoIsAvailable = true; 
        DocIsAvailable = true; 

        SFS.setChooseCharacter();

        // rend = GetComponent<Renderer>();
        // name = this.GameObject;
        //debugText = "";
        // selected.text = "";

        // Initialize SFS2X client. This can be done in an earlier scene instead
		/*SmartFox sfs = new SmartFox();
        // For C# serialization
		DefaultSFSDataSerializer.RunningAssembly = Assembly.GetExecutingAssembly();
        SFS.setSFS(sfs);

        SFS.Connect("teseegst");*/
    }

    // Update is called once per frame
    void Update()
    {
        if (SFS.IsConnected() && !alreadyCalled) {
            alreadyCalled = true;
            EnterChooseCharacterScene();// IF RUNNING TESTS STARTING FROM THIS SCENE,
                                        // MUST DELAY CALLING OF THIS METHOD BY 1-2 SECONDS
            /*
            THIS LISTENER CAN BE REMOVED ONCE THE CHARACTERS THEMSELVES CAN BE CLICKED
            */
            // button.onClick.AddListener(CharacterChoice);
        }
        if (SFS.IsConnected()) {
			SFS.ProcessEvents();
		}

        // for debugging
        /*if (SFS.moreText) {
            debugText += SFS.debugText;
            SFS.moreText = false;
        }
        if (debugText != selected.text) {
            selected.text = debugText;
        }*/
    }


    void OnMouseEnter()
 	{
 
    //   string objectName = gameObject.name;
    //   Debug.Log(objectName);
    //  startcolor = rend.material.color;
    //  rend.material.color = Color.grey;
     // Debug.Log(this.GameObject.name);
 	}

    void OnMouseExit()
 	{
 
 	}

 	void OnMouseDown()
 	{

 	}

    public static void trace(string msg) {
		//debugText += (debugText != "" ? "\n" : "") + msg;
	}

	public void EnterChooseCharacterScene() {
		ISFSObject obj = SFSObject.NewInstance();
        ExtensionRequest req = new ExtensionRequest("gm.enterChooseCharacterScene",obj);
        SFS.Send(req);
        trace("Sent enter scene message");
	}


    public void CharacterChoice(Button b) {
        //Debug.Log("called character choice");
        //var go = EventSystem.current.currentSelectedGameObject;
        character = b.name.ToUpper();
        //Debug.Log(character);
        SetAllNotInteractable();
        ISFSObject obj = SFSObject.NewInstance();
		obj.PutUtfString("chosenCharacter", character);
        ExtensionRequest req = new ExtensionRequest("gm.chosenCharacter",obj);
        SFS.Send(req);
        //trace("chose"+character);
    }

    public void UpdateDisplayText(string ut) {
        display.text += ut;
        SFS.chosenCharText = "";
    }

	public void DisplayRemainingCharacters(BaseEvent evt) {
		ISFSObject responseParams = (SFSObject)evt.Params["params"];
        try {
            ISFSArray a = responseParams.GetSFSArray("characterList");
            int size = responseParams.GetSFSArray("characterList").Size();
            //Debug.Log("Characters to choose from: " + size);
            // loop through all the buttons
            // if a character's name is in the input list -> active the button // otherwise deactive the btn 
            SetAllNotInteractable();
            if(character == "") { // only let buttons be interactable if you haven't chosen yet
                var foundButtonObjects = FindObjectsOfType<Button>();
                for (int i = 0; i < size; i++) {
                    foreach(Button btn in foundButtonObjects){
                        string banditName = (string)a.GetUtfString(i);
                        //Debug.Log(banditName);
                        if(btn.name == "Tuco" && banditName == "TUCO"){
                            //Debug.Log("setting tuco btn to true");
                            // save in a variable so we can use it later 
                            //character = "Tuco"; 
                            btn.interactable = true; 
                        }
                        if(btn.name == "Belle" && banditName == "BELLE"){
                            //character = "Belle"; 
                            btn.interactable = true; 
                        }
                        if(btn.name == "Cheyenne" && banditName == "CHEYENNE"){
                            //Debug.Log("setting cheyenne btn to true");
                            //character = "Cheyenne"; 
                            btn.interactable = true; 
                        }
                        if(btn.name == "Django" && banditName == "DJANGO"){
                            //character = "Django"; 
                            btn.interactable = true; 
                        }
                        if(btn.name == "Ghost" && banditName == "GHOST"){
                            //character = "Ghost"; 
                            btn.interactable = true; 
                        }
                        if(btn.name == "Doc" && banditName == "DOC"){
                            //character = "Doc"; 
                            btn.interactable = true; 
                        }
                    }
                }
            }
        } catch (Exception e) {
            Invoke("NextScene",3);
        }
	}

    private void SetAllNotInteractable() {
        var foundButtonObjects = FindObjectsOfType<Button>();
        foreach(Button btn in foundButtonObjects){
            if(btn.name == "Tuco"){
                //Debug.Log("setting tuco btn to false");
                btn.interactable = false; 
            }
            if(btn.name == "Belle"){
                btn.interactable = false; 
            }
            if(btn.name == "Cheyenne"){
                btn.interactable = false;
            }
            if(btn.name == "Django"){
                btn.interactable = false; 
            }
            if(btn.name == "Ghost"){
                btn.interactable = false; 
            }
            if(btn.name == "Doc"){
                btn.interactable = false; 
            }
        }
    }

    private void NextScene() {
        SFS.enteredGame = true;
        SceneManager.LoadScene("GameBoard");
    }

    public static void RemoveLaunchedSession() {
        Debug.Log("removing session?");
        if (true/*WaitingRoom.hosting*/) {
            Debug.Log("removing session!");
            Debug.Log("hash: " + WaitingRoom.gameHash);
            var request = new RestRequest("oauth/token", Method.POST)
            .AddParameter("grant_type", "password")
            .AddParameter("username", "admin")
            .AddParameter("password", "admin")
            .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
            IRestResponse response = client.Execute(request);
            
            var obj = JObject.Parse(response.Content);
            string adminToken = (string)obj["access_token"];
            adminToken = adminToken.Replace("+", "%2B");
            //PlayerPrefs.SetString("admintoken", adminToken);
            //PlayerPrefs.Save();

            var request2 = new RestRequest("api/sessions/" + WaitingRoom.gameHash + "?access_token=" + adminToken, Method.DELETE)
                .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
            IRestResponse response2 = client.Execute(request2);
        }
    }

    void OnApplicationQuit() {
        RemoveLaunchedSession();
		// Always disconnect before quitting
		SFS.Disconnect();
	}

}