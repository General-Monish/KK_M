using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class KitchenObject :NetworkBehaviour 
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IkitchenObjectParrent kitchenObjectparrent;
    public KitchenObjectSO GetKitchenObjectSO()
    {
        return  kitchenObjectSO;
    }

    public void SetkitchenObjectParrent(IkitchenObjectParrent kitchenobjectparrent )
    {
        if (this.kitchenObjectparrent != null)
        {
            this.kitchenObjectparrent.ClearKitchenObject();
        }
        this.kitchenObjectparrent = kitchenobjectparrent;

        if (kitchenObjectparrent.HasKitchenObject())
        {
            Debug.LogError("ikitchenobjectparrent Already Has Kitchen Object");
        }

        kitchenObjectparrent.SetKitchenObject(this);

        //transform.parent = kitchenObjectparrent.GetKitchenObejectFollowTransform();
        //transform.localPosition = Vector3.zero;
    }

    public IkitchenObjectParrent GetKitchenObjectparrent()
    {
        return kitchenObjectparrent;
    }

    public void DestroySelf()
    {
        kitchenObjectparrent.ClearKitchenObject();
        Destroy(gameObject);
    }


    public static void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO,IkitchenObjectParrent kitchenObjectParrent)
    {
        KitchenGameMultiplayer.Instance.SpawnKitchenObject(kitchenObjectSO, kitchenObjectParrent);

    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if(this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }
}
