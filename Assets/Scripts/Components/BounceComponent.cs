using System.Collections;
using UnityEngine;

public class BounceComponent : MonoBehaviour
{
    public int BounceAmount { get; private set; }
    MovementComponent moveComponent;

    void Awake()
    {
        BounceAmount = HelperMethods.rand.Next(8);
        moveComponent = GetComponent<MovementComponent>();
    }

    public IEnumerator BounceRoutine()
    {
        //Snap rotate 180 degrees.
        gameObject.transform.Rotate(HelperMethods.GetRotationAngle(180f));
        //Pick left or right U turn
        yield return moveComponent.TransformRotate(HelperMethods.GetRandomAngle(180f, -180f), 0.5f);
    }

    public IEnumerator ResetRoutine()
    {
        //Snap rotate 180 degrees.
        gameObject.transform.Rotate(HelperMethods.GetRotationAngle(180f));
        yield return HelperMethods.GetWaitTime(1f);

    }

    public void ResetBounces(int maxAmount)
    {
        BounceAmount = HelperMethods.rand.Next(maxAmount);
    }

    public void DecreaseBounce()
    {
        BounceAmount--;
    }
}
