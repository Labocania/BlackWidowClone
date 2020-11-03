using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, MainActions.IPlayerActions
{
	// The number of lives.
	public int lives;
	int score;

    // Initial position.
    Vector2 initialPosition;
	// Flag for executing animations.
	bool inAnimation;

	LerpComponent lerpAnimations;
	ShootingComponent shootComponent;
	MovementComponent moveComponent;
	PolygonCollider2D polyCollider;
	GameManager manager;

	// Color management.
	ColorSwapper swapper;
	
	// Input Management
	MainActions mainActions;
	Vector2 movementInput;
	Vector2 rotationInput;



    // Unity event functions section.
    void Awake()
	{
		lerpAnimations = GetComponent<LerpComponent>();
		shootComponent = GetComponent<ShootingComponent>();
		moveComponent = GetComponent<MovementComponent>();
		polyCollider = GetComponent<PolygonCollider2D>();
		swapper = GetComponent<ColorSwapper>();
		mainActions = new MainActions();
		mainActions.Player.SetCallbacks(this);
		EventList.enemyDeath += PlayerOnEnemyDeath;
		//manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
	}

	void Start()
	{
		initialPosition = new Vector2(-2.47f, 4.4f);
		swapper.ToggleObjectsColor(ColorNames.Green);			
		inAnimation = true;
		StartCoroutine(StartInitialAnimation(Vector2.zero));
	}

	public void Disable()
	{
		mainActions.Player.Disable();
	}


	public void OnMove(InputAction.CallbackContext context)
	{
		movementInput = context.ReadValue<Vector2>();
	}

	public void OnRotateFire(InputAction.CallbackContext context)
    {
		rotationInput = context.ReadValue<Vector2>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
		GameObject collidedObject = collision.gameObject;
		if (collidedObject.CompareTag("Enemy"))
		{
			StartCoroutine(Death());
		}
	}

	void FixedUpdate()
	{
        if (inAnimation)
        {
			return;
        }

		moveComponent.Move(movementInput);
		RotateAndFire();

		/*
		if (manager.paused) 
		{
			return;
		}
		*/
	}

	void RotateAndFire()
	{
		if (rotationInput.magnitude > 0.1f)
		{
			moveComponent.Rotate(rotationInput);
			shootComponent.Shoot();
		}
	}

	IEnumerator StartInitialAnimation(Vector2 targetPosition)
    {
		// Disables collider;
		polyCollider.enabled = false;
		// Starts rotation animation.
		yield return lerpAnimations.LerpPosition(initialPosition, targetPosition);
		// Changes sprite color.
		swapper.ToggleObjectsColor(ColorNames.Red);
		// Resets rotation to zero.
		transform.rotation = Quaternion.identity;
		// Enables controls and colliders
		mainActions.Player.Enable();
		polyCollider.enabled = true;
		// Resets animation flag.
		inAnimation = false;
	}

	// Fix later
	IEnumerator Death() 
	{
		inAnimation = true;
		// Disabling object.
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
		mainActions.Player.Disable();
		lives -= 1;

		// Death Animation
		StartCoroutine(swapper.IterateColors());
		yield return lerpAnimations.LerpRotation(1080f);
		// Restart Animation
		yield return StartInitialAnimation(transform.position);

		/*
		if (lives <= 0) 
		{
			manager.GameOver();
			yield break;
		}
		*/
	}

	void PlayerOnEnemyDeath(int score)
    {
		this.score += score;
    }

}
