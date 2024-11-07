using System;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public class OnProgressChangedEventArgs : EventArgs
    {
        public float progressNormalized;
    }

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;

    public override void Interact(Player player)
    {
        // Counter not contains kitchen object
        if (!HasKitchenObject())
        {
            // Player carring kitchen object
            if (player.HasKitchenObject())
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    cuttingProgress = 0;
                    // player carring something that can be cutted
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax,
                    });
                }
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

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            cuttingProgress += 1;

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax,
            });

            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                KitchenObjectSO outputKitchenObject = GetOutputFromInput(GetKitchenObject().GetKitchenObjectSO());

                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(outputKitchenObject, this);
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO recipe = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return recipe != null;
    }

    private KitchenObjectSO GetOutputFromInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO recipe = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        if (recipe != null)
        {
            return recipe.output;
        }
        else
        {
            return null;
        }
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO recipe in cuttingRecipeSOArray)
        {
            if (recipe.input == inputKitchenObjectSO)
            {
                return recipe;
            }
        }
        return null;
    }
}
