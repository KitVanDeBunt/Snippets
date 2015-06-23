using UnityEngine;

namespace BigBossBattle {
    public class BossReference : MonoBehaviour {
        public Boss boss;

        public void CallBossEvent(BossEventMessage eventMessage) {
            //Debug.Log("Boss Message");
            boss.CallBossEvent(eventMessage,gameObject.GetInstanceID());
        }
    }
}
