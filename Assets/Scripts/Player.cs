using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, MainActions.IPlayerActions
{
	// The number of lives.
	public int Lives { get; private set; }
	public int Score { get; private set; }
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
		EventList.enemyDeath += onEnemyDeath;
		//manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
	}

	void Start()
	{
		Lives = 3;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
		GameObject collidedObject = collision.gameObject;
		if (collidedObject.CompareTag("Enemy") || collidedObject.CompareTag("KillableEnemy"))
		{
			StartCoroutine(Death());
			EventList.playerDeath.Invoke();
		}
	}

    void FixedUpdate()
	{
        if (inAnimation)
        {
			return;
        }

        moveComponent.TransformMove(movementInput);
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
            moveComponent.TransformSnapRotate(rotationInput);
            shootComponent.Shoot();
		}
	}

	IEnumerator StartInitialAnimation(Vector2 targetPosition)
    {
        if (polyCollider.enabled)
        {
			// Disables collider;
			polyCollider.enabled = false;
		}
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
		polyCollider.enabled = false;
		inAnimation = true;
		// Disabling object.
		mainActions.Player.Disable();
		Lives -= 1;

		// Death Animation
		StartCoroutine(swapper.IterateColors());
		yield return lerpAnimations.LerpRotation(1080f);
		// Restart Animation
		yield return StartInitialAnimation(transform.position);
		EventList.waveStarted.Invoke();

		/*
		if (lives <= 0) 
		{
			manager.GameOver();
			yield break;
		}
		*/
	}

	void onEnemyDeath(int score)
    {
		Score += score;
    }

}
