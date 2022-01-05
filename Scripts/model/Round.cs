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
    public class Round : SerializableSFSType {
    
        public string roundTypeAsString;  
        public Turn currentTurn;  
        public int turnCounter; // Tracks the current turn
        public ArrayList turns ;
        
        // --EMPTY CONSTRUCTOR FOR SERIALIZATION--
        public Round() {}

        public Round(string rt) {
            int numOfBandits = GameManager.getInstance().bandits.Count;
            Debug.Assert(numOfBandits >= 2 && numOfBandits <= 6);
    	    this.roundTypeAsString = rt;
            this.turnCounter = 0;
            this.turns = new ArrayList();
            if(rt.Equals("AngryMarshal") && numOfBandits <= 4){
                turns.Add(new Turn("STANDARD"));
                turns.Add(new Turn("STANDARD"));
                turns.Add(new Turn("TUNNEL"));
                turns.Add(new Turn("SWITCHING"));
            }
            else if(rt.Equals("SwivelArm") && numOfBandits <= 4){
                turns.Add(new Turn("STANDARD"));
                turns.Add(new Turn("TUNNEL"));
                turns.Add(new Turn("STANDARD"));
                turns.Add(new Turn("STANDARD"));
            }
            else if(rt.Equals("Braking") && numOfBandits <= 4){
                turns.Add(new Turn("STANDARD"));
                turns.Add(new Turn("STANDARD"));
                turns.Add(new Turn("STANDARD"));
                turns.Add(new Turn("STANDARD"));
            }
            else if(rt.Equals("TakeItAll") && numOfBandits <= 4){
                turns.Add(new Turn("STANDARD"));
                turns.Add(new Turn("TUNNEL"));
                turns.Add(new Turn("SPEEDINGUP"));
                turns.Add(new Turn("STANDARD"));
            }
            else if(rt.Equals("PassengersRebellion") && numOfBandits <= 4){
                turns.Add(new Turn("STANDARD"));
                turns.Add(new Turn("STANDARD"));
                turns.Add(new Turn("TUNNEL"));
                turns.Add(new Turn("STANDARD"));
                turns.Add(new Turn("STANDARD"));
            }
            else if(rt.Equals("Bridge") && numOfBandits <= 4){
                turns.Add(new Turn("STANDARD"));
                turns.Add(new Turn("SPEEDINGUP"));
                turns.Add(new Turn("STANDARD"));
            }
            else if(rt.Equals("Cave") && numOfBandits <= 4){
                turns.Add(new Turn("STANDARD"));
                turns.Add(new Turn("TUNNEL"));
                turns.Add(new Turn("STANDARD"));
                turns.Add(new Turn("TUNNEL"));
                turns.Add(new Turn("STANDARD"));
            }
            else if(rt.Equals("AngryMarshal")){
                turns.Add(new Turn("STANDARD"));
                turns.Add(new Turn("STANDARD"));
                turns.Add(new Turn("SWITCHING"));
            }
            else if(rt.Equals("SwivelArm")){
                turns.Add(new Turn("STANDARD"));
                turns.Add(new Turn("TUNNEL"));
                turns.Add(new Turn("STANDARD"));
            }
            else if(rt.Equals("Braking")){
                turns.Add(new Turn("STANDARD"));
                turns.Add(new Turn("TUNNEL"));
                turns.Add(new Turn("STANDARD"));
                turns.Add(new Turn("STANDARD"));
            }
            else if(rt.Equals("TakeItAll")){
                turns.Add(new Turn("STANDARD"));
                turns.Add(new Turn("SPEEDINGUP"));
                turns.Add(new Turn("SWITCHING"));
            }
            else if(rt.Equals("PassengersRebellion")){
                turns.Add(new Turn("STANDARD"));
                turns.Add(new Turn("TUNNEL"));
                turns.Add(new Turn("STANDARD"));
                turns.Add(new Turn("SWITCHING"));
            }   
            else if(rt.Equals("Bridge")){
                turns.Add(new Turn("STANDARD"));
                turns.Add(new Turn("SPEEDINGUP"));
            }
            else if(rt.Equals("Cave")){
                turns.Add(new Turn("STANDARD"));
                turns.Add(new Turn("TUNNEL"));
                turns.Add(new Turn("STANDARD"));
                turns.Add(new Turn("TUNNEL"));
            }
            this.currentTurn = (Turn)this.turns[0];
        }
        
        public void addTurn(Turn a) {
            this.turns.Add(a);
        }
        public void removeTurn(Turn a) {
            this.turns.Remove(a);
        }        
        public Turn getTurnAt(int index) {
            if ((index < this.turns.Count)) {
                return (Turn)this.turns[index];
            }
            return null;
        }
        public int sizeOfTurns() {
            int size = this.turns.Count;
            return size;
        }
        
        public Turn getCurrentTurn() {
            return this.currentTurn;
        }
        public void setCurrentTurn(Turn turn){
            this.currentTurn = turn;
        }

        public bool hasNextTurn(){
            if(this.turnCounter+1 < this.turns.Count){
                return true;
            }
            else{
                return false;
            }
        }
        public Turn getNextTurn() {
            if (this.turnCounter+1 < this.turns.Count){
                return (Turn)this.turns[this.turnCounter++];
            }
            else{
                return null;
            }
        }
        public void setNextTurn() {
            this.turnCounter++;
            this.currentTurn = (Turn)this.turns[this.turnCounter];
        }
        
        public int getTurnCounter() {
            return this.turnCounter;
        }
        public void setTurnCounter(int i) {
            this.turnCounter = i;
        }
    }
}