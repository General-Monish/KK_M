using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class KitchenObject :NetworkBehaviour 
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    FollowTransform followTransform;

    protected virtual void Awake()
    {
        followTransform = GetComponent<FollowTransform>();
    }

    private IkitchenObjectParrent kitchenObjectparrent;
    public KitchenObjectSO GetKitchenObjectSO()
    {
        return  kitchenObjectSO;
    }

    public void SetkitchenObjectParrent(IkitchenObjectParrent kitchenobjectparrent )
    {
        SetKitchenObjectParrentClientRpc(kitchenobjectparrent.GetNetworkObject());
    }

    [ServerRpc(RequireOwnership =false)]
    void SetKitchenObjectParrentServerRpc(NetworkObjectReference kitchenObjectParrentNetworkObjectReferance)
    {
        SetKitchenObjectParrentClientRpc(kitchenObjectParrentNetworkObjectReferance);
    }

    [ClientRpc]
    void SetKitchenObjectParrentClientRpc(NetworkObjectReference kitchenObjectParrentNetworkObjectReferance)
    {
        kitchenObjectParrentNetworkObjectReferance.TryGet(out NetworkObject kitchenObjectParrentNetworkObject);
        IkitchenObjectParrent ikitchenObjectParrent = kitchenObjectParrentNetworkObject.GetComponent<IkitchenObjectParrent>();

        if (this.kitchenObjectparrent != null)
        {
            this.kitchenObjectparrent.ClearKitchenObject();
        }
        this.kitchenObjectparrent = ikitchenObjectParrent;

        if (kitchenObjectparrent.HasKitchenObject())
        {
            Debug.LogError("ikitchenobjectparrent Already Has Kitchen Object");
        }

        kitchenObjectparrent.SetKitchenObject(this);
        followTransform.SetTargetTransform(kitchenObjectparrent.GetKitchenObejectFollowTransform());
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
