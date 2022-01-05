using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

//using System.Windows;

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

public static class SFS
{
    public static SmartFox sfs;
    public static string defaultHost; 
	public static int defaultTcpPort;
    public static string zone;
    public static string debugText = "";
    public static bool moreText = false;
    public static string username; //should be set to logged in user's name
	public static bool enteredGame = false;
	
	public static string chatText = "";
	public static string chosenCharText = "";

	public static int step = 0;

	public static TestGame tg;
	public static GameBoard gb;
	public static ChooseCharacter cc;
	public static Chat chat;
	public static int trainIndex;

    static SFS(){
        defaultHost =  "13.72.79.112";//"127.0.0.1";  
	    defaultTcpPort = 9933;
        zone = "MergedExt";
    }

    public static void setSFS(SmartFox Sfs) {
        sfs = Sfs;
    }

	public static SmartFox getSFS() {
        return sfs;
    }

	public static void setTestGame() {
		tg = GameObject.Find("TestGameGO").GetComponent<TestGame>();
	}

	public static void setGameBoard() {
		gb = GameObject.Find("GameBoardGO").GetComponent<GameBoard>();
	}

	public static void setChooseCharacter() {
		cc = GameObject.Find("ChooseCharacterGO").GetComponent<ChooseCharacter>();
	}

	public static void setChat() {
		chat = GameObject.Find("ChatGO").GetComponent<Chat>();
	}

    public static void trace(string msg) {
		//debugText += (debugText != "" ? "\n" : "") + msg;
        debugText = msg;
        moreText = true;
	}

    
    public static void ProcessEvents() {
        sfs.ProcessEvents();
    }

    public static void Send(ExtensionRequest req) {
        sfs.Send(req);
    }

	public static void Send(PublicMessageRequest req) {
		Debug.Log("sending message");
        sfs.Send(req);
    }

    // client side: receiving feedback from SERVER
    private static void OnExtensionResponse(BaseEvent evt) {
        String cmd = (String)evt.Params["cmd"];
        trace("response received: " + cmd); // shpows up after "in-class" debug message
		if (cmd == "remainingCharacters") {
			ISFSObject responseParams = (SFSObject)evt.Params["params"];
			string player = responseParams.GetUtfString("player");
			if(player != null){
				string chosen = responseParams.GetUtfString("chosenCharacter");
				chosenCharText += player + " chose " + chosen + "!\n";
			}
			if (cc != null) {
				cc.UpdateDisplayText(chosenCharText);
				cc.DisplayRemainingCharacters(evt);
			}
		} 
		
		else if (cmd == "updateGameState") {
			Debug.Log("UGS called in SFS.cs");
			Debug.Log("Current SaveGame ID Content before updating gamestate: "+ GameBoard.saveGameId);
			gb.UpdateGameState(evt);
			if (GameBoard.started==false & GameBoard.saveGameId == "") {
				Debug.Log("Condition triggered, started = false");
			 	gb.promptHorseAttack();
			}
        } 
		
		else if (cmd == "nextAction") {
			ISFSObject responseParams = (SFSObject)evt.Params["params"];
			step = responseParams.GetInt("step");
			Debug.Log("received step " + step);
			// gb.executeHardCoded(step);
		} else if (false/*cmd == incoming character name from save game*/) {
			// 	assign this client to be the character as specified by the incoming string
		} else if (cmd == "testSerial") {
			ISFSObject responseParams = (SFSObject)evt.Params["params"];
			GameManager gm = (GameManager) responseParams.GetClass("gm");
			GameManager.replaceInstance(gm);
			GameManager newgm = GameManager.getInstance();
			TrainUnit t = (TrainUnit) newgm.trainRoof[0];
			Debug.Log(t.carTypeAsString);
			Debug.Log("test");
			Hashtable ht = newgm.banditPositions;
			foreach(Bandit key in ht.Keys)
			{			
				TrainUnit m = (TrainUnit) ht[key];
   				Debug.Log(String.Format("{0}: {1}", key.characterAsString, m.carTypeAsString));
			}
			//GameBoard.SendNewGameState();
		}  else if (cmd == "testgame") {
			tg.ReceiveInitializedGame(evt);
		}  else if (cmd == "currentSaveGameID") {
			ISFSObject responseParams = (SFSObject)evt.Params["params"];
			string saveGame = responseParams.GetUtfString("savegameID");
			Debug.Log("Here is the returned SaveGame after Loading!!!"+ saveGame);
			if(saveGame != ""){
				GameBoard.saveGameId = saveGame;
			}
		}
    }

