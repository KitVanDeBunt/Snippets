using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BigBossBattle {
    [RequireComponent(typeof( MeshFilter ),typeof( MeshRenderer ))]
    class HitPlayer {
        public PlayerState player;
        public float time;
    }

    public class BossLazor : MonoBehaviour {
        [SerializeField]
        private Transform lazorParticles;
        [SerializeField]
        private LayerMask lazorLayer;


        private PlayerState hitPlayer;
        private System.Collections.Generic.List<HitPlayer> playerList = new System.Collections.Generic.List<HitPlayer>();

        private MeshRenderer meshRender;
        private MeshFilter meshFilter;

        [SerializeField]
        private Material lazerMaterial;
        private Vector3[] points;

        int i;

        [SerializeField]
        float lazorAngle = 0;

        [SerializeField]
        float width = 0.1f;

        [SerializeField]
        float widthTop = 0.1f;

        private float ratio = 0.3085f;

		[SerializeField]
		private float uvL = 1f;
		[SerializeField]
        private float uvLAnimation = 0f;
        [SerializeField]
        private float uvLAnimationSpeed = 1f;
        [SerializeField]
        private Transform target;
        [SerializeField]
        private Transform beginEffect;

        public float setLazorAngle {
            set{
                lazorAngle = value-90;
            }
        }

        public float setLazorAngleFirstTime {
            set {
                lazorAngle = value - 90;
                Start();
                Update();
            }
        }

        void Start() {
            meshFilter = GetComponent<MeshFilter>();
            meshRender = GetComponent<MeshRenderer>();
            newMesh = new Mesh();
        }

        void Update() {
            uvLAnimation += (uvLAnimationSpeed * Time.deltaTime);

            //Vector3 target
            //float angle = Movement.angleToPoint(transform,points,);
            //Debug

            RaycastHit2D castHit = Physics2D.Raycast(transform.position, Movement.AngleToDirection(lazorAngle), 10F, lazorLayer);
            
            if (castHit.collider != null) {
                //Debug.DrawLine(transform.position, castHit.point, Color.red);
                //set particle
                Vector2 directionHit = castHit.normal;
                directionHit.x = -directionHit.x;
                lazorParticles.transform.eulerAngles = new Vector3(0, 0, (Movement.DirectionToAngle(directionHit)));
                lazorParticles.transform.position = castHit.point;


                //Debug.Log(castHit.collider.gameObject.name);
                if (castHit.collider.gameObject.tag == "Player") {
                    int playerListPosition = -1;
                    for (int i = 0; i < playerList.Count; i++) {
                        if (playerList[i].player == hitPlayer) {
                            playerListPosition = i;
                        }
                    }

                    hitPlayer = castHit.collider.gameObject.GetComponent<PlayerState>();

                    //player add to list
                    if (playerListPosition == -1) {
                        HitPlayer newPlayer = new HitPlayer();
                        newPlayer.player = hitPlayer;
                        newPlayer.time = Time.time;
                        playerList.Add(newPlayer);
                        hitPlayer.Hit();

                        Analytics.TriggerEvent(Analytics.Game_Level001_PlayerDamage_BiggLazor);
                    } else {
                        if (playerList[playerListPosition].time + 0.5f < Time.time) {
                            playerList[playerListPosition].time = Time.time;
                            hitPlayer.Hit();

                            Analytics.TriggerEvent(Analytics.Game_Level001_PlayerDamage_BiggLazor);
                        }
                    }
                }
                points = new Vector3[] { Vector3.zero, (new Vector3(castHit.point.x, castHit.point.y, 0) - transform.position) };

                Debug.DrawLine(transform.position, castHit.point, Color.green);
            } else {
                Debug.DrawRay(transform.position, transform.right * 10F, Color.green);

            }
            // transform.rotation

            meshRender.sharedMaterial = lazerMaterial;
            DrawSnake();

            transform.rotation = Quaternion.identity;
            transform.position = target.position;

            beginEffect.rotation = Movement.AngleToQuaturnion(lazorAngle);
        }


        private void DrawSnake() {

            Mesh snakeMesh = new Mesh();
            List<Vector3> vertices = new List<Vector3>();

            CreateSnakeMesh();
        }

        private void CreateSnakeMesh() {
            List<Vector3> vertList = new List<Vector3>();
            List<Vector2> uvList = new List<Vector2>();
            List<int> triList = new List<int>();
            float lenghtSnake = 1;

            float dist =Vector3.Distance(points[0], points[1]);
   //         Debug.Log("dist:" + dist);
            float height = ((Vector3.Distance(points[0], points[1]) / (( width / ratio))));
            float heightCeil = Mathf.Ceil(height);
            float heightRest = heightCeil - height;

            //float endL = heightCeil + 
  //          Debug.Log("height: " + height);
  //          Debug.Log("height ciel: " + heightCeil);
   //         Debug.Log("height heightRest: " + heightRest);
            if (Vector3.Distance(points[0], points[1]) > (width / ratio)) {

            }

            //Vertices
            for (i = 0; i < lenghtSnake; i++) {

                Vector3 pointPos = points[i];
                Vector3 pointPosNex = points[i + 1];

                Vector3 newVert;

                Vector2 dirTop = (Movement.AngleToDirection((lazorAngle - 90)) * widthTop * ratio);
                Vector2 dir = (Movement.AngleToDirection((lazorAngle - 90)) * width * ratio);


                newVert = pointPos + new Vector3(dirTop.x, dirTop.y, 0);
                vertList.Add(newVert);
                newVert = pointPos + new Vector3(-dirTop.x, -dirTop.y, 0);
                vertList.Add(newVert);
                newVert = pointPosNex + new Vector3(dir.x, dir.y, 0);
                vertList.Add(newVert);
                newVert = pointPosNex + new Vector3(-dir.x, -dir.y, 0);
                vertList.Add(newVert);
            }

            //uv
            for (i = 0; i < lenghtSnake; i++) {
                //0.0f,0.0f
                //1.0f,0.0f
                //0.0f,1.0f
                //1.0f,1.0f
				uvList.Add(new Vector2(0, ((0*uvL)*(height/ratio))+uvLAnimation) );
				uvList.Add(new Vector2(1, ((0*uvL)*(height/ratio))+uvLAnimation) );
					        uvList.Add(new Vector2(0, ((1*uvL)*(height/ratio))+uvLAnimation) );
					        uvList.Add(new Vector2(1, ((1*uvL)*(height/ratio))+uvLAnimation) );
            }

            //triangles
            for (i = 0; i < lenghtSnake; i++) {
                //1,0,3,
				//0,2,3
				triList.Add(3 );
				triList.Add(2 );
				triList.Add(1 );
					
					
				triList.Add(1 );
				triList.Add(2 );
				triList.Add(0 );
            }
            DataToMesh("roadMesh", vertList.ToArray(), uvList.ToArray(), triList.ToArray());
        }

        Mesh newMesh;
        private void DataToMesh(string name, Vector3[] vertices, Vector2[] uvs, int[] triangles) {


            newMesh = meshFilter.mesh;
            newMesh.name = name;
            newMesh.vertices = vertices;
            newMesh.uv = uvs;
            newMesh.triangles = triangles;
            //newMesh.Optimize();
            //newMesh.RecalculateNormals();
            newMesh.RecalculateBounds();
            meshFilter.mesh = newMesh;
        }
    }
}
