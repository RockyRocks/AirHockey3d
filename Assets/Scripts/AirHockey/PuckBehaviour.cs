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
	private Vector3 previousPlanarPosition;
	private float puckRadius = 0.12f;
	private bool sweptRailThisStep;

	void Start () {
		ConfigurePuckBody();
		AirHockeySurfaceMaterials.ApplyToScene();
		previousPlanarPosition = transform.position;
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
            this.GetComponent<Rigidbody>().linearVelocity = new Vector3(Random.Range(2, 5), 0, Random.Range(-3, 4));
        }
	}
	// On Collision Enter event for the velocity update after HumanPlayer or AiPlayer hits the puck
	void OnCollisionEnter(Collision col){
		if (col.collider.tag == "Player") {
			StartCoroutine(PlayAudio(Playerhit));
				_ContactPoint=col.contacts[0].normal;
                players = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
                this.Collided = true;
                ApplyMalletStrike(col);
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
                ApplyMalletStrike(col);
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
         if (IsRailCollision(col))
            ApplyRailBounce(col);
	}

    void OnTriggerEnter(Collider col){
        if(Properties.GameType==Properties.Modes.PlayforMoney){
            // flagging should be done here
        }
        if (col.tag == "TWAflag" || col.tag == "TWBflag"){
            this.Topposthit = true;
			StartCoroutine(PlayAudio(AiPlayerWallhit));
        }
        if (col.tag == "BWAflag" || col.tag == "BWBflag"){
            this.bottomposthit = true;
			StartCoroutine(PlayAudio(PlayerWallhit));
        }
    }

	void FixedUpdate(){
		Rigidbody body = GetComponent<Rigidbody>();
		// The mallet strike is applied in OnCollisionEnter from the mallet's
		// planar velocity. Collided is cleared here so a single contact cannot
		// stack a second impulse on the next physics step.
		Collided = false;
		sweptRailThisStep = false;

		Vector3 velocity = FromNumerics(PuckCollisionMath.ApplyAirDrag(
			ToNumerics(body.linearVelocity),
			PuckCollisionMath.AirDragPerSecond,
			Time.fixedDeltaTime));
		velocity = FromNumerics(PuckCollisionMath.ClampPlanarSpeed(ToNumerics(velocity), MaxSpeed));
		body.linearVelocity = velocity;
		LockToTable(body);
		PreventRailTunnel(body);
	}

	void ConfigurePuckBody()
	{
		Rigidbody body = GetComponent<Rigidbody>();
		body.useGravity = false;
		body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		body.interpolation = RigidbodyInterpolation.Interpolate;
		body.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
		body.linearDamping = 0.02f;
		body.angularDamping = 0.35f;
		body.maxDepenetrationVelocity = 3f;
		Collider puckCollider = GetComponent<Collider>();
		if (puckCollider != null)
		{
			puckCollider.material = AirHockeySurfaceMaterials.Puck;
			puckRadius = Mathf.Max(puckCollider.bounds.extents.x, puckCollider.bounds.extents.z);
		}
	}

	void ApplyMalletStrike(Collision col)
	{
		if (players == null || col.contactCount == 0)
			return;
		Vector3 physicsMallet = col.rigidbody != null ? col.rigidbody.linearVelocity : Vector3.zero;
		Vector3 puckIncoming = col.relativeVelocity + physicsMallet;
		Vector3 malletVelocity = players.PlanarVelocity;
		if (malletVelocity.magnitude < Player_minspeed)
			malletVelocity = _ContactPoint.sqrMagnitude > 1e-6f ? _ContactPoint.normalized * Player_minspeed : malletVelocity;
		System.Numerics.Vector3 resolved = PuckCollisionMath.ResolveMalletHit(
			ToNumerics(puckIncoming),
			ToNumerics(malletVelocity),
			ToNumerics(_ContactPoint),
			PuckCollisionMath.MalletRestitution,
			Player_minspeed);
		resolved = PuckCollisionMath.ClampPlanarSpeed(resolved, MaxSpeed);
		GetComponent<Rigidbody>().linearVelocity = FromNumerics(resolved);
	}

	void ApplyRailBounce(Collision col)
	{
		if (sweptRailThisStep || col.contactCount == 0)
			return;
		Vector3 normal = col.contacts[0].normal;
		System.Numerics.Vector3 resolved = PuckCollisionMath.ResolveRail(
			ToNumerics(col.relativeVelocity),
			ToNumerics(normal),
			PuckCollisionMath.RailRestitution,
			PuckCollisionMath.RailFriction);
		resolved = PuckCollisionMath.ClampPlanarSpeed(resolved, MaxSpeed);
		GetComponent<Rigidbody>().linearVelocity = FromNumerics(resolved);
	}

	bool IsRailCollision(Collision col)
	{
		string tag = col.collider.tag;
		if (tag == "Player" || tag == "AIPlayer" || tag == "GoalPostBottom" || tag == "GoalPostTop" || tag == "Puck")
			return false;
		int wallLayer = LayerMask.NameToLayer("Walls");
		if (wallLayer >= 0 && col.collider.gameObject.layer == wallLayer)
			return true;
		string name = col.collider.gameObject.name;
		return name.IndexOf("Wall", System.StringComparison.OrdinalIgnoreCase) >= 0
			|| name.IndexOf("Rail", System.StringComparison.OrdinalIgnoreCase) >= 0
			|| name.IndexOf("Board", System.StringComparison.OrdinalIgnoreCase) >= 0;
	}

	void LockToTable(Rigidbody body)
	{
		Vector3 position = body.position;
		position.y = 0f;
		body.position = position;
		Vector3 angular = body.angularVelocity;
		angular.x = 0f;
		angular.z = 0f;
		body.angularVelocity = angular;
	}

	void PreventRailTunnel(Rigidbody body)
	{
		Vector3 current = body.position;
		current.y = 0f;
		Vector3 movement = current - previousPlanarPosition;
		float distance = movement.magnitude;
		if (distance > puckRadius * 0.25f)
		{
			int wallLayer = LayerMask.NameToLayer("Walls");
			int mask = wallLayer >= 0 ? (1 << wallLayer) : Physics.DefaultRaycastLayers;
			Vector3 direction = movement / distance;
			if (Physics.SphereCast(previousPlanarPosition, puckRadius * 0.85f, direction, out RaycastHit hit, distance, mask, QueryTriggerInteraction.Ignore))
			{
				body.position = hit.point + hit.normal * puckRadius;
				System.Numerics.Vector3 bounced = PuckCollisionMath.ResolveRail(
					ToNumerics(body.linearVelocity),
					ToNumerics(hit.normal),
					PuckCollisionMath.RailRestitution,
					PuckCollisionMath.RailFriction);
				body.linearVelocity = FromNumerics(PuckCollisionMath.ClampPlanarSpeed(bounced, MaxSpeed));
				sweptRailThisStep = true;
				current = body.position;
			}
		}
		previousPlanarPosition = current;
	}

	static System.Numerics.Vector3 ToNumerics(Vector3 value)
	{
		return new System.Numerics.Vector3(value.x, value.y, value.z);
	}

	static Vector3 FromNumerics(System.Numerics.Vector3 value)
	{
		return new Vector3(value.X, value.Y, value.Z);
	}
	
	void PuckReSpawn(){
		this.transform.position = Vector3.zero;
		Rigidbody body = GetComponent<Rigidbody>();
		body.linearVelocity = Vector3.zero;
		body.angularVelocity = Vector3.zero;
		previousPlanarPosition = Vector3.zero;
	}
    public void Reset(){
        this.PuckReSpawn();
    }

	public float PlayerSpeed{
		get{return playerSpeed;}
		set {playerSpeed=value;}
	}

    public IEnumerable PuckDrop(Vector3 transformPosition){
        this.transform.GetComponent<Rigidbody>().position = Vector3.MoveTowards(new Vector3(transformPosition.x, 15f, transformPosition.z), transformPosition, Time.smoothDeltaTime);
        yield return transformPosition;
    }
	IEnumerator PlayAudio(AudioClip audioClip){
				this.GetComponent<AudioSource>().volume = Properties.sfxVolume;
				this.GetComponent<AudioSource>().PlayOneShot (audioClip);
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
        this.transform.position = PuckPositions[Random.Range(0, PuckPositions.Length)];
        Rigidbody body = GetComponent<Rigidbody>();
        body.linearVelocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
        previousPlanarPosition = transform.position;
    }
}








