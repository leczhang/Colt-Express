using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Move : MonoBehaviour
{
    // public TrainUnit position;
    // public string carTypeAsString; 
    // public string carFloorAsString; 

    // map: key: object, value: (x,y) 
    public Dictionary<GameObject, List<float>> objectToPosition = new Dictionary<GameObject, List<float>>();

    // possible positions 
    // we can check the cartTypeAsString and carFloorAsString received from GM and find the corresponding x,y,z 
    private List<float> cartZeroTop = new List<float>() {840.5F,878.4F,-364.9F};
    private List<float> cartZeroBtm = new List<float>() {786.1F, 813.5F, -364.9F};

    private List<float> cartOneTop = new List<float>() {1025.7F, 889.4F, -364.9F};
    private List<float> cartOneBtm = new List<float>() {1027.9F, 806.4F, -364.9F};

    private List<float> cartTwoTop = new List<float>() {1265.4F, 894.7F, -364.9F};
    private List<float> cartTwoBtm = new List<float>() {1279.8F, 817.7F, -364.9F};

    private List<float> cartLocoTop = new List<float>() {1410.5F, 893.4F, -364.9F};
    private List<float> cartLocoBtm = new List<float>() {1390.0F, 824.9F, -364.9F};


    // all movable gameobjects' position
    private List<float> belPos; 
    private List<float> chePos; 
    private List<float> docPos; 
    private List<float> djaPos; 
    private List<float> tucPos; 
    private List<float> ghoPos; 

    // pos offsets (may need to update these later)
    private List<float> belOffset = new List<float>() {1F, 0F, 0F}; 
    private List<float> cheOffset = new List<float>() {5F, 0F, 0F}; 
    private List<float> docOffset = new List<float>() {10F, 0F, 0F}; 
    private List<float> djaOffset = new List<float>() {-5F, 0F, 0F}; 
    private List<float> tucOffset = new List<float>() {-3F, 0F, 0F}; 
    private List<float> ghoOffset = new List<float>() {-1F, 0F, 0F}; 

    // getClicked returns the name of the GM that user clicks 
    /*string getClicked(){
        var goClicked = EventSystem.current.currentSelectedGameObject;
        // Debug.Log(goClicked.name);
        return goClicked;
    }*/

    // use the hashmap to map the GO to the desired type so we can pass it back to the GM 

    void Start()
    {
        //  Debug.Log(zeroBtm.transform.position);
        //  Debug.Log(oneTop.transform.position);
        //  Debug.Log(oneBtm.transform.position);
        // Debug.Log(twoTop.transform.position);
        //  Debug.Log(twoBtm.transform.position);
        // Debug.Log(locoTop.transform.position);
        //  Debug.Log(locoBtm.transform.position);
        // transform.position = Vector3.Lerp(startPoint, endPoint, (Time.deltaTime));

    }

    // void OnMouseDown(){
    //     // Vector3 p = lootToMove.transform.position; 
    //     // float x = lootToMove.transform.position.x;
    //     // x = x + 50; 
    //     // float y = lootToMove.transform.position.y;
    //     // float z = lootToMove.transform.position.z;
    //     // y += 60;
    //     // lootToMove.transform.position = new Vector3(x, y, z);
    //     //  startPoint = transform.position;
    //     //  endPoint = new Vector3(beginX, beginY, 0);

    // }

    // void OnMouseUp(){
    //      startPoint = transform.position;
    //      endPoint = new Vector3(beginX, beginY, 0);
    // }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetMouseButtonDown(0))
        // {
        // // this wont move the object forwards but will reset it's position to 0, 0, 1
        // // Debug.Log("POSITIONNNN");
        // //     Debug.Log(lootToMove.transform.position);
        //     // lootToMove.transform.position = new Vector3 (1219, 819, -364);

        //     // check if the obj being clicked on is the loot/bandit that we want to move 
        //     float posX = cartZeroTop[0]; 
        //     float posY = cartZeroTop[1]; 
        //     float posZ = cartZeroTop[2]; 
        //     lootToMove.transform.position = new Vector3 (posX, posY, posZ);
        // THE FOLLOWING CODE MOVES THE LOOTTOMOVE TO THE NEW POSITION 
        //     lootToMove.transform.position += lootToMove.transform.forward * Time.deltaTime * 5f; // can be any float number
        // }
    }
}
