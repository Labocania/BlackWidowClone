using System.Collections;

public class PlayerChaser : ChasingType
{
    protected override void Awake()
    {
        base.Awake();
        PlayerTarget = HelperMethods.playerTransform;
        IsChasing = true;
    }

    protected override IEnumerator PickTarget()
    {
        while (GrubTarget == null)
        {
            GrubTarget = HelperMethods.SelectBug();
            if (GrubTarget != null && GrubTarget.CompareTag("Grub") == false)
            {
                GrubTarget = null;
            }
            yield return HelperMethods.GetWaitTime(0.5f);
        }

        IsChasing = true;
    }
}
