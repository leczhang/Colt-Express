using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Sfs2X;
using Sfs2X.Logging;
using Sfs2X.Util;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Protocol.Serialization;
using Random = System.Random;

//The following code is executed right after creating the SmartFox object:
// using System.Reflection;
//        DefaultSFSDataSerializer.RunningAssembly = Assembly.GetExecutingAssembly();
namespace model {

    public class Bandit : SerializableSFSType {
    
        public string characterAsString;
        public string hostageAsString;
        public ArrayList loot;      
        public ArrayList bullets;        
        public ArrayList deck;
        public ArrayList deckAC;
        public ArrayList deckBC;
        public ArrayList hand;
        public ArrayList handAC;
        public ArrayList handBC;
        public ActionCard toResolve;
        public int consecutiveTurnCounter;
        
        // --EMPTY CONSTRUCTOR FOR SERIALIZATION--
        public Bandit() {}

        public Bandit(string c) {
            this.characterAsString = c;
            this.hostageAsString = null;
            this.loot = new ArrayList();
            this.bullets = new ArrayList();
            this.deck = new ArrayList();
            this.hand = new ArrayList();
            this.toResolve = null;
            this.consecutiveTurnCounter = 0;
        }
        
        /**
        *  --GETTERS AND SETTERS--
        */

        //character
        public string getCharacter() {
            return this.characterAsString;
        }
        
        //position
        public TrainUnit getPosition() {
            GameManager gm = GameManager.getInstance();
            TrainUnit pos = (TrainUnit)gm.banditPositions[this.characterAsString];
            return (TrainUnit)pos;
        }
        public void setPosition(TrainUnit newObject) {
            //change hashmap of bandit-position
            GameManager gm = GameManager.getInstance();
            gm.banditPositions[this.characterAsString] = newObject;
        }
        
        //loot
        public Loot getLootAt(int index) {
            if ((this.loot.Count > index)) {
                Loot a = (Loot)this.loot[index];
                return a;
            }
            return null;
        }
        public void addLoot(Loot a) {
            this.loot.Add(a);
        }
        public void removeLoot(Loot a) {
            this.loot.Remove(a);
        }
        public ArrayList getLoot() {
            return this.loot;
        }
        public bool containsLoot(Loot l){
            if(this.loot.Contains(l)){
                return true;
            }
            else{
                return false;
            }
        }
        
        //deck
        public void addToDeckAt(int index, Card a) {
            bool contains = this.deck.Contains(a);
            if (contains) {
                return;
            }
            this.deck.Insert(index, a);
        }
        public void removeFromDeckAt(int index) {
            this.deck.RemoveAt(index);
        }
        public Card getFromDeckAt(int index) {
            if ((this.deck.Count > index)) {
                return (Card)this.deck[index];
            }
            return null;
        }
        public void addToDeck(Card a) {
            this.deck.Add(a);
        }
        public void removeFromDeck(Card a) {
            this.deck.Remove(a);
        }
        public int sizeOfDeck() {
            int size = this.deck.Count;
            return size;
        }
        public ArrayList getDeck() {
            return this.deck;
        }
        
        //hand
        public void addToHand(Card a) {
            this.hand.Add(a);
            
        }
        public void removeFromHand(Card a) {
            this.hand.Remove(a);

        }
        public int sizeOfHand() {
            int size = this.hand.Count;
            return size;
        }
        public ArrayList getHand() {
            return this.hand;
        }
        
        //bullets
        public void addBullet(BulletCard b){
            this.bullets.Add(b);
        }
        public BulletCard popBullet(){
            BulletCard popped = (BulletCard) this.bullets[this.bullets.Count-1];
            this.bullets.Remove(popped);
            return popped;
        }
        public int getSizeOfBullets(){
            return this.bullets.Count;
        }
        
        //hostage
        public string getHostageAsString() {
            return this.hostageAsString;
        }
        
        public void setHostageAsString(string hostage) {
            this.hostageAsString = hostage;
        }

