using UnityEngine;

public class ClearCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        // Counter not contains kitchen object
        if (!HasKitchenObject())
        {
            // Player carring kitchen object
            if (player.HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }

        // Counter contains kitchen object
        else
        {
            // Player carring kitchen object
            if (player.HasKitchenObject())
            {

            }
            // Player not carring kitchen object
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}
