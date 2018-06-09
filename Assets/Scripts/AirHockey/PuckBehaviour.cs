using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Managing the physics and gameplay of the puck!
public class PuckBehaviour : MonoBehaviour {

	public AudioClip PuckMoving;
	public AudioClip Playerhit;
	public AudioClip Aihit;
	public AudioClip GoalFast;
	public AudioClip GoalSlow;
	public AudioClip PlayerGoal;
    public AudioClip[] GoalCollection_audio;
	public AudioClip AiPlayerGoal;
	public AudioClip PlayerWallhit;
	public AudioClip AiPlayerWallhit;
    public AudioClip[] BlockShot_audio;


	private int ScoreTopValue=0;
	private int ScoreBottomValue=0;
	private Vector3 _ContactPoint;
	private bool Collided=false;
	private float playerSpeed;
	private PlayerController players;
    private int NoofWalls = 0;
    private bool HumanPlayerCourt;
    private bool AIPlayerCourt;
    private float Player_minspeed = 2f;

    public float MaxSpeed = 8f;
    public Vector3 AiPlayerHitpoint;
    public bool GoalScored = false;
    public int PlayerHits = 0;
    public int AIPlayerHits = 0;
    public Vector3[] PuckPositions;
    public bool Topposthit = false;
    public bool bottomposthit = false;
	// Use this for initialization
	void Start () {
		//players = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
		if(Properties.GameType==Properties.Modes.PlayforMoney){
        PuckPositions = new Vector3[] { 
            new Vector3(-1f,0f,1.5f),
            new Vector3(1f,0f,1.5f),
            new Vector3(-1.5f,0f,1.5f),
            new Vector3(1.5f,0f,1.5f),
            new Vector3(0.5f,0f,1.5f),
            new Vector3(-1.39f,0f,-2.38f),
            new Vector3(1.41f,0f,-2.38f)
        };
        //StartCoroutine("PuckDrop",(new Vector3(-1f,0f,1.5f)));
            this.challengePositions(Properties.SelectedChallenge);
        }else{
            this.rigidbody.velocity = new Vector3(Random.Range(2, 5), 0, Random.Range(-3, 4));
        }
	}
	// On Collision Enter event for the velocity update after HumanPlayer or AiPlayer hits the puck
	void OnCollisionEnter(Collision col){
		if (col.collider.tag == "Player") {
			StartCoroutine(PlayAudio(Playerhit));
				_ContactPoint=col.contacts[0].normal;
                players = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
                this.Collided = true;
                this.PlayerHits += 1;
                if (this.AIPlayerCourt){
                    this.HumanPlayerCourt = false;
                    Properties.NoofBlocks += 1;
                    StartCoroutine(PlayAudio(Aihit));
                    this.AIPlayerCourt = false;
                }
			}
		if(col.collider.tag=="AIPlayer"){
				StartCoroutine(PlayAudio(Aihit));
                _ContactPoint = col.contacts[0].normal;
				AiPlayerHitpoint=_ContactPoint;
                players = GameObject.FindGameObjectWithTag("AIPlayer").GetComponent<PlayerController>();
                this.Collided = true;
            if(!this.AIPlayerCourt)
                this.AIPlayerHits += 1;
                this.AIPlayerCourt = true;
			}
			
		//Updating the  scores of  individual players here.
		if (col.collider.tag == "GoalPostBottom") {
			StartCoroutine(PlayAudio(GoalFast));
			ScoreBottomValue+=1;
			//GameObject.FindGameObjectWithTag("AIPlayerScore").guiText.text=" "+ ScoreBottomValue;
             Properties.AIPlayerGoals = ScoreBottomValue;
			this.GoalScored=true;
			this.PuckReSpawn();
				}
		 if( col.collider.tag == "GoalPostTop") {
			StartCoroutine(PlayAudio(GoalSlow));
			ScoreTopValue+=1;
			//GameObject.FindGameObjectWithTag("PlayerScore").guiText.text=" "+ScoreTopValue;
			Properties.HumanplayerGoals = ScoreTopValue;
			this.GoalScored=true;
            StartCoroutine(PlayAudio(GoalCollection_audio[Random.Range(0,3)]));
			this.PuckReSpawn();
				}
         if (col.collider.gameObject.layer == LayerMask.NameToLayer("Walls")
             && Properties.GameType == Properties.Modes.PlayforMoney){
            Properties.NoofWalls += 1;
         }
	}

    void OnTriggerEnter(Collider col){
        if(Properties.GameType==Properties.Modes.PlayforMoney){
            // flagging should be done here
        }
        if (col.collider.tag == "TWAflag" || col.collider.tag == "TWBflag"){
            this.Topposthit = true;
			StartCoroutine(PlayAudio(AiPlayerWallhit));
        }
        if (col.collider.tag == "BWAflag" || col.collider.tag == "BWBflag"){
            this.bottomposthit = true;
			StartCoroutine(PlayAudio(PlayerWallhit));
        }
    }

	void FixedUpdate(){
		if (Collided && players!=null) {
			if(players.Speed<Player_minspeed)
                players.Speed = Player_minspeed;
			this.rigidbody.AddForce (_ContactPoint * players.Speed * 2f,ForceMode.Force);
			Collided=false;
		}
		// Take out the Hard Coded stuff.
		if (this.rigidbody.velocity.z > MaxSpeed)
						this.rigidbody.velocity = new Vector3(this.rigidbody.velocity.x,
			                                      this.rigidbody.velocity.y,
			                                      MaxSpeed);
		if(this.rigidbody.velocity.z < -MaxSpeed)
						this.rigidbody.velocity = new Vector3(this.rigidbody.velocity.x,
			                                      this.rigidbody.velocity.y,
			                                      -MaxSpeed);
	}
	
	void PuckReSpawn(){
		this.transform.position = Vector3.zero;
		this.rigidbody.velocity = Vector2.zero;
	}
    public void Reset(){
        this.PuckReSpawn();
    }

	public float PlayerSpeed{
		get{return playerSpeed;}
		set {playerSpeed=value;}
	}

    public IEnumerable PuckDrop(Vector3 transformPosition){
        this.transform.rigidbody.position = Vector3.MoveTowards(new Vector3(transformPosition.x, 15f, transformPosition.z), transformPosition, Time.smoothDeltaTime);
        yield return transformPosition;
    }
	IEnumerator PlayAudio(AudioClip audioClip){
				audio.volume = Properties.sfxVolume;
				audio.PlayOneShot (audioClip);
				yield return new WaitForSeconds (audioClip.length);
		}
    public void challengePositions(string challengeName){
        switch (challengeName){
            case "Bank Shot":
            case "Off the Boards":
            case "Ricochet Champ":
            case "Penalty Shot":
                this.RandomPuckReSpawn();
                break;
        }
    }
    public void RandomPuckReSpawn(){
        this.transform.position = PuckPositions[Random.Range(5, 6)];
        this.rigidbody.velocity = Vector2.zero;
    }
}
