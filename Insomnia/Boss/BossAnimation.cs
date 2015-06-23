using UnityEngine;
using System.Collections;

namespace BigBossBattle {
    [RequireComponent(typeof (Boss))]
    public class BossAnimation : EntityAnimationManager {

        public const string ANIMATION_B1_BIG_LAZOR_FIRE = "bigLazerFire";
        public const string ANIMATION_B1_BIG_LAZOR_PREPARE = "bigLazerPrepare";
        public const string ANIMATION_B1_DEATH = "death";
        public const string ANIMATION_B1_SLIDE_LEFT = "slideLeft";
        public const string ANIMATION_B1_SLIDE_RIGHT = "slideRight";
        public const string ANIMATION_B1_SPAWN = "spawn";

        public const string ANIMATION_B2_DEATH = "Death";
        public const string ANIMATION_B2_FORCEFIELD = "forcefield";
        public const string ANIMATION_B2_FORCEFIELD_LOOP = "forcefieldLoop";
        public const string ANIMATION_B2_SPAWN = "spawnV1";
        public const string ANIMATION_B2_SUMMON_CHAINS = "summonChains";

        public const string ANIMATION_HIT = "hit";
        public const string ANIMATION_IDLE = "idle";

        [SerializeField]
        private bool autoFlipX = false;     
        private Transform FlipTarget;

        private Boss boss;
        private bool bossStarted = false;

        private bool hitDelegateSet = false;
        private bool death = false;

        public void Update() {
            if (autoFlipX) {
                if (transform.position.x > Main.instance.playerPos.x) {
                    transform.localScale = new Vector3( 1,1,1);
                } else {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
            }
        }

        public override void SetAnimation(int trackIndex, string anim, bool loop, bool harSet = false) {
            
            if (boss.bossType == BossType.Boss2) {
                if (anim == ANIMATION_B1_DEATH) {
                    anim = ANIMATION_B2_DEATH;
                }
                if (anim == ANIMATION_B1_SPAWN) {
                    anim = ANIMATION_B2_SPAWN;
                }
            }
            if (!death) {
                base.SetAnimation(trackIndex, anim, loop, harSet);
            } 
            if (anim == ANIMATION_B1_DEATH) {
                death = true;
            }
        }

        public override void Start() {
            base.Start();
            boss = gameObject.GetComponent<Boss>();
            hitDelegateSet = true;
            boss.bossState.onHit += BossHit;
        }

        public override void OnDisable() {
            if (hitDelegateSet) {
                hitDelegateSet = false;
                boss.bossState.onHit -= BossHit;
            }
        }

        //should be call by HitEvent delegate in BigBossBattle.EntityState
        public void BossHit() {
            if (!death) {
                SetAnimation(1, ANIMATION_HIT, false);
            }
        }

        public override void AllAnimationsDone() {
            //_skeletonAnimation.state.Data.DefaultMix = 1.0f;
            //Debug.Log(_skeletonAnimation.state.Data.defaultMix);
            if (!death) {
                SetAnimation(0, ANIMATION_IDLE, true);
            }
        }

        public override void AnimationsComplete(Spine.AnimationState state, int trackIndex, int loopCount) {
            if (!bossStarted) {
                bossStarted = true;
                boss.StartBoss();
            }
        }

        public override void AnimationsEnd(Spine.AnimationState state, int trackIndex) {
        }
        public override void OnAnimationsEvent(Spine.AnimationState state, int trackIndex, Spine.Event e) {
        }
    }
}
