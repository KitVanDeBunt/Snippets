using UnityEngine;

namespace BigBossBattle {
    public class BossSlamPoint : MonoBehaviour {

        public Transform startSlam;
        public Transform endSlamApproximate;
        [SerializeField]
        private Color drawColor = Color.yellow;
        [SerializeField]
        private bool alwaysDraw = false;

        private void OnDrawGizmosSelected() {
            if (!alwaysDraw && startSlam != null && endSlamApproximate != null) {
                Gizmos.color = drawColor;
                Gizmos.DrawLine(startSlam.position, endSlamApproximate.position);
                Gizmos.DrawWireSphere(startSlam.position, 0.25f);
                Gizmos.DrawWireSphere(endSlamApproximate.position, 0.25f);
            }
        }

        private void OnDrawGizmos() {
            if (alwaysDraw && startSlam != null && endSlamApproximate != null) {
                Gizmos.color = drawColor;
                Gizmos.DrawLine(startSlam.position, endSlamApproximate.position);
                Gizmos.DrawWireSphere(startSlam.position, 0.25f);
                Gizmos.DrawWireSphere(endSlamApproximate.position, 0.25f);
            }
        }
    }
}