using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mosquito : Insect
{
    List<Vector3> rotationAngles = new List<Vector3>();
    List<WaitForSeconds> waitTimes = new List<WaitForSeconds>();
    System.Random randomIndex = new System.Random();

    protected override void Awake()
    {
        base.Awake();

        //Left rotation
        rotationAngles.Add(new Vector3(0f, 0f, 45f));
        //Right rotation
        rotationAngles.Add(new Vector3(0f, 0f, -45f));
        //U-turn left
        rotationAngles.Add(new Vector3(0f, 0f, 180f));
        //U-turn right
        rotationAngles.Add(new Vector3(0f, 0f, -180f));

        waitTimes.Add(new WaitForSeconds(1f));
        waitTimes.Add(new WaitForSeconds(2f));
        waitTimes.Add(new WaitForSeconds(5f));
        waitTimes.Add(new WaitForSeconds(8f));
    }

    protected override void OnEnable() => base.OnEnable();
    protected override void OnDisable() => base.OnDisable();
    protected override void OnBecameInvisible() => base.OnBecameInvisible();

    void FixedUpdate()
    {
        if (gameObject.activeSelf == true)
        {
            moveComponent.TransformMove(transform.up);       
        }
    }

    protected override IEnumerator StartMovementRoutine()
    {
        while (gameObject.activeSelf == true && animating == false)
        {
            yield return waitTimes[randomIndex.Next(waitTimes.Count)];
            yield return moveComponent.TransformRotate(rotationAngles[randomIndex.Next(rotationAngles.Count)], 3f);
        }
    }
   
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            base.Die();
        }
    }

}
