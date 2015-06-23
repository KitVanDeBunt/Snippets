using UnityEngine;

namespace BigBossBattle {
    //[RequireComponent(typeof(BossMovement))]
    public class Boss:MonoBehaviour {

        private bool hitDelegateAdded = false;
        public BossAnimation bossAnimation;
        public BossState bossState;

        private BossAIState bossAIstate_ = BossAIState.spawnig;

        [SerializeField]
        private BossBehaviourSet[] bossBehaviours;

        private BossBehaviourSet HitBehaviourSet;

        private BossBehaviourSet currentBossBehaviourSet;

        private BossBehaviour[] currentBossBehaviours;

        [SerializeField]
        private Material bossMaterial;
        [SerializeField]
        private Color32 colorNormal = Color.white;
        [SerializeField]
        private Color32 colorHit = Color.red;

        private bool inHit = false;
        
        private int i;
        private int j;

        public BossType bossType;

        private float hitTime;
        private float startHitTime;
        private int hitCount = 0;

        public BossAIState bossAIstate {
            get {
                return bossAIstate_;
            }
        }

        public void CallBossEvent(BossEventMessage eventMessage,int callerInstanceID) {

            for (i = 0; i < currentBossBehaviours.Length; i++) {
                currentBossBehaviours[i].BossEvent(eventMessage, callerInstanceID);
            }
        }

        public void SpawnBoss() {
            //ani.SetTrigger("spawn");
        }

        private void BossHit() {
            hitCount++;
            

            inHit = true;
            hitTime = bossState.getHitTime;
            startHitTime = bossState.getHitTime;

            // start boss hit behaviour if avalable
            if (hitCount > 2) {
                //end behaviours
                if (HitBehaviourSet != null) {
                    for (i = 0; i < currentBossBehaviours.Length; i++) {
                        currentBossBehaviours[i].End();
                    }
                    //startHitBehaviour
                    currentBossBehaviourSet = HitBehaviourSet;
                    ProgressInBehaviourSet = 0;
                    NextBehavioursFromSet();
                    //= new BossBehaviour[] { HitBehaviourSet };
                    currentBossBehaviours[0].Init(this);
                }
                
                hitCount = 0;
            }
        }

        private void BossHitEnd() {
            inHit = false;
        }

        private void OnEnable() {
            AddOnHit();
        }
        private void OnDisable() {
            RemoveOnHit();
        }

        private void AddOnHit() {
            if (!hitDelegateAdded) {
                bossState.onHit += BossHit;
                bossState.onHitEnd += BossHitEnd;
                hitDelegateAdded = true;
            }
        }
        private void RemoveOnHit() {
            if (hitDelegateAdded) {
                bossState.onHit -= BossHit;
                bossState.onHitEnd -= BossHitEnd;
                hitDelegateAdded = false;
            }
        }

        //call by animator/BossAnimation to start boss behaviour
        public void StartBoss() {

            bossMaterial.color = colorNormal;

            //skeletonAnimation.
        
            bossAIstate_ = BossAIState.active;
            NewBehaviour();

            //get hit behaviour
            bool hitBehaviourFound = false;
            for (int i = 0; i < bossBehaviours.Length; i++) {
                for (j = 0; j < bossBehaviours.Length; j++) {
                    if (bossBehaviours[i].onHitBehaviourSet) {
                        HitBehaviourSet = bossBehaviours[i];
                        hitBehaviourFound = true;
                        break;
                    }
                    if (hitBehaviourFound) {
                        break;
                    }
                }
            }
            AddOnHit();
        }

        public void EndBoss() {
            RemoveOnHit();

            //Debug.Log("EndBoss");
            bossAIstate_ = BossAIState.death;
            for (i = 0; i < currentBossBehaviours.Length; i++) {
                currentBossBehaviours[i].End();
            }
            bossAnimation.SetAnimation(0,BossAnimation.ANIMATION_B1_DEATH, false);
            //ani.SetTrigger("death");
        }

        private void Update(){
            hitTime -= Time.deltaTime;
            if (inHit) {

                bossMaterial.color = Color32.Lerp(colorNormal, colorHit, (hitTime / startHitTime));
            } else {
                //bossMaterial.color = Color32.Lerp(new Color32(0xff, 0xff, 0xff, 0xff), new Color32(0xff, 0xff, 0xff, 0x00), 0.0f);
            }


            switch (bossAIstate_) {
                case BossAIState.spawnig:

                    break;
                case BossAIState.active:
                    
                    if (currentBossBehaviours != null) {

                        //update boss behaviours
                        for (i = 0; i < currentBossBehaviours.Length; i++) {
                            currentBossBehaviours[i].Tick();
                        }
                        //check if currentBossBehaviours are done
                        bool behavioursDone = true;
                        for (i = 0; i < currentBossBehaviours.Length; i++) {
                            if (!currentBossBehaviours[i].behaviourDone) {
                                behavioursDone = false;
                                break;
                            }
                        }
                        if (behavioursDone) {
                            NewBehaviour();
                        }
                    }
                    break;

                case BossAIState.death:
                    
                    break;
            }
        }

        private int ProgressInBehaviourSet = 0;

        private void NewBehaviour() {
            if (currentBossBehaviours != null) {
                //Debug.Log("new behaviour:" + ProgressInBehaviourSet + " | " + (currentBossBehaviourSet.behaviourSet.Length - 2));
            }
            if (currentBossBehaviours == null || ProgressInBehaviourSet > currentBossBehaviourSet.behaviourSet.Length - 2) {
                //new set

                //Debug.Log("new set");
                ProgressInBehaviourSet = 0;
                int totalWeight = 0;
                for (int i = 0; i < bossBehaviours.Length; i++) {
                    totalWeight += bossBehaviours[i].setWeight;
                }
                int start = 0;
                int spawnInt = Random.Range(start, totalWeight);
                int spawnCounter = 0;

                for (int i = 0; i < bossBehaviours.Length; i++) {
                    spawnCounter += bossBehaviours[i].setWeight;
                    if (spawnInt < spawnCounter) {

                        //set new behavoiur set
                        currentBossBehaviourSet = bossBehaviours[i];
                        NextBehavioursFromSet();
                        break;
                    }
                }
            } else {
                //next in set
                //Debug.Log("next in set" + ProgressInBehaviourSet);
                ProgressInBehaviourSet++;
                NextBehavioursFromSet();

            }
        }

        private void NextBehavioursFromSet() {
            //set new behaviours
            currentBossBehaviours = currentBossBehaviourSet.behaviourSet[ProgressInBehaviourSet].behaviour;

            //init behaviours
            for (i = 0; i < currentBossBehaviours.Length; i++) {
                currentBossBehaviours[i].Init(this);
            }
        }

    }
}
