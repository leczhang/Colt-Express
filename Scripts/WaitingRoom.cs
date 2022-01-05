using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using RestSharp;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
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

public class WaitingRoom : MonoBehaviour
{
    private static RestClient client = new RestClient("http://13.72.79.112:4242");

    public Button HostGameButton;
    public Button LaunchGameButton;
    public Button NewGameButton;
    public Button SavedSessionButtonA;
    public Button SavedSessionButtonB;
    public Button DeleteButton;
    public Button DeleteSessionButton;

    public Text fToken; // newgamebutton text
    public Text SavedSessionButtonAText;
    public Text SavedSessionButtonBText;
    public Text SavedSessionIDButtonAText;
    public Text SavedSessionIDButtonBText;
    public Text InfoText;
    public Text launchText;
    public Text deleteText;
    public Text deleteSessionText;

    //bool playingSaveGame = false;

    public static string gameHash = null;
    private static string token;
    private static string username;

    private static bool joined = false;
    public static bool hosting = false;
    public static int numPlayers;
    public static int numSessions = 0;
    public static bool intentToDelete = false;
    public static bool intentToDeleteSession = false;

    public static bool resetFields = false;

    private static Dictionary<Button, string> hashes = new Dictionary<Button, string>();
    private static Dictionary<Button, string> saveMap = new Dictionary<Button, string>();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(GetAdminToken());
        token = PlayerPrefs.GetString("token", "No token found");
        username = PlayerPrefs.GetString("username", "noUser");
        
        if (SFS.getSFS() == null) {
            // Initialize SFS2X client. This can be done in an earlier scene instead
            SmartFox sfs = new SmartFox();
            // For C# serialization
            DefaultSFSDataSerializer.RunningAssembly = Assembly.GetExecutingAssembly();
            SFS.setSFS(sfs);
            Debug.Log("SFS was null. Setting it now");
        }
        if (!SFS.IsConnected()) {
            SFS.Connect(username);
            Debug.Log("was not connected. Connecting now");
        }

        HostGameButton.interactable = false;
        LaunchGameButton.interactable = false;
        NewGameButton.interactable = false;
        SavedSessionButtonA.interactable = false;
        SavedSessionButtonB.interactable = false;
        DeleteButton.interactable = false;
        DeleteSessionButton.interactable = false;

        // if(resetFields) {
        //     SetFields();
        // }

        GetSessions();

        //TODO:
        // make enter room functionality only occur when entered gameboard
        // do not allow the same user to log in from multiple machines


