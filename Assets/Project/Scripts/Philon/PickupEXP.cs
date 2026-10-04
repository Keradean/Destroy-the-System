using UnityEngine;

public class PickupEXP : Pickup
{
    [SerializeField] private float expAmount = 10f;

    protected override void PickupAction(GameObject player)
    {
        var expHandler = player.GetComponent<EXPHandler>();
        if (expHandler != null)
        {
            expHandler.AddExp(expAmount);
        }
    }

    public void SetExpAmount(float amount)
    {
        expAmount = amount;
    }
}