    public static void Connect(string uname) {
		if (sfs == null || !sfs.IsConnected) {
			
			username = uname;
			// CONNECT

			// Clear console
			//debugText.text = "";
			
			Debug.Log("Now connecting...");	
			
            // Add listeners
			sfs.AddEventListener(SFSEvent.CONNECTION, OnConnection);
			sfs.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
            sfs.AddEventListener(SFSEvent.LOGIN, OnLogin);
		    sfs.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);
            sfs.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);

			sfs.AddLogListener(LogLevel.INFO, OnInfoMessage);
			sfs.AddLogListener(LogLevel.WARN, OnWarnMessage);
			sfs.AddLogListener(LogLevel.ERROR, OnErrorMessage);

			//sfs.AddEventListener(SFSEvent.ROOM_JOIN, OnRoomJoin);
			sfs.AddEventListener(SFSEvent.ROOM_JOIN_ERROR, OnRoomJoinError);
			sfs.AddEventListener(SFSEvent.PUBLIC_MESSAGE, OnPublicMessage);
			//sfs.AddEventListener(SFSEvent.USER_ENTER_ROOM, OnUserEnterRoom);
			sfs.AddEventListener(SFSEvent.USER_EXIT_ROOM, OnUserExitRoom);
			//sfs.AddEventListener(SFSEvent.ROOM_ADD, OnRoomAdd);
			
			// Set connection parameters
			ConfigData cfg = new ConfigData();
			cfg.Host = defaultHost;
			cfg.Port = Convert.ToInt32(defaultTcpPort.ToString());
			cfg.Zone = zone;
			//cfg.Debug = true;
				
