using UnityEngine;

namespace BigBossBattle {
    [RequireComponent(typeof(BoxCollider2D))]
    public class BossZone :MonoBehaviour
    {
        private BoxCollider2D bossZone;
        [SerializeField]
        private Boss boss;

        void OnDrawGizmos() {
            if (bossZone == null) {
                bossZone = gameObject.GetComponent<BoxCollider2D>();
            }
            Gizmos.color = Color.red;
            Vector3 zoneSize = new Vector3(bossZone.size.x, bossZone.size.y, 0);
            //Vector3 zoneCenter = new Vector3(bossZone.center.x, bossZone.center.y, 0);
            Gizmos.DrawWireCube(bossZone.transform.position, zoneSize);
        }

        void OnTriggerEnter2D(Collider2D other) {
            if (other.tag == "Player") {
                //boss.StartBoss();
            }
        }
    }
}
