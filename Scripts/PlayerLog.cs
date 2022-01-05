using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using model;
 
 public class PlayerLog : MonoBehaviour {
     // Private VARS
     public List<string> Eventlog = new List<string>();

     public static bool myTurnSchemin = false;

     public static GameManager gm;
     // public static Text announcement; 

     public static void setGame(GameManager newGm) {
        gm = newGm;
    }



      // public string guiText = "";
     public string guiText = "Player Played: ";
     private string logmsg; 
     public int maxLines = 3; 
     public GameObject cardA; 
     public Text log; 

    public static void promptDrawCardsOrPlayCard() {
          Debug.Log("setting my turn to true");
          myTurnSchemin = true;
          string msgCard = "PLEASE PLAY A CARD OR DRAW 3 CARDS";
      
          // GameObject.Find("promptDrawCardsOrPlayCardMsg").GetComponent<UnityEngine.UI.Text>().text = msg.ToString();
          // setting the prompt message 
          GameObject.Find("promptDrawCardsOrPlayCardMsg").GetComponent<Text>().text = msgCard.ToString();


          // set clickable objects 
     }

     public static void promptChooseLoot() {
          string msgLoot = "PLEASE CHOOSE A LOOT";
      
          // GameObject.Find("promptDrawCardsOrPlayCardMsg").GetComponent<UnityEngine.UI.Text>().text = msg.ToString();
          // setting the prompt message 
          GameObject.Find("promptChooseLoot").GetComponent<Text>().text = msgLoot.ToString();

          // set clickable loots 
     }

     public static void promptPunchTarget() {
          string msgPunch = "PLEASE CHOOSE A BANDIT TO PUNCH";
          GameObject.Find("promptPunchTarget").GetComponent<Text>().text = msgPunch.ToString();
          // set clickable loots 
     }


//      public void AddEvent(string eventString){
//          Eventlog.Add(eventString);
//  
//          if (Eventlog.Count >= maxLines) Eventlog.RemoveAt(0);
         
//          guiText = ""; 

//          foreach (string logEvent in Eventlog)
//          {
//              guiText += logEvent;
//              guiText += "\n";
//         }
//      }

     void Start(){
          log.text = guiText;
          
          promptDrawCardsOrPlayCard();
     }

     public void OnButtonClick()
     {
         var go = EventSystem.current.currentSelectedGameObject;
         if (go != null)
             Debug.Log("Clicked on : "+ go.name);
         else
             Debug.Log("curr game obj is null :(");

          // guiText += go.name;
          // guiText += "\n";
          if(go.name == "CardA"){
               log.text += "Ghost plays Move"+"\n";
          }

          if(go.name == "CardB"){
               log.text += "Ghost plays Rob"+"\n";
          }

          if(go.name == "CardC"){
                  log.text += "Cheyenne plays Marshal"+"\n";
          }

          if(go.name == "CardD"){
               log.text += "Cheyenne plays ChangeFloor"+"\n";
          }

          log.text += go.name; 
          guiText += go.name;
          guiText += "\n";

          if(myTurnSchemin) {
               Debug.Log("calling playcard");
               // ActionCard c = (ActionCard)GameBoard.objects[go];
               myTurnSchemin = false;
               // gm.playCard(c);
          }


     }

//      public void AddEvent(string eventString){
 
//  
//          // if (Eventlog.Count >= maxLines) Eventlog.RemoveAt(0);
         
//          guiText = ""; 
//          guiText += eventString; 

// //          foreach (string logEvent in Eventlog)
// //          {
// //              guiText += logEvent;
// //              guiText += "\n";
// //         }
//      }

     void Update () 
     {
          // log.text = guiText;
     }
          // msg.text = guiText; 
          // if(Input.GetButtonDown("CardA")){
          //      Debug.Log("CARDA");

          //      Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
          //      RaycastHit hit;
          //      if( Physics.Raycast( ray, out hit, 100 ) )
          //      {
          //           Debug.Log( hit.transform.gameObject.name );
          //      } 

          // }
     
//         string button = EventSystem.current.currentSelectedGameObject.name;
//         
//         if (button == EventSystem.current.currentSelectedGameObject.name)
//          eventLog.AddEvent("Player clicked on " + button);

//         else if (Input.GetKey(KeyCode.LeftArrow))
//          eventLog.AddEvent("Player Moves Left");
//  
//         else if (Input.GetKey(KeyCode.RightArrow))
//          eventLog.AddEvent("Player Moves Right");
      // string name =  EventSystem.current.currentSelectedGameObject.name;
          // Debug.Log(name);
          
     //    if (Input.GetKey(KeyCode.LeftArrow))
     //         eventLog.AddEvent("Player Moves Left");
 
     //     if (Input.GetKey(KeyCode.RightArrow))
     //         eventLog.AddEvent("Player Moves Right");

     // void OnMouseDown(){
     //      // if (!Input.GetMouseButtonDown(0)) return;
     //      Debug.Log (this.gameObject.name);
     //      eventLog.AddEvent("Player Moves Left");
     //      eventLog.AddEvent("Player Moves Right");
     // }

     // Public VARS
     // public int maxLines = 3;


  

//     void OnMouseDown(){
//         Debug.Log("loggg"); 
//         string button = EventSystem.current.currentSelectedGameObject.name;
//         if (button == EventSystem.current.currentSelectedGameObject.name){
//             Eventlog.Add("ADDED TO EVENTLOG!");
//             AddEvent("EVENT ADDED");
//         }

// //         if (button == EventSystem.current.currentSelectedGameObject.name)
// //          eventLog.AddEvent("Player clicked on " + button);

// //         else if (Input.GetKey(KeyCode.LeftArrow))
// //          eventLog.AddEvent("Player Moves Left");
// //  
// //         else if (Input.GetKey(KeyCode.RightArrow))
// //          eventLog.AddEvent("Player Moves Right");
//     }
 }