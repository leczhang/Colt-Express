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

namespace model {
    public abstract class Card : SerializableSFSType {
      
        

        public Card() {}
        

    }
}

/* alternate method of querying cards - dont delete for now

ArrayList cards = b.deck.Clone();
               cards.AddRange(b.hand.Clone());
               cards.AddRange(b.discardPile.Clone());
               foreach(Card c in cards) {
                    if (c.Equals(this)){
                        return b;
                    }
               }*/