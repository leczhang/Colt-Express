using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sfs2X;
using Sfs2X.Logging;
using Sfs2X.Util;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Protocol.Serialization;


//The following code is executed right after creating the SmartFox object:
// using System.Reflection;
//        DefaultSFSDataSerializer.RunningAssembly = Assembly.GetExecutingAssembly();
namespace model {
    public class Whiskey : Loot, SerializableSFSType {
    
        public string whiskeyTypeAsString;
        public string whiskeyStatusAsString;
        
        // --EMPTY CONSTRUCTOR FOR SERIALIZATION--
        public Whiskey() {}
        
        public Whiskey(string whiskeyTypeAsString){
            this.whiskeyTypeAsString = whiskeyTypeAsString;
            this.whiskeyStatusAsString = "UNFLIPPED";
        }
        
        // whiskeyTypeAsString
        public string getWhiskeyTypeAsString() {
            return this.whiskeyTypeAsString;
        }
        
        public void setWhiskeyTypeAsString(string s) {
            if(s.Equals("NORMAL") ^ s.Equals("OLD")) {
                this.whiskeyTypeAsString = s;
            }
            else{
                Debug.Log("INVALID WHISKEY TYPE SET");
            }
        }
        
        // whiskeyStatusAsString
        public string getWhiskeyStatusAsString() {
            return this.whiskeyStatusAsString;
        }
        
        public void setWhiskeyStatusAsString(string s) {
            if(s.Equals("FLIPPED") ^ s.Equals("UNFLIPPED")){
                this.whiskeyStatusAsString = s;
            }
            else{
                Debug.Log("INVALID WHISKEY STATUS SET");
            }
        }
    }
}