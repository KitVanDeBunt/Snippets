using UnityEngine;
using System.Collections;

namespace BigBossBattle {
    [RequireComponent(typeof(Boss))]
    public class BossStarter : MonoBehaviour {
        [SerializeField]
        private Boss boss;
	    // Use this for initialization
	    void Start () {
            
            boss.StartBoss();
	    }
	
    }
}