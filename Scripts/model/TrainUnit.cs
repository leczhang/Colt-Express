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
    public class TrainUnit : SerializableSFSType {
    
        public string carTypeAsString;
        public string carFloorAsString;
        public bool isMarshalHere;
        public ArrayList lootHere;
        
        
        // --EMPTY CONSTRUCTOR FOR SERIALIZATION--
        public TrainUnit() {}
        
        // private TrainUnit(string carType, string carFloor) {
        //     //this.carType = carType;
        //     //this.carFloor = carFloor;
        //     this.carTypeAsString = carType;
        //     this.carFloorAsString = carFloor;
        //     // TODO: createGraphic()
        // }
        
        // carType
        // public string getCarType() {
        //     return this.carType;
        // }
        
        // public void setCarType(string type) {
        //     this.carType = type;
        // }
        
        // carTypeAsString
        public string getCarTypeAsString() {
            return this.carTypeAsString;
        }
        
        public void setCarTypeAsString(string type) {
            this.carTypeAsString = type;
        }
        
        // carFloor
        // public string getCarFloor() {
        //     return this.carFloor;
        // }
        
        // public void setCarFloor(string floor) {
        //     this.carFloor = floor;
        // }
        
        // carFloorAsString
        public string getCarFloorAsString() {
            return this.carFloorAsString;
        }
        
        public void setCarFloorAsString(string floor) {
            this.carFloorAsString = floor;
        }
        
        // // trainLength
        // public static int getTrainLength() {
        //     return TrainUnit.trainLength;
        // }
        
        // public static void setTrainLength(int length) {
        //     TrainUnit.trainLength = length;
        // }
        
        // // train
        // public ArrayList getTrain() {
        //     return train;
        // }
        
        // // stagecoach
        // public static TrainUnit[] getStagecoach() {
        //     return TrainUnit.stagecoach;
        // }
        
        public TrainUnit getAbove() {
            GameManager gm = GameManager.getInstance();
            int index = gm.trainCabin.IndexOf(this);
            if (index == -1) { return null; }
            TrainUnit above = (TrainUnit) gm.trainRoof[index];
            return above;
        }
        

        // get above/get below logic: query the hashtable, find the element in the roof or cabin arraylists in the GM, and treat as 2D array
        /*public void setAbove(TrainUnit otherTrainUnit) {
            this.above = otherTrainUnit;
            otherTrainUnit.below = this;
        }*/
        
        public TrainUnit getBelow() {
            GameManager gm = GameManager.getInstance();
            int index = gm.trainRoof.IndexOf(this);
            if (index == -1) { return null; }
            TrainUnit below = (TrainUnit) gm.trainCabin[index];
            return below;
        }
        
        /*public void setBelow(TrainUnit otherTrainUnit) {
            this.below = otherTrainUnit;
            otherTrainUnit.above = this;
        }*/
        
        public TrainUnit getLeft() {
            GameManager gm = GameManager.getInstance();
            int index = gm.trainCabin.IndexOf(this);
            bool isRoof = false;
            if (index == -1) {
                index = gm.trainRoof.IndexOf(this);
                if (index > -1) {
                    isRoof = true;
                }
                else {
                    return null;
                }
            }
            int size = gm.trainCabin.Count;
            //Debug.Log("cabin count in left: " + size);
            //Debug.Log("index of train in left: " + index);
            if (index + 1 < size) {
                if (isRoof) {
                    return (TrainUnit)gm.trainRoof[index+1];
                }
                else {
                    return (TrainUnit)gm.trainCabin[index+1];
                }
            }
            Debug.Log("returning null");
            return null;
        }
        
        /*public void setRight(TrainUnit otherTrainUnit) {
            this.right = otherTrainUnit;
            otherTrainUnit.left = this;
        }*/
        
        public TrainUnit getRight() {
            GameManager gm = GameManager.getInstance();
            int index = gm.trainCabin.IndexOf(this);
            bool isRoof = false;
            if (index == -1) {
                index = gm.trainRoof.IndexOf(this);
                if (index > -1) {
                    isRoof = true;
                }
                else {
                    return null;
                }
            }
            int size = gm.trainCabin.Count;
            //Debug.Log("cabin count in right: " + size);
            //Debug.Log("index of train in right: " + index);
            if (index-1  >= 0) {
                if (isRoof) {
                    return (TrainUnit)gm.trainRoof[index-1];
                }
                else {
                    return (TrainUnit)gm.trainCabin[index-1];
                }
            }
            Debug.Log("returning null");
            return null;
        }
        
        /*public void setLeft(TrainUnit otherTrainUnit) {
            this.left = otherTrainUnit;
            otherTrainUnit.right = this;
        }*/
        
        /*public TrainUnit getBeside() {
            GameManager gm = GameManager.getInstance();
            TrainUnit beside = 
            return beside;
        }*/
        
        //similar logic as above, below, left right
        public bool isAdjacentTo(TrainUnit otherTrainUnit) {
            if (otherTrainUnit == this.getAbove() || otherTrainUnit == this.getBelow() || otherTrainUnit == this.getLeft() || otherTrainUnit == this.getRight()) {
                return true;
            }
            return false;
        }
        
        //change the hashmap for bandit stuff
        public void addBandit(Bandit b) {
            GameManager gm = GameManager.getInstance();
            gm.banditPositions[b.characterAsString] = this;
        }
        
        /*public void removeBandit(Bandit b) {
            System.Diagnostics.Trace.Assert(banditsHere.Contains(b));
            this.banditsHere.Remove(b);
        }*/
        
        //query hashmap
        public bool containsBandit(Bandit b) {
            GameManager gm = GameManager.getInstance();
            if (gm.banditPositions[b.characterAsString] == this) {
                return true;
            }
            return false;
        }
        
        public int numOfBanditsHere() {
            return this.getBanditsHere().Count;
        }
        
        public ArrayList getBanditsHere() {
            GameManager gm = GameManager.getInstance();
            ArrayList banditsArr = new ArrayList();
            foreach (Bandit b in gm.bandits) {
                foreach (DictionaryEntry de in gm.banditPositions) {
                    TrainUnit t = (TrainUnit) de.Value;
                    if (b.characterAsString.Equals(de.Key) && de.Value.Equals(this)) {
                        banditsArr.Add(b);
                        break;
                    }
                }
            }
            return banditsArr;
        }
        
        public void addLoot(Loot a) {
            System.Diagnostics.Trace.Assert(!this.lootHere.Contains(a));
            this.lootHere.Add(a);
        }
        
        public void removeLoot(Loot a) {
            System.Diagnostics.Trace.Assert(this.lootHere.Contains(a));
            this.lootHere.Remove(a);
        }
        
        public bool containsLoot(Loot a) {
            return this.lootHere.Contains(a);
        }
        
        public int numOfLootHere() {
            return this.lootHere.Count;
        }
        
        public ArrayList getLootHere(){
            return this.lootHere;
        }

        public bool getIsMarshalHere() {
            return this.isMarshalHere;
        }
        
        public void setIsMarshalHere(bool b) {
            this.isMarshalHere = b;
        }
        
        // public void moveMarshalTo(TrainUnit dest) {
        //     this.isMarshalHere = false;
        //     dest.isMarshalHere = true;
        // }

        public ArrayList getHorsesHere(){
            GameManager gm = GameManager.getInstance();
            ArrayList horsesHere = new ArrayList();
            foreach(Horse h in gm.horses){
                if(h.getAdjacentTo().Equals(this)){
                    horsesHere.Add(h);
                }
            }
            return horsesHere;
        }
    }
}