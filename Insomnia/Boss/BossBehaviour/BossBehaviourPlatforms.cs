using UnityEngine;
using System.Collections;
namespace BigBossBattle {
    public class BossBehaviourPlatforms : BossBehaviour {

        [SerializeField]
        private GameObject[] platforms;
        [SerializeField]
        private GameObject chosenPlatform;

        private int chosenInt;

        private float blinkTime = 0.1f;

        public override void BossEvent(BossEventMessage message, int callerID) {
        }

        private void PlatformBlink() {


            for (int i = 0; i < platforms.Length; i++) {
                if (i == chosenInt) {
                    i++;

                } else {
                    platforms[i].GetComponent<Renderer>().enabled = !platforms[i].GetComponent<Renderer>().enabled;

                }
                if (--blinkTime == 0) {
                    if (!platforms[i].GetComponent<Renderer>().enabled) {
                        platforms[i].GetComponent<Collider2D>().enabled = !platforms[i].GetComponent<Collider2D>().enabled;
                    }
                }
                //platforms[chosenInt].renderer.enabled = false;
                Debug.Log("chosen Integer is: " + chosenInt);
                Debug.Log(i);
            }
        }

        public override void Tick() {
        }

        public override void End() {
        }

        public override void Init(Boss boss) {
            base.Init(boss);
            chosenInt = Random.Range(0, platforms.Length);
            InvokeRepeating("PlatformBlink", 1f, .5f);

        }

    }
}