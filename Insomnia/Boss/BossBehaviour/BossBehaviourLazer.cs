using UnityEngine;

namespace BigBossBattle {
    [System.Serializable]
    public class BossBehaviourLazer : BossBehaviour 
    {
        enum LazerState {
            lazor,
            preLazor
        }

        [SerializeField]
        private float speed = 10;
        [SerializeField]
        private float speedParticleState = 10;
        [SerializeField]
        private float startAngle = -50;
        [SerializeField]
        private float deltaAngle = 50;

        [SerializeField]
        private BossLazor lazor;
        [SerializeField]
        private Transform particle;

        private LazerState currentState = LazerState.preLazor;
        private float interpolation;

        public override void BossEvent(BossEventMessage message, int callerID) {
        }

        public override void Tick() {
            switch (currentState) {
                case LazerState.preLazor:
                    interpolation += speedParticleState * Time.deltaTime;
                    if (interpolation > 1) {

						//sound
                        SoundController.instance.playPreset(SoundPreset.BossBigLazor, transform.position);
						//SoundController.instance.Play( lazorAudioClip, 1f, 1f, transform.position,false);
						//Main.instance.PlaySoundOneShot(lazorAudioClip);

                        SetState(LazerState.lazor);
                        particle.gameObject.SetActive(false);
                        lazor.gameObject.SetActive(true);
                        interpolation = 0;

                        UpdateLazorAngle(true);
                    }
                    break;
                case LazerState.lazor:
                    UpdateLazorAngle();
                    if (interpolation > 1) {
                        End();
                    }
                    break;
            }
        }

        void UpdateLazorAngle(bool first = false) {
            //shake Screen
            Main.instance.cam.effectManager.AddShake(new EffectScreenShake(0.016f, 3, 0.025f));

            interpolation += speed * Time.deltaTime;
            float endAngle = startAngle + deltaAngle;
            float angle = Mathf.Lerp(startAngle, endAngle, interpolation);
            //Debug.Log("\n angle:" + angle + " interpolation: " + interpolation);
            if (first) {
                lazor.setLazorAngleFirstTime = angle;
            }else{
                lazor.setLazorAngle = angle;
            }
        }
        
        private void  SetState(LazerState newState) {
            switch (newState) {
                case LazerState.preLazor:
                    _boss.bossAnimation.SetAnimation(1,BossAnimation.ANIMATION_B1_BIG_LAZOR_PREPARE, false);
                    break;
                case LazerState.lazor:
                    _boss.bossAnimation.SetAnimation(1,BossAnimation.ANIMATION_B1_BIG_LAZOR_FIRE, false);
                    break;
            }
            currentState = newState;
        }

        public override void End() {
            //Debug.Log("end");
            particle.gameObject.SetActive(false);
            lazor.gameObject.SetActive(false);
            behaviourDone_ = true;
        }

        public override void Init(Boss boss) {
            base.Init(boss);

            SoundController.instance.playPreset(SoundPreset.BossPreLazor, transform.position);
			//SoundController.instance.Play( preLazorAudioClip, 0.5f, 1f, transform.position);
			//Main.instance.PlaySoundOneShot(preLazorAudioClip);

            SetState(LazerState.preLazor);
            lazor.gameObject.SetActive(false);
            particle.gameObject.SetActive(true);
            interpolation = 0;
        }
    }
}
