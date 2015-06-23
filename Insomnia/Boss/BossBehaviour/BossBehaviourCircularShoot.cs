
using UnityEngine;

namespace BigBossBattle {
    [System.Serializable]
    public class BossBehaviourCircularShoot : BossBehaviour {

        public enum ShootType {
            Spiral,
            Kaboom,
            SpiralKaboom
        }

        public Bullet bulletPrefab;
        public Transform spawn;

        [SerializeField]
        public ShootType shootType;
        [SerializeField]
        private float speed = 0.5f;
        [SerializeField]
        private float spiralSpeed = 0.03f;
        [SerializeField]
        private float bulletSpeed = 1;
        [SerializeField]
        private float spawnTime = 0.5f;
        [SerializeField]
        private int kaboomBulletCount = 20;
        [SerializeField]
        private Transform bulletHolder;

        private float interpolation;
        private float spawnTimer = 0;
        private float angleIterpolate = 0;

        private int behaviourState = 0;

        //spawn sprite
        private Color spawnSpriteColor;
        private tk2dSprite spawnSprite;
        [SerializeField]
        private float spawnRotationSpeed = 1f;
        [SerializeField]
        private float spawnScale = 400f;
        private float currentAngle = 0;
        [SerializeField]
        private bool flipTexture;

        [SerializeField]
        private Transform bossHandTop;
        [SerializeField]
        private Transform bossHandBottom;
        [SerializeField]
        private BossHandAnimationManager bossHandTopAnimation;
        [SerializeField]
        private BossHandAnimationManager bossHandBottomAnimation;

        public override void BossEvent(BossEventMessage message, int callerID) {

        }

        public override void Tick() {
            if (!behaviourDone_) {
                //Debug.Log("behaviourState: "+behaviourState);
                spawn.transform.rotation = MathAngles.AngleToQuaturnion((currentAngle += (spawnRotationSpeed * 50f * Time.deltaTime)));
                if (behaviourState == 1) {
                    interpolation += speed * Time.deltaTime;

                    spawnTimer += Time.deltaTime;
                    if (shootType == ShootType.Kaboom) {
                        if (spawnTimer > spawnTime) {
                            spawnTimer = 0f;
                            SpawnBulletCircle(kaboomBulletCount);
                        }
                    } else if (shootType == ShootType.Spiral) {
                        if (spawnTimer > spawnTime) {
                            spawnTimer = 0f;
                            angleIterpolate += spiralSpeed;
                            if (angleIterpolate > 1f) {
                                angleIterpolate = 0f;
                            }
                            if (angleIterpolate < 0f) {
                                angleIterpolate = 1f;
                            }
                            SpawnBullet(angleIterpolate * 360f);
                        }
                    } else if (shootType == ShootType.SpiralKaboom ) {
                        if (spawnTimer > spawnTime) {
                            spawnTimer = 0f;

                            angleIterpolate += spiralSpeed;
                            if (angleIterpolate > 1f) {
                                angleIterpolate = 0f;
                            }
                            if (angleIterpolate < 0f) {
                                angleIterpolate = 1f;
                            }

                            SpawnBulletCircle(kaboomBulletCount, (angleIterpolate * 360f));
                        }
                    }
                    if (interpolation > 1) {
                        behaviourState++;
                        //spawnSpriteColor.a+=Time.deltaTime;
                    }
                } else if (behaviourState == 0) {
                    if (spawnSpriteColor.a > 1f) {
                        behaviourState++;
                    } else {
                        spawnSpriteColor.a += Time.deltaTime;
                        UpdateSpawn();
                    }
                } else if (behaviourState == 2) {
                    if (bossHandTopAnimation.reversed && bossHandBottomAnimation.reversed) {
                        behaviourState++;
                        return;
                    }
                    if (!bossHandTopAnimation.reversing) {
                        bossHandTopAnimation.ReverseHandBulletSpawn(BossHandAnimationManager.ANIMATION_BOSS_HAND_BARAGE_TOP_HAND_REVERSE);
                    }
                    if (!bossHandBottomAnimation.reversing) {
                        bossHandBottomAnimation.ReverseHandBulletSpawn(BossHandAnimationManager.ANIMATION_BOSS_HAND_BARAGE_BOTTOM_HAND_REVERSE);
                    }
                } else if (behaviourState == 3) {
                    spawnSpriteColor.a -= Time.deltaTime;
                    UpdateSpawn();
                    if (spawnSpriteColor.a < 0.1f) {
                        End();
                        //Debug.Log("end");
                    }
                }
            }
        }

        private void SpawnBulletCircle(int count,float deltaAngle = 0) {
            //sound
            SoundController.instance.playPreset(SoundPreset.BossLazor, transform.position);

            for (int i = 0; i < count; i++) {
                //Debug.Log((((i + 1f) / count) * 360f )+ "\n" + i);
                SpawnBullet(( (((i+1f)/(float)count)*360f)+deltaAngle));
            }
        }

        private void SpawnBullet(float angel) {
            
            GameObject newBullet = (GameObject)GameObject.Instantiate(bulletPrefab.gameObject, spawn.position, Quaternion.Euler(new Vector3(0,0,angel)));
            newBullet.GetComponent<Bullet>().Init(bulletSpeed, angel, bulletHolder);
            newBullet.transform.parent = bulletHolder.transform;
        }

        public override void End() {
            bossHandTopAnimation.StopBulletSpawn();
            bossHandBottomAnimation.StopBulletSpawn();
            spawnSpriteColor.a = 0f;
            spawnSprite.color = spawnSpriteColor;
            behaviourDone_ = true;
        }

        public override void Init(Boss boss) {
            spawnSprite = spawn.GetComponent<tk2dSprite>();
            spawnSpriteColor = spawnSprite.color;
            spawnSpriteColor.a = 0f;
            base.Init(boss);
            interpolation = 0f;
            behaviourState = 0;

            bossHandTopAnimation.StartBulletSpawn(BossHandAnimationManager.ANIMATION_BOSS_HAND_BARAGE_TOP_HAND);
            bossHandBottomAnimation.StartBulletSpawn(BossHandAnimationManager.ANIMATION_BOSS_HAND_BARAGE_BOTTOM_HAND);
        }

        public void UpdateSpawn() {
            if (flipTexture) {
                Vector3 scale = new Vector3(-1, 1, 1);
                spawn.transform.localScale = (scale * spawnScale * spawnSpriteColor.a);
            } else {
                spawn.transform.localScale = (Vector3.one * spawnScale * spawnSpriteColor.a);
            }
            spawnSprite.color = spawnSpriteColor;
        }
    }
}

