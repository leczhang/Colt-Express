using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using model;

public class loot : MonoBehaviour
{
    public static GameManager gm;
    
    public GameObject gem; 

    void OnMouseDown(){
        // gameObject.active = false; 
        // Destroy(gameObject);
        // gem = GameObject.Find("gem");
        // Debug.Log("position is: " + gem.transform.position);

        /*if(objects[gem] is in the arraylist of clickable) {
            if(action == "punch") {
                gm.punch(objects[gem]);
            } else if (action == "rob") {
                gm.rob(objects[gem]);
            }
        }
        gem.SetActive(false);
        Destroy(gem);*/
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

// public class loot : MonoBehaviour
// {
//     public static GameManager gm;
    
//     public GameObject gem; 

//     void OnMouseDown(){
//         // gameObject.active = false; 
//         // Destroy(gameObject);
//         // gem = GameObject.Find("gem");
//         // Debug.Log("position is: " + gem.transform.position);

    // Update is called once per frame
    void Update()
    {
        // instead of using this to get gm, updateGameState() in gameboard.cs simply called setGame(gm) below
    }

    public static void setGame(GameManager newGm) {
        gm = newGm;
    }
}
