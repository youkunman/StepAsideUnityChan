using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UnityChanController : MonoBehaviour {
        //アニメーションするためのコンポーネントを入れる
        private Animator myAnimator;
        //Unityちゃんを移動させるコンポーネントを入れる（追加）
        private Rigidbody myRigidbody;
        //前進するための力（追加）
        private float forwardForce = 800.0f;

        private float turnForce = 500.0f;

        private float upForce = 500.0f;

        private float movableRange = 3.4f;

        private float coefficient = 0.95f;

        private bool isEnd = false;

        private GameObject stateText;

        private GameObject scoreText;

        private int score = 0;

        private bool isLButtonDown = false;

        private bool isRButtonDown = false;
        
        // Use this for initialization
        void Start () {
                
                //アニメータコンポーネントを取得
                this.myAnimator = GetComponent<Animator>();
                
                //走るアニメーションを開始
                this.myAnimator.SetFloat ("Speed", 1);

                //Rigidbodyコンポーネントを取得（追加）
                this.myRigidbody = GetComponent<Rigidbody>();

                this.stateText = GameObject.Find("GameResultText");

                this.scoreText = GameObject.Find("ScoreText");
        }

        // Update is called once per frame
        void Update () {

                if (this.isEnd) {
                        this.forwardForce *= this.coefficient;
                        this.turnForce *= this.coefficient;
                        this.upForce *= this.coefficient;
                        this.myAnimator.speed *= this.coefficient;
                }

                //Unityちゃんに前方向の力を加える（追加）
                this.myRigidbody.AddForce (this.transform.forward * this.forwardForce);

                if ((Input.GetKey (KeyCode.LeftArrow) || this.isLButtonDown) && -this.movableRange < this.transform.position.x) {
                        //左に移動（追加）
                        this.myRigidbody.AddForce (-this.turnForce, 0, 0);
                } else if ((Input.GetKey (KeyCode.RightArrow) || this.isRButtonDown) && this.transform.position.x < this.movableRange) {
                        //右に移動（追加）
                        this.myRigidbody.AddForce (this.turnForce, 0, 0);
                }

                if (this.myAnimator.GetCurrentAnimatorStateInfo(0).IsName ("Jump")) {
                        this.myAnimator.SetBool ("Jump", false);
                }
                
                //ジャンプしていない時にスペースが押されたらジャンプする（追加）
                if (Input.GetKeyDown(KeyCode.Space) && this.transform.position.y < 0.5f) {
                        //ジャンプアニメを再生（追加）
                        this.myAnimator.SetBool ("Jump", true);
                        //Unityちゃんに上方向の力を加える（追加）
                        this.myRigidbody.AddForce (this.transform.up * this.upForce);
                }
        }

        void OnTriggerEnter(Collider other){

                if (other.gameObject.tag == "CarTag" || other.gameObject.tag == "TrafficConeTag") {
                        this.isEnd = true;

                        this.stateText.GetComponent<Text>().text = "GAME OVER";

                        Destroy (other.gameObject);
                }

                if (other.gameObject.tag == "GoalTag") {
                        this.isEnd = true;

                        this.stateText.GetComponent<Text>().text = "CLEAR!!";
                }

                if (other.gameObject.tag == "CoinTag") {

                        // スコアを加算(追加)
                        this.score += 10;

                        //ScoreText獲得した点数を表示(追加)
                        this.scoreText.GetComponent<Text> ().text = "Score " + this.score + "pt";

                        //パーティクルを再生
                        GetComponent<ParticleSystem> ().Play ();

                        //接触したコインのオブジェクトを破棄
                        Destroy (other.gameObject);
                }
        }

        public void GetMyJumpButtonDown() {
                if (this.transform.position.y < 0.5f) {
                        this.myAnimator.SetBool ("Jump", true);
                        this.myRigidbody.AddForce (this.transform.up * this.upForce);
                }
        }

        public void GetMyLeftButtonDown() {
                this.isLButtonDown = true;
        }

        public void GetMyLeftButtonUp() {
                this.isLButtonDown = false;
        }

        public void GetMyRightButtonDown() {
                this.isRButtonDown = true;
        }

        public void GetMyRightButtonUp() {
                this.isRButtonDown = false;
        }
}