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
    public class BulletCard : Card, SerializableSFSType {
    
        public bool fired;
        public string belongsToAsString;

        // --EMPTY CONSTRUCTOR FOR SERIALIZATION--
        public BulletCard() {}

        public BulletCard(string belongsToAsString) {
            this.belongsToAsString = belongsToAsString;
            this.fired = false;
        }

        //belongsTo
        public Bandit getBelongsTo() {
           GameManager gm = GameManager.getInstance();
           foreach (Bandit b in gm.bandits) {
               if (b.characterAsString.Equals(this.belongsToAsString)) {
                   return b;
               }
           }
           return null;
       }

        //fired
        public bool getFired(){
            return this.fired;
        }

        public void setFired(bool b){
            this.fired = b;
        }

        //belongsToAsString
        public string getBelongsToAsString(){
            return this.belongsToAsString;
        }

       public void setBelongsToAsString(string belongsTo) {
           if(belongsTo.Equals("GHOST")||belongsTo.Equals("DOC")||belongsTo.Equals("TUCO")||belongsTo.Equals("CHEYENNE")||belongsTo.Equals("BELLE")||belongsTo.Equals("DJANGO")){
               this.belongsToAsString = belongsTo;
           }
           else{
               Debug.Log("CARD SET TO INVALID CHARACTER");
           }
       }



    }
}
