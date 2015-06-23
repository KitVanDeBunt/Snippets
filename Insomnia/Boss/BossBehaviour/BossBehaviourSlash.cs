using UnityEngine;

namespace BigBossBattle {
    [System.Serializable]
    public class BossBehaviourSlash : BossBehaviour {

        public Transform SlashPart;
        private Vector3 currentTargetPosition;
        private Vector3 startPosition;

        //[SerializeField]
        //private Animator ani;
        [SerializeField]
        private BossHandAnimationManager bossHandAnimationManager;

        [SerializeField]
        private float speed = 10;
        [SerializeField]
        [Range(0f, 2f)]
        private float sloopDist = 0.3f;
        [SerializeField]
        [Range(1f, 20f)]
        private float sloopPower = 5f;
        [SerializeField]
        private BossSlamPoint[] points;

        private Vector3 endPosition;
        private BossSlamPoint chosenPoint;
        private float interpolation;
        private float dist;
        private float speedMult;
        private float standaartDist = 2f;

        private MoveUtils.SmoothInterpolate interpolator;

        [SerializeField]
        private float attackRange = 0.5f;

        private int behaviourState = 0;

        [SerializeField]
        private GameObject prefabParticleSlam;

        public override void BossEvent(BossEventMessage message, int callerID) {
            if (!behaviourDone_) {
                switch (message) {
                    case BossEventMessage.SlamAttackDone:
                        if (bossHandAnimationManager.gameObject.GetInstanceID() == callerID) {
                            HitAnimationDone();
                        }
                        break;
                    case BossEventMessage.SlamAttack:
                        if (bossHandAnimationManager.gameObject.GetInstanceID() == callerID) {
                            //outdated
                            //SlamAttack();
                        }
                        break;
                    case BossEventMessage.PreSlamAttackDone:
                        if (bossHandAnimationManager.gameObject.GetInstanceID() == callerID) {
                            //Debug.Log("slamDone");
                            behaviourState++;
                        }
                        break;
                }
            }
        }

        private void HitAnimationDone() {
            //interpolation = 2f;
            behaviourState++;
            interpolation = 0;
            BackToStart();
        }

        private Vector3 slamPos;
        private void SlamAttack2() {
            //shake Screen
            Main.instance.cam.effectManager.AddShake(new EffectScreenShake(0.016f, 30, 0.05f));
            //spawn slam particle effect
            slamPos.y -= 0.2f;
            GameObject.Instantiate(prefabParticleSlam, slamPos, prefabParticleSlam.transform.rotation);

            SoundController.instance.playPreset(SoundPreset.BossEarthSlam, transform.position);
            //Main.instance.PlaySoundOneShot(slamAudioClip);
        }

        private void SlamAttack() {

            //Vector2 castPos = chosenPoint.endSlamApproximate.position;
            Vector2 castPos = chosenPoint.startSlam.position;
            Vector2 castPos2 = chosenPoint.endSlamApproximate.position;
            slamPos = castPos2;
            float castDist = Vector3.Distance(castPos, castPos2);
            //Collider2D[] colliders = Physics2D.OverlapCircleAll(castPos, attackRange);
            RaycastHit2D[] rayCastHit2D = Physics2D.CircleCastAll(castPos, attackRange, -Vector2.up, castDist);

            //Debug.Log(castPos);

            Debug.DrawLine(castPos, (castPos + (Vector2.right * attackRange)), Color.red, 1.0f);
            Debug.DrawLine(castPos, (castPos + (Vector2.up * attackRange)), Color.red, 1.0f);
            Debug.DrawLine(castPos, (castPos + (Vector2.up * -attackRange)), Color.red, 1.0f);
            Debug.DrawLine(castPos, (castPos + (Vector2.right * -attackRange)), Color.red, 1.0f);

            Debug.DrawLine(castPos2, (castPos2 + (Vector2.right * attackRange)), Color.red, 1.0f);
            Debug.DrawLine(castPos2, (castPos2 + (Vector2.up * attackRange)), Color.red, 1.0f);
            Debug.DrawLine(castPos2, (castPos2 + (Vector2.up * -attackRange)), Color.red, 1.0f);
            Debug.DrawLine(castPos2, (castPos2 + (Vector2.right * -attackRange)), Color.red, 1.0f);

            for (int i = 0; i < rayCastHit2D.Length; i++) {
                if (rayCastHit2D[i].collider.tag == "Player") {
                    //Debug.Log("hit player");
                    rayCastHit2D[i].collider.gameObject.GetComponent<PlayerState>().Hit();

                    Analytics.TriggerEvent(Analytics.Game_Level001_PlayerDamage_Slam);
                }
            }
            //chosenPoint.endSlamApproximate.po
        }

        public override void Tick() {

            if (!behaviourDone_) {
                interpolation = interpolator.GetCurrentPosition(SlashPart.position);
                SlashPart.position = Vector3.Lerp(currentTargetPosition, endPosition, interpolation);
                //Debug.Log(interpolation);
                if (interpolation > 1) {
                    if (behaviourState == 0) {
                        bossHandAnimationManager.SetAnimation(0, BossHandAnimationManager.ANIMATION_BOSS_HAND_SLAM_PREPARE, false);
                    } else if (behaviourState == 1) {
                        behaviourState++;
                        //ani.SetTrigger("slam");
                        
                        bossHandAnimationManager.SetAnimation(0, BossHandAnimationManager.ANIMATION_BOSS_HAND_SLAM, false);
                        interpolation = 0; 
                        Invoke("SlamAttack", 0.2f);
                        Invoke("SlamAttack2", 0.4f);
                    } else if (behaviourState == 2) {
                        
                    }else{
                        behaviourDone_ = true;
                    }
                }
            }
        }

        public override void End() {
            SlashPart.position = startPosition;
            behaviourDone_ = true;
            CancelInvoke();
        }

        private void BackToStart() {
            endPosition = startPosition;
            currentTargetPosition = SlashPart.position;
            if (dist < 0.01f) {
                behaviourDone_ = true;
            } else {
                speedMult = dist / standaartDist;
                interpolator = new MoveUtils.SmoothInterpolate(currentTargetPosition, sloopDist, dist, speed, sloopPower, speedMult);
                base.Init(_boss);
            }
        }

        public Transform ClosestToPlayer() {
            float closest = 999999999f;
            int closestNum = 0;
            for (int i = 0; i < points.Length; i++)
			{
                float distToPlayer = Vector3.Distance( Main.instance.playerPos, points[i].endSlamApproximate.position);
                if (distToPlayer < closest) {
                    closest = distToPlayer;
                    closestNum = i;
                    chosenPoint = points[i];
                }
			}
            return points[closestNum].transform;
        }

        public override void Init(Boss boss) {

            behaviourState = 0;
            startPosition = SlashPart.position;
            currentTargetPosition = startPosition;
            //int nextPoint = Random.Range(0, points.Length);
            endPosition = ClosestToPlayer().position;

            dist = Vector3.Distance(currentTargetPosition, endPosition);

            if (dist < 0.01f) {
                behaviourDone_ = true;
            } else {
                speedMult = dist / standaartDist;
                interpolator = new MoveUtils.SmoothInterpolate(currentTargetPosition, sloopDist, dist, speed, sloopPower, speedMult);

                //Debug.Log(speedMult);

                interpolation = 0;
                base.Init(boss);
            }
        }
    }
}