        // add savegame logic to WR
        /*
        - //have >=2 buttons each for savegames
        - //clicking hostgame launches a session
        - clicking on a savegame creates a forked session
        - clicking 'save' during gameplay creates a savegame. Every subsequent
          click to save the game does nothing in LS but updates saved game on server
        - completing/leaving a game that was never saved deletes the launched session
        - completing a game that was saved (either during the game or earlier)
          first deletes the savegames with that savegameid, then the session
        - leaving a game that was saved deletes the launched session but keeps the last
          savegame
        - add a delete savegame button to WR that deletes savegames (and unlaunched
          games forked from it)

        - //if someone logs out, they leave the session. If they are the host, the session
          is deleted and the buttons become clickable
        - //only one session button is needed at all times. Once someone hosts (by clicking
          host game or clicking on a savegame), no one else should be able to do anything
          but join that game. When you enter the WR, either there will be a game ready
          for you to join that has been launched by someone and you can't click anything
          else, or you can host a game. There is never an unlaunched session going on
          unless its about to be played
        - change logic to require 'min-players' # of people to register before
          a game becomes launchable, not just a fixed number like 2 or 3. Because
          savegames can have a higher min number

        - do not allow for more than 2/3 savegames to ever be created
        - add savegame api call logic to a 'settings' scene--no savegames are created in WR
        */
    }

    void SetFields() {
        gameHash = null;
        joined = false;
        hosting = false;
        numSessions = 0;
        intentToDelete = false;
        intentToDeleteSession = false;

        hashes = new Dictionary<Button, string>();
        saveMap = new Dictionary<Button, string>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SFS.IsConnected()) {
			SFS.ProcessEvents();
		}

        // Create a temporary reference to the current scene.
        Scene currentScene = SceneManager.GetActiveScene();
         // Retrieve the name of this scene.
        string sceneName = currentScene.name;

        if(sceneName == "WaitingRoom") {
            GetSessions();
            if(!SFS.enteredGame && numSessions > 0) {
                if(hosting && !LaunchGameButton.interactable) {
                    ActivateLaunchGameButton();
                }
                GoToGame();
            }
            Invoke("Stall",1);
        }
    }

    private void Stall() { }

    public void GoToGame() {
        if (gameHash != null && joined) {
            var request = new RestRequest("api/sessions/" + gameHash, Method.GET)
                .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
            IRestResponse response = client.Execute(request);
            var obj = JObject.Parse(response.Content);
            Dictionary<string, object> sessionDetails = obj.ToObject<Dictionary<string, object>>();
            
            if (Launched(gameHash)) {
                numPlayers = 1 + sessionDetails["players"].ToString().ToCharArray().Count(c => c == ',');
                SceneManager.LoadScene("ChooseYourCharacter");
            }
            
        }
    }

    public void PlayingSaveGame(Text savegameID) {
        if(!intentToDelete) {
            Debug.Log("GameID content : " + savegameID.text);
            ISFSObject obj2 = SFSObject.NewInstance();
            obj2.PutUtfString("savegameId", savegameID.text);
            ExtensionRequest req = new ExtensionRequest("gm.loadSavedGame",obj2);
            SFS.Send(req);
        }
    }

    private bool Launched(string hash) {
        var request = new RestRequest("api/sessions/" + hash, Method.GET)
                .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
        IRestResponse response = client.Execute(request);
        var obj = JObject.Parse(response.Content);
        Dictionary<string, object> sessionDetails = obj.ToObject<Dictionary<string, object>>();
        if (sessionDetails["launched"].ToString().ToLower() == "true") {
            return true;
        }
        return false;
    }

    public void ActivateLaunchGameButton() {
        var request = new RestRequest("api/sessions/" + gameHash, Method.GET)
            .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
        IRestResponse response = client.Execute(request);
        var obj = JObject.Parse(response.Content);
        Dictionary<string, object> sessionDetails = obj.ToObject<Dictionary<string, object>>();
        numPlayers = 1 + sessionDetails["players"].ToString().ToCharArray().Count(c => c == ',');

        //Dictionary<string, object> sessionGameParams = (Dictionary<string, object>)sessionDetails["gameParameters"];
        int minPlayers = 3; //number of players from savegame //(int)sessionGameParams["minSessionPlayers"];
        // get savegame if it is one, check how many players it needs
        if (numPlayers >= minPlayers) {
            LaunchGameButton.interactable = true;
        }
    }

    public void JoinGame(Button gameToJoin)
    {   

        gameHash = hashes[gameToJoin];
        var request = new RestRequest("api/sessions/" + gameHash + "/players/" + username + "?access_token=" + token, Method.PUT)
        .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
        IRestResponse response = client.Execute(request);
        foreach(KeyValuePair<Button, string> entry in hashes){
            entry.Key.interactable = false;
        } 
        LaunchGameButton.interactable = false;
        //HostGameButton.interactable = false;

        var request2 = new RestRequest("api/sessions/" + gameHash, Method.GET)
        .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
        IRestResponse response2 = client.Execute(request2);
        var obj2 = JObject.Parse(response2.Content);
        Dictionary<string, object> sessionDetails = obj2.ToObject<Dictionary<string, object>>();

        Debug.Log("SavedGame ID : " + sessionDetails["savegameid"].ToString());

        GameBoard.saveGameId = sessionDetails["savegameid"].ToString();

        joined = true;
        InfoText.text = "You will be brought to the next scene once the host launches the game!";
        
        SFS.JoinRoom();

        // if(!intentToDeleteSession){
        //     gameHash = hashes[gameToJoin];
        //     var request = new RestRequest("api/sessions/" + gameHash + "/players/" + username + "?access_token=" + token, Method.PUT)
        //     .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
        //     IRestResponse response = client.Execute(request);
        //     foreach(KeyValuePair<Button, string> entry in hashes){
        //         entry.Key.interactable = false;
        //     } 
        //     LaunchGameButton.interactable = false;
        //     //HostGameButton.interactable = false;

        //     var request2 = new RestRequest("api/sessions/" + gameHash, Method.GET)
        //         .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
        //     IRestResponse response2 = client.Execute(request2);
        //     var obj2 = JObject.Parse(response2.Content);
        //     Dictionary<string, object> sessionDetails = obj2.ToObject<Dictionary<string, object>>();

        //     Debug.Log("SavedGame ID : " + sessionDetails["savegameid"].ToString());

        //     GameBoard.saveGameId = sessionDetails["savegameid"].ToString();

        //     joined = true;
        //     InfoText.text = "You will be brought to the next scene once the host launches the game!";
        
        //     SFS.JoinRoom();
        // }else{
        //     if(joined){
        //         LaunchGameButton.interactable = false;
        //         joined = false;
        //     }
        //     if(!Launched(gameHash)) {
        //         DeleteSession();
        //     }else{
        //         var request = new RestRequest("api/sessions/" + gameHash + "?access_token=" + GetAdminToken(), Method.DELETE)
        //         .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
        //         IRestResponse response = client.Execute(request);
        //     } 
        //     hosting = false;
        //     deleteSessionText.text = "Delete a session";

        //     if (hashes.ContainsKey(gameToJoin)) {
        //         hashes.Remove(gameToJoin);
        //         fToken.text = "";
        //         NewGameButton.interactable = false;
        //         InfoText.text = "You can:\n-Host a new game\n-Click on a saved game to host it\n-Click on a game session to join it";
        //     }
        // }
        
    }

    public void CreateSession(Text savegameID)
    {
        if(!intentToDelete) {
            dynamic j = new JObject();
            j.creator = username;
            j.game = "ColtExpress";
            if(savegameID == null) {
                Debug.Log("null");
                j.savegame = "";
            } else {
                j.savegame = savegameID.text;
            }

            var request = new RestRequest("api/sessions?access_token=" + token, Method.POST)
                .AddParameter("application/json", j.ToString(), ParameterType.RequestBody)
                .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
            IRestResponse response = client.Execute(request);
            gameHash = response.Content;
            //HostGameButton.interactable = false;
            hosting = true;
            joined = true;
            InfoText.text = "You will be brought to the next scene once you launch your game!";

            SFS.JoinRoom();
        }
    }

    // FIX, ATTACH TO BUTTONS, ALLOW FOR MAX 3 SESSIONS
    private void GetSessions()
    {
        var request = new RestRequest("api/sessions", Method.GET);
        IRestResponse response = client.Execute(request);

        var obj = JObject.Parse(response.Content);
        Dictionary<string, Dictionary<string, Dictionary<string, object>>> sessions = obj.ToObject<Dictionary<string, Dictionary<string, Dictionary<string, object>>>>();
        //Dictionary<string, Dictionary<string, Dictionary<string, string>>> sessionsPlayers = obj.ToObject<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>();
        int numSessionsTemp = 0;
        foreach(KeyValuePair<string, Dictionary<string, object>> entry in sessions["sessions"])
        {
            // create a gamebutton for each game here

            if (!hashes.ContainsKey(NewGameButton)) {
                hashes.Add(NewGameButton, entry.Key);
                if(!hosting) {
                    NewGameButton.interactable = true;
                }
                //NewGameButton.onClick.AddListener(JoinGame);//delegate{JoinGame(NewGameButton);});
            }
            
            fToken.text = "\nSession ID: " + entry.Key;
            fToken.text += "\nCreated by: " + entry.Value["creator"].ToString();
            string players = entry.Value["players"].ToString();
            var charsToRemove = new string[] { "[", "]", "\n", "\"", " "};
            foreach (var c in charsToRemove)
            {
                players = players.Replace(c, string.Empty);
            }
            fToken.text += "\nCurrrent participants: " + players;
            fToken.text += "\nLaunch status: " + entry.Value["launched"].ToString();


            
            numSessionsTemp++;
            // for now, we only have one button, so break after first session
            break;   
        }
        numSessions = numSessionsTemp;

        GetSaveGames();

        if(numSessions==0) {
            DeleteButton.interactable = true;
            DeleteSessionButton.interactable = false;
            if(intentToDelete) {
                HostGameButton.interactable = false;
            } else {
                HostGameButton.interactable = true;
            }
            if(hashes.Count > 0) {
                hashes.Remove(NewGameButton);
                fToken.text = "";
                NewGameButton.interactable = false;
                InfoText.text = "You can:\n-Host a new game\n-Click on a saved game to host it\n-Click on a game session to join it";
            }
        } else {
            HostGameButton.interactable = false;
            DeleteButton.interactable = false;
            SavedSessionButtonA.interactable = false;
            SavedSessionButtonB.interactable = false;
            if(joined) {
                NewGameButton.interactable = false;
            }
            if(hosting){
                DeleteSessionButton.interactable = true;
                if(intentToDeleteSession){
                    NewGameButton.interactable = true;
                }else{
                    NewGameButton.interactable = false;
                }
            }
        }

    }

    private void GetSaveGames() {
        var request = new RestRequest("api/gameservices/ColtExpress/savegames?access_token=" + token, Method.GET);
        IRestResponse response = client.Execute(request);

        //var obj = JObject.Parse(response.Content);
        var arr = JArray.Parse(response.Content);
        //Debug.Log(arr);
        //Debug.Log("type: " + Type.GetType(arr).ToString());

        List<Dictionary<string, object>> saveGames = arr.ToObject<List<Dictionary<string, object>>>();//response.Content;
        //List<Dictionary<string, object>> saveGames = obj.ToObject<List<Dictionary<string, object>>>();
        int gameNo = 0;
        Text saveGameText;
        Text saveGameIDText;
        int i = 0;
        foreach(Dictionary<string, object> saveGame in saveGames)
        {
            if(i>1) break;
            i++;
            // create a gamebutton for each game here

            if (gameNo == 0) {
                if(numSessions == 0) {
                    SavedSessionButtonA.interactable = true;
                }
                saveGameText = SavedSessionButtonAText;
                saveGameIDText = SavedSessionIDButtonAText;
            } else if (gameNo == 1) {
                if(numSessions == 0) {
                    SavedSessionButtonB.interactable = true;
                }
                saveGameText = SavedSessionButtonBText;
                saveGameIDText = SavedSessionIDButtonBText;
            } else {
                saveGameText = null;
                saveGameIDText = null;
            }
            
            saveGameText.text = "SaveGame ID:\n\n";
            saveGameIDText.text = (string)saveGame["savegameid"];
            string players = saveGame["players"].ToString();
            var charsToRemove = new string[] { "[", "]", "\n", "\"", " "};
            foreach (var c in charsToRemove)
            {
                players = players.Replace(c, string.Empty);
            }
            saveGameText.text += "\nOriginal participants: " + players;
            //fToken.text += "\nLaunch status: " + entry.Value["launched"].ToString();

            // for now, we only have one button, so break after first session
            gameNo++;   
        }
        if (gameNo==0) {
            SavedSessionButtonAText.text = "";
            SavedSessionIDButtonAText.text = "";
            SavedSessionButtonA.interactable = false;
            SavedSessionButtonBText.text = "";
            SavedSessionButtonB.interactable = false;
        } else if (gameNo==1) {
            SavedSessionButtonBText.text = "";
            SavedSessionIDButtonBText.text = "";
            SavedSessionButtonB.interactable = false;
        }
    }

    public void IntentToDelete() {
        if(intentToDelete) {
            intentToDelete = false;
            deleteText.text = "Delete a saved game";
        } else {
            intentToDelete = true;
            deleteText.text = "Go back";
        }

    }

     public void IntentToDeleteSession() {
        if(intentToDeleteSession) {
            intentToDeleteSession = false;
            deleteSessionText.text = "Delete a session";
        } else {
            intentToDeleteSession = true;
            deleteSessionText.text = "Go back";
        }

    }

    static public void DeleteSaveGame(Text savegameID) {
        if(intentToDelete) {
            string adminToken = GetAdminToken();
            var request = new RestRequest("api/gameservices/ColtExpress/savegames/" + savegameID.text + "?access_token=" + adminToken, Method.DELETE)
                .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
            IRestResponse response = client.Execute(request);
            Debug.Log("Here is the delete saved game return: "+ response.ErrorMessage + "   " + response.StatusCode);

            Debug.Log("GameID of saved game to be deleted content : " + savegameID.text);
            ISFSObject obj = SFSObject.NewInstance();
            obj.PutUtfString("savegameId", savegameID.text);
            ExtensionRequest req = new ExtensionRequest("gm.deleteSavedGame",obj);
            SFS.Send(req);
        }
    }

    private void LeaveSession() {
        if(joined && !Launched(gameHash)) {
            if(!hosting) {
                var request = new RestRequest("api/sessions/" + gameHash + "/players/" + username + "?access_token=" + token, Method.DELETE)
                    .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
                IRestResponse response = client.Execute(request);
            } else {
                DeleteSession();
                hosting = false;
            }
        } 
        joined = false;
    }

    public void LaunchSession()
    {
        var request = new RestRequest("api/sessions/" + gameHash + "?access_token=" + token, Method.POST)
                .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
            IRestResponse response = client.Execute(request);
        joined = true;
        launchText.text = "Your game is launching!";
    }

    /*private static Boolean LaunchStatus()
    {
        var request = new RestRequest("api/sessions", Method.GET);
        IRestResponse response = client.Execute(request);
        var obj = JObject.Parse(response.Content);
        var status = (string)obj["sessions"][ExtractHash()]["launched"];
        if (status.ToLower().Equals("true"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }*/

    public static string GetAdminToken() {
        var request = new RestRequest("oauth/token", Method.POST)
            .AddParameter("grant_type", "password")
            .AddParameter("username", "admin")
            .AddParameter("password", "admin")
            .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
            IRestResponse response = client.Execute(request);
            
        var obj = JObject.Parse(response.Content);
        string adminToken = (string)obj["access_token"];
        return adminToken.Replace("+", "%2B");
    }

    private void DeleteSession() {
        var request = new RestRequest("api/sessions/" + gameHash + "?access_token=" + token, Method.DELETE)
                .AddHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
        IRestResponse response = client.Execute(request);
    }

    public void Logout() {
        ChooseCharacter.RemoveLaunchedSession();
        LeaveSession();
        SFS.Disconnect();
        SFS.setSFS(null);
        SceneManager.LoadScene("Login");
    }

    void OnApplicationQuit() {
        ChooseCharacter.RemoveLaunchedSession();
		// Always disconnect before quitting
		SFS.Disconnect();
	}

    public void clickSaveGame() {
        SaveGameState("test");
    }

    public void SaveGameState(string savegameID) {    
        GameBoard.TestSave();
	}
}