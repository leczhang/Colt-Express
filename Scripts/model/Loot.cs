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
    public abstract class Loot : SerializableSFSType {
        // --EMPTY CONSTRUCTOR FOR SERIALIZATION--
        public Loot() {}
        
        
        public void drop() {
            // TODO
        }
        
        public void pickup() {
            // TODO
        }

        public Bandit getBelongsTo(){
            GameManager gm = GameManager.getInstance();
            foreach(Bandit b in gm.bandits){
                if(b.containsLoot(this)){
                    Debug.Assert(this.getPosition() == null);
                    return b;
                }
            }
            return null;
        }

        public TrainUnit getPosition(){
            GameManager gm = GameManager.getInstance();
            foreach(TrainUnit t in gm.trainRoof){
                if(t.containsLoot(this)){
                    Debug.Assert(this.getBelongsTo() == null);
                    return t;
                }
            }
            foreach(TrainUnit t in gm.trainCabin){
                if(t.containsLoot(this)){
                    Debug.Assert(this.getBelongsTo() == null);
                    return t;
                }
            }
            foreach(TrainUnit t in gm.stagecoach){
                if(t.containsLoot(this)){
                    Debug.Assert(this.getBelongsTo() == null);
                    return t;
                }
            }
            return null;
        }
    }
}
