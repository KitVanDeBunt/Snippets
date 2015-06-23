using UnityEngine;

namespace BigBossBattle {
    public class BossHandAnimationManager:EntityAnimationManager {

        public const string ANIMATION_BOSS_HAND_DEATH = "death";
        public const string ANIMATION_BOSS_HAND_IDLE = "idle";//in game
        public const string ANIMATION_BOSS_HAND_BARAGE_BOTTOM_HAND = "lazerbarageBottomHand";//in game
        public const string ANIMATION_BOSS_HAND_BARAGE_BOTTOM_HAND_REVERSE = "lazerbarageBottomHandReverse";//in game
        public const string ANIMATION_BOSS_HAND_BARAGE_TOP_HAND = "lazerbarageTopHand";//in game
        public const string ANIMATION_BOSS_HAND_BARAGE_TOP_HAND_REVERSE = "lazerbarageTopHandReverse";//in game
        public const string ANIMATION_BOSS_HAND_SLAM = "slam";//in game
        public const string ANIMATION_BOSS_HAND_SLAM_PREPARE = "slamPrepare";//in game
        public const string ANIMATION_BOSS_SPAWN = "spawn";//in game

        [SerializeField]
        private Boss boss;

        [HideInInspector]
        private bool bulletsSpawning = false;
        private bool reversing_ = false;
        private bool reversed_ = false;

        public bool reversing {
            get {
                return reversing_;
            }
        }

        public bool reversed {
            get {
                return reversed_;
            }
        }

        public override void AnimationsComplete(Spine.AnimationState state, int trackIndex, int loopCount) {
            //Debug.Log("hand animaatio done: " + _skeletonAnimation.state.GetCurrent(0).animation.name);
            if (_skeletonAnimation.state.GetCurrent(trackIndex).animation.name == ANIMATION_BOSS_HAND_SLAM) {
                boss.CallBossEvent(BossEventMessage.SlamAttackDone, gameObject.GetInstanceID());
            } else if (_skeletonAnimation.state.GetCurrent(trackIndex).animation.name == ANIMATION_BOSS_HAND_SLAM_PREPARE) {
                boss.CallBossEvent(BossEventMessage.PreSlamAttackDone, gameObject.GetInstanceID());
            }
        }


        public override void OnAnimationsEvent(Spine.AnimationState state, int trackIndex, Spine.Event e) {
        }
        public override void AnimationsEnd(Spine.AnimationState state, int trackIndex) {
        }

        public void StartBulletSpawn(string anim) {
            bulletsSpawning = true;
            SetAnimation(0, anim, false);
            reversed_ = false;
        }
        public void ReverseHandBulletSpawn(string anim) {
            reversing_ = true;
            SetAnimation(0, anim, false);
        }

        public void StopBulletSpawn() {
            bulletsSpawning = false;
            
            SetAnimation(0, ANIMATION_BOSS_HAND_IDLE, true);
        }

        public override void AllAnimationsDone() {
            if (!bulletsSpawning) {
                SetAnimation(0, ANIMATION_BOSS_HAND_IDLE, true);
            } else if (reversing_) {
                reversing_ = false;
                reversed_ = true;
            }
        }
    }
}
