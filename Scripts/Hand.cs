
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;
using RestSharp;
using Newtonsoft.Json.Linq;
public class Hand : MonoBehaviour
{
    public Text message;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void doAction(){
        message.text = "hii";

        string button = EventSystem.current.currentSelectedGameObject.name;
        message.text = "Clicked on " + button;
    }
}