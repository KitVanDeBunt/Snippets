using UnityEngine;
using System.Collections;

namespace BigBossBattle {
    public abstract class EntityAnimationManager : MonoBehaviour {

        [SerializeField]
        internal SkeletonAnimation _skeletonAnimation;

        internal string currentAnimation;

        private bool delegateSet = false;

        public abstract void AllAnimationsDone();
        public abstract void AnimationsComplete(Spine.AnimationState state, int trackIndex, int loopCount);
        public abstract void AnimationsEnd(Spine.AnimationState state, int trackIndex);
        public abstract void OnAnimationsEvent(Spine.AnimationState state, int trackIndex, Spine.Event e);

        public virtual void Start() {
            delegateSet = true;
            _skeletonAnimation.state.Complete += OnAnimationComplete;
            _skeletonAnimation.state.End += OnAnimationEnd;
            _skeletonAnimation.state.Event += OnAnimationsEvent;
        }

        public virtual void OnDisable() {
            if (delegateSet) {
                delegateSet = false;
                _skeletonAnimation.state.Complete -= OnAnimationComplete;
                _skeletonAnimation.state.End -= OnAnimationEnd;
                _skeletonAnimation.state.Event -= OnAnimationsEvent;
            }
        }

        public virtual void SetAnimation(int trackIndex, string anim, bool loop, bool harSet = false) {
            //Debug.Log("boss animation start: " + anim);
            if (!harSet && currentAnimation == anim) {
                return;
            }
            _skeletonAnimation.state.SetAnimation(trackIndex, anim, loop);
            currentAnimation = anim;
        }

        ///
        /// private metodes {
        ///

        private IEnumerator TestDone(int trackIndex) {

            yield return null;

            if (_skeletonAnimation.state.GetCurrent(trackIndex) != null) {
                //Debug.Log("\n Test animation conpleted: " + _skeletonAnimation.state.GetCurrent(0).animation.name);
            } else {
                //Debug.Log("\n Test no animations");
                currentAnimation = "";
                AllAnimationsDone();
            }
        }

        //animation comlpete delegate 
        private void OnAnimationComplete(Spine.AnimationState state, int trackIndex, int loopCount) {
            AnimationsComplete(state, trackIndex, loopCount);
            //Debug.Log("\n OnAnimationComplete");

            StartCoroutine(TestDone(trackIndex));
        }

        private void OnAnimationEnd(Spine.AnimationState state, int trackIndex) {
            
        }

        ///
        /// } private metodes
        ///

        /*void Update() {
            if (_skeletonAnimation.state.GetCurrent(0) != null) {
                //Debug.Log("\n ------- " + _skeletonAnimation.state.GetCurrent(0).animation.name);
            } else {
                //Debug.Log("\n ------- nope ");
            }
        }*/
    }
}
