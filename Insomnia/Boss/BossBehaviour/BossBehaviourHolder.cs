namespace BigBossBattle {
    [System.Serializable]
    public class BossBehaviourHolder :UnityEngine.MonoBehaviour
    {
        //[UnityEngine.HideInInspector]
        public int behaviourWeight;
        public BossBehaviour[] behaviour;
    }
}