        //toResolve
        public ActionCard getToResolve() {
        return this.toResolve;
        }
        public void setToResolve(ActionCard ac) {
            this.toResolve = ac;
        }

        //consecutiveTurnCounter
        public int getConsecutiveTurnCounter() {
		    return this.consecutiveTurnCounter;
	    }
	    public void setConsecutiveTurnCounter(int i) {
		    this.consecutiveTurnCounter = i;
	    }
        
        public void clearHand(){
            foreach (Card c in this.hand){
                this.deck.Add(c);
            }
            this.hand.Clear();
        }

     // FIX THIS
        public void shuffle() {
            System.Random RandomGen = new System.Random(DateTime.Now.Millisecond);
            ArrayList ScrambledList = new ArrayList();
            Int32 Index;
            while (this.deck.Count > 0)
            {
                Index = RandomGen.Next(this.deck.Count);
                ScrambledList.Add(this.deck[Index]);
                this.deck.RemoveAt(Index);
            }
            this.deck = ScrambledList;
        } 

        public static ArrayList shuffle(ArrayList c) {
            c = (ArrayList) c.Clone();
            Random RandomGen = new Random(DateTime.Now.Millisecond);
            ArrayList ScrambledList = new ArrayList();
            Int32 Index;
            while (c.Count > 0)
            {
                Index = RandomGen.Next(c.Count);
                ScrambledList.Add(c[Index]);
                c.RemoveAt(Index);
            }
            return ScrambledList;
        }

        public void drawCards(int cardsToDraw) {
            Debug.Log("cards to draw: " + cardsToDraw);
            int deckSize = this.sizeOfDeck();
            if(deckSize == 0){
                return;
            }
            else if(deckSize <= cardsToDraw){
                for (int i = 0; i<deckSize; i++) {
                    Card toAdd = this.getFromDeckAt(sizeOfDeck()-1);
                    this.removeFromDeckAt(sizeOfDeck()-1);
                    this.addToHand(toAdd);
                }
            }
            else{
                for (int i = deckSize-1; i > deckSize-cardsToDraw-1; i--) {
                    Card toAdd = this.getFromDeckAt(i);
                    this.removeFromDeckAt(i);
                    this.addToHand(toAdd);
                }
            }
        }

        public void shotByMarhsal(){
            GameManager gm = GameManager.getInstance();
            if(gm.neutralBulletCard.Count > 0){
                this.addToDeck(gm.popNeutralBullet());
                Debug.Assert(this.getPosition().getCarFloorAsString().Equals("CABIN") && this.getPosition().getAbove() != null);
                this.setPosition(this.getPosition().getAbove());
            }
        }

        public ArrayList getBulletCards(){
            return this.bullets;
        }



        public void updateMainDeck(){
            deck.Clear();
            foreach (ActionCard c in deckAC) {
                deck.Add(c);
            }
            foreach (BulletCard c in deckBC) {
                deck.Add(c);
            }
            // SHUFFLE DECK
        }

        public void updateOtherDecks() {
            deckAC.Clear();
            deckBC.Clear();
            foreach (Card c in deck) {
                if (c is ActionCard) {
                    //Debug.Log("Adding an action card to deck");
                    deckAC.Add(((ActionCard) c));
                }
                else {
                   // Debug.Log("Adding a bullet card to deck");
                    deckBC.Add(((BulletCard) c));
                }
            }
       }

       public void updateMainHand(){
            hand.Clear();
            foreach (ActionCard c in handAC) {
                hand.Add(c);
            }
            foreach (BulletCard c in handBC) {
                hand.Add(c);
            }
            // SHUFFLE DECK
        }

        public void updateOtherHands() {
            handAC.Clear();
            handBC.Clear();
            foreach (Card c in hand) {
                if (c is ActionCard) {
                    //Debug.Log("Adding an action card to hand");
                    handAC.Add(((ActionCard) c));
                }
                else {
                    //Debug.Log("Adding a bullet card to hand");
                    handBC.Add(((BulletCard) c));
                }
            }
       }

    }

}