			// Connect to SFS2X
			sfs.Connect(cfg);
		} else {
			
			// Disconnect from SFS2X
			sfs.Disconnect();
            trace("Disconnected");
		}
	}

    public static bool IsConnected() {
        if (sfs != null && sfs.IsConnected) {
            return true;
        }
        return false;
    }


    private static void reset() {
		// Remove SFS2X listeners
		sfs.RemoveEventListener(SFSEvent.CONNECTION, OnConnection);
		sfs.RemoveEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
        sfs.RemoveEventListener(SFSEvent.LOGIN, OnLogin);
		sfs.RemoveEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);
        sfs.RemoveEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);

		sfs.RemoveLogListener(LogLevel.INFO, OnInfoMessage);
		sfs.RemoveLogListener(LogLevel.WARN, OnWarnMessage);
		sfs.RemoveLogListener(LogLevel.ERROR, OnErrorMessage);

		//sfs.RemoveEventListener(SFSEvent.ROOM_JOIN, OnRoomJoin);
		sfs.RemoveEventListener(SFSEvent.ROOM_JOIN_ERROR, OnRoomJoinError);
		sfs.RemoveEventListener(SFSEvent.PUBLIC_MESSAGE, OnPublicMessage);
		//sfs.RemoveEventListener(SFSEvent.USER_ENTER_ROOM, OnUserEnterRoom);
		sfs.RemoveEventListener(SFSEvent.USER_EXIT_ROOM, OnUserExitRoom);
		//sfs.RemoveEventListener(SFSEvent.ROOM_ADD, OnRoomAdd);
		
		sfs = null;

		//clearRoomList();
	}

    private static void OnConnection(BaseEvent evt) {
		if ((bool)evt.Params["success"]) {
			Debug.Log("Connection established successfully");

            // Login with some username after having made connection
			sfs.Send(new Sfs2X.Requests.LoginRequest(username));

		} else {
			Debug.Log("Connection failed; is the server running at all?");
			
			// Remove SFS2X listeners and re-enable interface
			reset();
		}
	}
	
	private static void OnConnectionLost(BaseEvent evt) {
		Debug.Log("Connection was lost; reason is: " + (string)evt.Params["reason"]);
		
		// Remove SFS2X listeners and re-enable interface
		reset();
	}
	
	//----------------------------------------------------------
	// SmartFoxServer log event listeners
	//----------------------------------------------------------
	
	public static void OnInfoMessage(BaseEvent evt) {
		string message = (string)evt.Params["message"];
		ShowLogMessage("INFO", message);
	}
	
	public static void OnWarnMessage(BaseEvent evt) {
		string message = (string)evt.Params["message"];
		ShowLogMessage("WARN", message);
	}
	
	public static void OnErrorMessage(BaseEvent evt) {
		string message = (string)evt.Params["message"];
		ShowLogMessage("ERROR", message);
	}
	
	private static void ShowLogMessage(string level, string message) {
		message = "[SFS > " + level + "] " + message;
		trace(message);
		Debug.Log(message);
	}

    /*public static void OnApplicationQuit() {
		// Always disconnect before quitting
		if (sfs != null && sfs.IsConnected)
			sfs.Disconnect();
	}*/

    public static void Disconnect() {
        if (sfs != null && sfs.IsConnected)
			sfs.Disconnect();
    }

    //** LOGIN STUFF **//


    private static void OnLogin(BaseEvent evt) {
		/*User user = (User) evt.Params["user"];

		// Show system message
		string msg = "Login successful!\n";
		msg += "Logged in as " + user.Name;
		trace(msg);

		// Populate Room list
		populateRoomList(sfs.RoomList);*/
	}

	public static void JoinRoom() {
		// Join first Room in Zone
		if (sfs.RoomList.Count > 0) {
			Debug.Log("joining a room");
			sfs.Send(new Sfs2X.Requests.JoinRoomRequest(sfs.RoomList[0].Name));
		}
	}

	public static void LeaveRoom() {
		Debug.Log("leaving room");
		sfs.Send(new Sfs2X.Requests.LeaveRoomRequest());
	}
	
	private static void OnLoginError(BaseEvent evt) {
		// Disconnect
		sfs.Disconnect();

		// Remove SFS2X listeners and re-enable interface
		reset();
		
		// Show error message
		debugText = "Login failed: " + (string) evt.Params["errorMessage"];
	}
	
	private static void OnRoomJoinError(BaseEvent evt) {
		// Show error message
		Debug.Log("Room join failed: " + (string) evt.Params["errorMessage"]);
	}
	
	private static void OnUserExitRoom(BaseEvent evt) {
		ISFSObject obj = SFSObject.NewInstance();
		ExtensionRequest req = new ExtensionRequest("gm.removeGame",obj);
        Send(req);

		User user = (User) evt.Params["user"];
		username = PlayerPrefs.GetString("username", "No username found");
		Debug.Log(username);
		Debug.Log(user.Name);
		if (user.Name != username) {			
			Debug.Log(user.Name + " left the game!");
			//gb.exit.SetActive(true);
			gb.exitText.text = user.Name + " left the game! You will now be redirected to the Waiting Room"; 
			//Invoke("GoToWaitingRoom", 5);
			gb.GoToWaitingRoom();
		} else {
			//gb.exit.SetActive(true);
			gb.exitText.text = "You will now be redirected to the Waiting Room";
			Debug.Log("Returning to waiting room");
			//Invoke("GoToWaitingRoom", 5);
			gb.GoToWaitingRoom();
		}
	}

    private static void OnPublicMessage(BaseEvent evt) {
		User sender = (User) evt.Params["sender"];
		string message = (string) evt.Params["message"];
		
		printUserMessage(sender, message);
		if(chat != null) chat.printUserMessage();
	}

	private static void printUserMessage(User user, string message) {
		chatText += "<b>" + (user == sfs.MySelf ? "You" : user.Name) + ":</b> " + message + "\n";
	}
}
