using UnityEngine;
using System.Collections;
namespace BigBossBattle {
    public class BossBehaviourChain : BossBehaviour {

        [Range(0,999)]
        [SerializeField]
        private int chainSpaneNumber;
        [SerializeField]
        private BossChainPoint[] chainPoints;
        [SerializeField]
        private GameObject chain;
        [SerializeField]
        private float speed = 1.0F;
        [SerializeField]
        [Range(1,10)]
        private int spawnChains = 1;
        [SerializeField]
        private HitTrigger playerHitTrigger;

        private BossChainPoint chosenPoint;
        private BossChainPoint rotationPoint;

        private float startTime;
        private float journeyLength;
        private float smooth = 2f;
        private float timer;
        private int chainCounter;

        public override void BossEvent(BossEventMessage message, int callerID) {
        }

        private void CastChain() {
            if (chainCounter < spawnChains) {
                _boss.bossAnimation.SetAnimation(1, BossAnimation.ANIMATION_B2_SUMMON_CHAINS, false);
                //Debug.Log("chainCounter");
                chosenPoint = chainPoints[Random.Range(0, chainPoints.Length)];
                //Debug.Log(chosenPoint);
                chain.transform.localRotation = chosenPoint.rotation;

                startTime = Time.time;
                journeyLength = Vector3.Distance(chosenPoint.chainBeginPoint.position, chosenPoint.chainEndPoint.position);
                Vector2 hitDir = new Vector2(chosenPoint.chainEndPoint.position.x - chosenPoint.chainBeginPoint.position.x,
                    chosenPoint.chainEndPoint.position.y - chosenPoint.chainBeginPoint.position.y);

                playerHitTrigger.SetHitDir(hitDir);
                
                chain.transform.position = chosenPoint.chainBeginPoint.position;

                chainCounter++;
                //Invoke("CastChain", 2f);

            } else {
                //Debug.Log("end");
                End();
            }
        }

        public override void Tick() {
            if (!behaviourDone_) {
                float distCovered = (Time.time - startTime) * speed;
                float fracJourney = ((distCovered / journeyLength) * smooth);
                if (fracJourney < 1.0f) {
                    chain.transform.position = Vector3.Lerp(
                        chosenPoint.chainBeginPoint.transform.position,
                        chosenPoint.chainEndPoint.transform.position,
                        fracJourney);
                } else {
                    CastChain();
                }
            }
        }

        public override void Init(Boss boss) {
            base.Init(boss);
            chainCounter = 0;
            CastChain();
        }

        public override void End() {
            behaviourDone_ = true;
        }
    }
}
