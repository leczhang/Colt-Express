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
    public class Horse : SerializableSFSType 
    {

        public TrainUnit adjacentTo;
        public Bandit riddenBy;
        
        // --EMPTY CONSTRUCTOR FOR SERIALIZATION--
        public Horse(){}

        public Horse(TrainUnit adjacentTo){
            this.adjacentTo = adjacentTo;
        }

        // adjacentTo
        public TrainUnit getAdjacentTo() {
            return this.adjacentTo;
        }
        
        public void setAdjacentTo(TrainUnit adjacentTo) {
            this.adjacentTo = adjacentTo;
        }     
        // riddenBy
        public Bandit getRiddenBy() {
            //return this.riddenBy.value;
            return this.riddenBy;
        }      
        public void setRiddenBy(Bandit b) {
            //TODO: whenever a Bandit is set to a horse their sprite needs to be moved onto the horse
            this.riddenBy = b;
        }
    }

}
