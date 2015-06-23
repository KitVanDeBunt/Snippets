using UnityEngine;
namespace BigBossBattle {
    
    public abstract class EntityInput : MonoBehaviour {


        [HideInInspector]
        public float xInput;
        [HideInInspector]
        public float lastXInput;
        [HideInInspector]
        public bool shoot;
        [HideInInspector]
        public bool jump;
        [HideInInspector]
        public bool slash;
        [HideInInspector]
        public bool pause;

        [HideInInspector]
        public bool inSlashAnimation = false;

        [HideInInspector]
        public bool inShootAnimation = false;

    }
}
