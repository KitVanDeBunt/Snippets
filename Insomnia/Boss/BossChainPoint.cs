using UnityEngine;
using System.Collections;
namespace BigBossBattle {
    public class BossChainPoint : MonoBehaviour {

        public Transform chainBeginPoint;
        public Transform chainEndPoint;

        public Quaternion rotation;

        void Start() {

            rotation = transform.localRotation;
        }

        private void OnDrawGizmosSelected() {
            if (chainBeginPoint != null && chainEndPoint != null) {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(chainBeginPoint.position, chainEndPoint.position);
                Gizmos.DrawWireSphere(chainBeginPoint.position, 0.35f);
                Gizmos.DrawWireSphere(chainEndPoint.position, 0.35f);

            }
        }
    }
}