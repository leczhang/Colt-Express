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
    public class PlayedPile : SerializableSFSType {
    
        public static PlayedPile instance;
        public ArrayList playedCards = new ArrayList();
        
        // --EMPTY CONSTRUCTOR FOR SERIALIZATION--
        public PlayedPile() {}
        
        public void addPlayedCardsAt(int index, ActionCard a) {
            this.playedCards.Insert(index, a);
        }

        public void removePlayedCardsAt(int index) {
            if ((this.playedCards.Count > index)) {
                this.playedCards.RemoveAt(index);
            }
        }

        public ActionCard getPlayedCardsAt(int index) {
            if (this.playedCards.Count > index) {
                return (ActionCard)this.playedCards[index];
            }
            return null;
        }

        public void addPlayedCards(ActionCard a) {
            this.playedCards.Insert(0, a);
        }
        
        public void removePlayedCard(ActionCard a) {
           this.playedCards.Remove(a);
        }
        
        public bool containsPlayedCard(ActionCard a) {
            bool contains = this.playedCards.Contains(a);
            return contains;
        }
        
        public int sizeOfPlayedCards() {
            int size = this.playedCards.Count;
            return size;
        }
        
        public ArrayList getPlayedCards() {
            return this.playedCards;
        }
        
        public ActionCard takeTopCard() {
            ActionCard top = this.getPlayedCardsAt(this.playedCards.Count-1);
    	    this.removePlayedCardsAt(this.playedCards.Count-1);
            return top;
        }

        public static PlayedPile getInstance() {
            if ((instance == null)) {
                instance = new PlayedPile();
            }
            return instance;
        }
    }
}