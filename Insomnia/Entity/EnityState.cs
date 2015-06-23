using UnityEngine;

namespace BigBossBattle {
    public abstract class EntityState :UnityEngine.MonoBehaviour{
        
        public delegate void HitEvent();
        public HitEvent onHit;
        public HitEvent onLifeChange;
        public HitEvent onHitEnd;

        [SerializeField]
        protected PlayerMove playerMove;
        [SerializeField]
        protected int health_ = 3;
        [SerializeField]
        protected int life_ = 1;

        protected bool death_ = false;

        private bool inHit_ = false;
        private float hitTime_ = 0.0f;

        [SerializeField]
        private bool useHitDelay = false;


        public float getHitTime {
            get {
                return hitTime_;
            }
        }
        public bool inHit {
            get {
                return inHit_;
            }
        }

        public bool death {
            get {return death_; }
        }

        public int health {
            get {return health_;}
        }

        public int life {
            get { return life_; }
        }

        public void AddHealth(uint add = 1) {
            health_ += (int)add;
        }

        public void Hit(uint damage = 1, Vector2? power = null, float hitTime = 0.5f) {
            //Debug.Log(GetInstanceID() + " hitTime" + hitTime);
            hitTime_ = hitTime; 

            bool stop = false;
            if (inHit_) {
                //CancelInvoke("HitEnd");
                if (useHitDelay) {
                    stop = true;
                }
            } else {
                Invoke("HitEnd", hitTime);
                
            }
            inHit_ = true;
            if (!stop) {
                if (power != null) {
                    playerMove.Hit(power.Value, hitTime);
                }
                health_ -= (int)damage;
                OnHealtChange();
                if (health_ < 1) {
                    life_ -= 1;
                    if (life_ > 0) {
                        OnRemoveLife();
                        if (onLifeChange != null) {
                            onLifeChange();
                        }
                    } else {
                        OnRemoveLife();
                        death_ = true;
                        Death();
                    }
                }
                
            }
            if (onHit != null) {
                onHit();
            }
        }

        public void HitEnd(){
            inHit_ = false;
            if (onHitEnd != null) {
                onHitEnd();
            }
        }

        public abstract void Death();

        public abstract void OnRemoveLife();

        public abstract void OnHealtChange();

    }
}
