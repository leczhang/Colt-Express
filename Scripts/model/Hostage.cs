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
    public class Hostage : SerializableSFSType {
    
        public string hostageTypeAsString;
        
        // --EMPTY CONSTRUCTOR FOR SERIALIZATION--
        public Hostage() {}
        
        public Hostage(string hostageTypeAsString){
            this.hostageTypeAsString = hostageTypeAsString;
        }
        
        // hostageTypeAsString
        public string getHostageTypeAsString() {
            return this.hostageTypeAsString;
        }
        
        public void setHostageTypeAsString(string hostage) {
            if(hostage.Equals("POODLE") ||
            hostage.Equals("MINISTER") ||
            hostage.Equals("TEACHER") ||
            hostage.Equals("ZEALOT") ||
            hostage.Equals("OLDLADY") ||
            hostage.Equals("POKERPLAYER") ||
            hostage.Equals("PHOTOGRAPHER")){
                this.hostageTypeAsString = hostage;
            }
            else{
                Debug.Log("INVALID HOSTAGE TYPE SET");
            }
        }
    }
}
