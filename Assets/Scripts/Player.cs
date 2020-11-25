using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class Player : MonoBehaviour, MainActions.IPlayerActions
{
	// The number of lives.
	public int Lives { get; set; }
	public int Score { get; private set; }
	// Initial position.
	Vector2 firstPosition;
	// Flag for executing animations.
	bool inAnimation;

	LerpComponent lerpAnimations;
	ShootingComponent shootComponent;
	MovementComponent moveComponent;
	PolygonCollider2D polyCollider;

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
		EventList.enemyDeath += Player_OnEnemyDeath;
		EventList.grubCollect += Player_OnGrubCollect;
	}

    void Start()
	{
		Lives = 3;
		firstPosition = new Vector2(-2.47f, 4.4f);
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
			EventList.playerDeath.Invoke();
			StartCoroutine(Death());
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

	IEnumerator StartInitialAnimation(Vector2 finalPosition)
    {
		// Sets to Grub Layer.
		gameObject.layer = 15;
		// Starts movement animation.
		yield return MovementAnimations(firstPosition, finalPosition);
		// Sets to Default Layer.
		gameObject.layer = 0;
		// Changes sprite color.
		swapper.ToggleObjectsColor(ColorNames.Red);
		// Resets rotation to zero.
		transform.rotation = Quaternion.identity;
		// Enables controls and colliders
		mainActions.Player.Enable();
		// Resets animation flag.
		inAnimation = false;
	}

	IEnumerator MovementAnimations(Vector2 firstPosition, Vector2 finalPosition)
    {
		List<Grub> grubs = new List<Grub>(FindObjectsOfType<Grub>());
        if (grubs != null)
        {
			Vector2 currentPosition = firstPosition;
			Vector2 nextPosition;
            for (int i = 0; i < grubs.Count; i++)
            {
                if (grubs[i] == null)
                {
					continue;
                }

				nextPosition = grubs[i].gameObject.transform.position;
				yield return lerpAnimations.LerpPosition(currentPosition, nextPosition);
				currentPosition = nextPosition;
			}

			yield return lerpAnimations.LerpPosition(currentPosition, finalPosition);
        }
        else
        {
			yield return lerpAnimations.LerpPosition(firstPosition, finalPosition);
		}
    }

	IEnumerator Death() 
	{
		// Disabling player.
		inAnimation = true;
		mainActions.Player.Disable();

		// Death Animation.
		polyCollider.enabled = false;
		StartCoroutine(swapper.IterateColors());
		yield return lerpAnimations.LerpRotation(1080f);
		polyCollider.enabled = true;

		Lives -= 1;
        if (Lives == 0)
        {
			EventList.gameOver.Invoke();
        }

		// Restart Animation.
		yield return StartInitialAnimation(transform.position);
		EventList.waveStarted.Invoke();
	}

	void Player_OnEnemyDeath(int score)
    {
		Score += score;
    }

	private void Player_OnGrubCollect(int score)
	{
		Score += score;
		Debug.Log(Score);
	}

}
