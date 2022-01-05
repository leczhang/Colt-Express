using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.EventSystems;

public class Rob : MonoBehaviour
{
    public string announcement = "ROB: You can take a Loot token of your choice from the Car where you are currently located.";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetMouseButtonDown(0))
        // {
        //     var selectedLoot = EventSystem.current.currentSelectedGameObject;
        //     selectedLoot.transform.position = new Vector3 (1219, 819, -364);
        //     selectedLoot.transform.position += selectedLoot.transform.forward * Time.deltaTime * 5f; // can be any float number
        //     selectedLoot.SetActive(false);
        //     Destroy(selectedLoot);
        // }
    }

    public void OnButtonClick()
     {
         var selectedLoot = EventSystem.current.currentSelectedGameObject;
         if (selectedLoot != null)
             Debug.Log("Clicked on : "+ selectedLoot.name);
         else
             Debug.Log("curr game obj is null :(");
     }

    // 
    
    // 
}
