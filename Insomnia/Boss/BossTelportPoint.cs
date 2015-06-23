using UnityEngine;
using System.Collections;

public class BossTelportPoint : MonoBehaviour {

    public Transform teleportPoint;

    private void OnDrawGizmosSelected() {
        if (teleportPoint != null) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(teleportPoint.position, 0.25f);

        }
    }
}
