using UnityEngine;
using System.Collections;
namespace BigBossBattle {
    public class BossBehaviourTeleport : BossBehaviour {

        [SerializeField]
        private BossTelportPoint[] teleportPoints;
        [SerializeField]
        private Material bossMaterial;
        //[SerializeField]
        //private SkeletonAnimation bossAnimation;

        [SerializeField]
        private ParticleSystem smokeBom;
        [SerializeField]
        private float interplolationSpeed = 0.1f;
        [SerializeField]
        private float startDelaySpeed = 2.0f;

        private Vector3 chosenPoint;

        private float interpolation;
        private int state;


        public override void BossEvent(BossEventMessage message, int callerID) {
        }

        protected override void InitSettings(bool onHitBehaviour) {
            base.InitSettings(true);
        }

        public override void Init(Boss boss) {
            base.Init(boss);

            chosenPoint = GetNewPoint();
            interpolation = 0;
            state = 0;

            smokeBom.Play();
        }

        public override void End() {
            behaviourDone_ = true;
            //smokeBom.Stop();
        }

        public override void Tick() {
            switch (state) {
                //start delay
                case 0:
                    if (interpolation < 1f) {
                        interpolation += (Time.deltaTime * startDelaySpeed);
                    } else {
                        state += 1;
                        interpolation = 0;
                    }
                    break;
                // fade out
                case 1:
                    if (interpolation < 1f) {
                        interpolation += (Time.deltaTime * interplolationSpeed);
                        SetAlpha((1f-interpolation));
                    } else {
                        state += 1;
                        interpolation = 0;
                        _boss.transform.position = chosenPoint;
                    }
                    break;
                //fade in
                case 2:
                    if (interpolation < 1f) {
                        interpolation += (Time.deltaTime * interplolationSpeed);
                        SetAlpha(interpolation);
                    } else {
                        state += 1;
                        interpolation = 0;
                        End();
                    }
                    break;
            }
        }

        Vector3 GetNewPoint() {
            if (teleportPoints.Length < 2) {
                Debug.LogError("need more points");
            }
            Vector3 newPoint = _boss.transform.position;
            while (newPoint == _boss.transform.position) {
                newPoint = teleportPoints[Random.Range(0, teleportPoints.Length)].transform.position;
            }
            return newPoint;
        }

        void SetAlpha(float alpha) {
            Color c = bossMaterial.color;
            c.a = alpha;
            bossMaterial.color = c;
        }
    }
}