using UnityEngine;
namespace BigBossBattle {
    public class BossState:EntityState {
        [HideInInspector]
        public float bossStartHealth;
        [HideInInspector]
        public float bossHealth;
        //[SerializeField]
        //private UnityEngine.UI.Text healtText;

        private Boss boss;
        private void Start() {
            OnHealtChange();
            boss = gameObject.GetComponent<Boss>();
            bossHealth = (float)health;
            bossStartHealth = bossHealth;
        }

        public override void Death() {
            boss.EndBoss();
        }

        public override void OnRemoveLife() {

        }

        public override void OnHealtChange() {
            SoundController.instance.playPreset(SoundPreset.Boss1Death, transform.position);
            //healtText.text = "boss healt:" + health.ToString();
            bossHealth = (float)health;
        }
    }
}
