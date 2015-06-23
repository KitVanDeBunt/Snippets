using UnityEngine;

namespace BigBossBattle {
    [System.Serializable]
    public class BossBehaviourMove : BossBehaviour {
        [SerializeField]
        private Transform bossPos;
        [SerializeField]
        private float speed = 10;
        [SerializeField]
        [Range(0f, 2f)]
        private float sloopDist = 0.3f;
        [SerializeField]
        [Range(1f, 20f)]
        private float sloopPower = 5f;
        [SerializeField]
        private Transform[] points;


        private Vector3 startPosition;
        private Vector3 endPosition;
        private float interpolation;
        private float dist;
        private float speedMult;
        private float standaartDist = 2f;
        /*
        private float standaartDist = 2f;
        
        private float speedMult;
        private float sloopTop = 1f;
        private float currentSloopLength = 1f;
        */
        private MoveUtils.SmoothInterpolate interpolator;
        private int previousPoint = -1;

        public override void BossEvent(BossEventMessage message, int callerID) {

        }

        protected override void InitSettings(bool onHitBehaviour) {
            base.InitSettings(true);
        }

        public override void Tick() {
            if (!behaviourDone_) {
                
                /*float distTravled = Vector3.Distance(startPosition, boss.position) ;

                bool startSloop = (distTravled < currentSloopLength);
                bool startEnd = (distTravled > (dist - currentSloopLength));
                float sloopProgress;
                float sloopSpeedMult = 1f;
                if (startSloop){
                    sloopProgress = (distTravled / currentSloopLength);
                    sloopSpeedMult = Mathf.Lerp(0.001f, sloopTop, sloopProgress);
                }else if (startEnd){
                    sloopProgress = ((distTravled - (dist - currentSloopLength)) / currentSloopLength);
                    sloopSpeedMult = Mathf.Lerp(sloopTop, 0.001f, sloopProgress);
                }

                //Debug.Log("distTravled" + distTravled + "\nsloopSpeedMult: " + sloopSpeedMult);
                //interpolation += ((Time.deltaTime / speedMult) * speed * (sloopSpeedMult * sloopPower));
                 */
                //boss.position = Vector3.Lerp(startPosition, endPosition, interpolation);
                interpolation = interpolator.GetCurrentPosition(bossPos.position);
                bossPos.position = Vector3.Lerp(startPosition, endPosition, interpolation);
                //Debug.Log(boss.position + "\n" + interpolation);
                if (interpolation > 1) {
                    behaviourDone_ = true;
                }
            }
        }

        public override void End() {
            behaviourDone_ = true;
        }

        public override void Init(Boss boss) {
            startPosition = bossPos.position;
            int nextPoint = Random.Range(0, points.Length);
            while (nextPoint == previousPoint) {
                nextPoint = Random.Range(0, points.Length);
            }
            endPosition = points[nextPoint].position;
            previousPoint = nextPoint;

            if (startPosition.x > endPosition.x) {
                boss.bossAnimation.SetAnimation(0,BossAnimation.ANIMATION_B1_SLIDE_LEFT, false);
            } else {
                boss.bossAnimation.SetAnimation(0,BossAnimation.ANIMATION_B1_SLIDE_RIGHT, false);
            }

            dist = Vector3.Distance(startPosition, endPosition);

            if (dist < 0.01f) {
                behaviourDone_ = true;
            } else {
                speedMult = dist / standaartDist;
                interpolator = new MoveUtils.SmoothInterpolate(startPosition, sloopDist, dist, speed, sloopPower, speedMult);

                //Debug.Log(speedMult);
                
                interpolation = 0;
                base.Init(boss);
                /*
                //prefent slope from overlapping
                sloopTop = 1f;
                currentSloopLength = sloopDist;
                if ((sloopDist * 2f) > dist) {
                    float distHalf = dist / 2f;
                    float sloopOverlapHalf = sloopDist - distHalf;
                    currentSloopLength = sloopDist - sloopOverlapHalf;
                    sloopTop = currentSloopLength / sloopDist;
                }
                 * */
            }
        }
    }
}
