namespace BigBossBattle {
    [System.Serializable]
    public abstract class BossBehaviour :UnityEngine.MonoBehaviour{

        [UnityEngine.SerializeField]
        protected bool behaviourDone_ = false;
        internal Boss _boss;
        private BossBehaviourSettings settings;

        public bool onHitBehavior {
            get {
                return settings.onHitBehaviour;
            }
        }
        public bool behaviourDone {
            get {
                return behaviourDone_;
            }
        }

        public abstract void BossEvent(BossEventMessage message, int callerID);
        public abstract void Tick();
        public abstract void End();
        public virtual void Init(Boss boss){
            _boss = boss;
            behaviourDone_ = false;   
        }

        private void Awake(){
            InitSettings();
        }

        protected virtual void InitSettings(bool onHitBehaviour = false){
            //UnityEngine.Debug.Log("InitSettings onHitBehaviour: " + onHitBehaviour);
            settings = new BossBehaviourSettings(onHitBehaviour);
        }
    }
}

