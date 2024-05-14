using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class KitchenGameMultiplayer : NetworkBehaviour
{
    public static KitchenGameMultiplayer Instance { get; private set; }

    [SerializeField] private kitchenobjectListSO kitchenobjectListSO;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    public  void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IkitchenObjectParrent kitchenObjectParrent)
    {

        SpawnKitchenObjectServerRpc(GetkitchenObjectIndexSO(kitchenObjectSO), kitchenObjectParrent.GetNetworkObject());
    }

   [ServerRpc(RequireOwnership =false)]
    void SpawnKitchenObjectServerRpc(int  kitchenObjectSOindex, NetworkObjectReference  kitchenObjectParrentNetworkObjectReferance)
    {
        KitchenObjectSO kitchenObjectSO = GetkitchenObjectSOfromIndex(kitchenObjectSOindex);
        Transform kitchenobjectTransform = Instantiate(kitchenObjectSO.prefab);
        NetworkObject kitchenObjNetworkObject = kitchenobjectTransform.GetComponent<NetworkObject>();
        kitchenObjNetworkObject.Spawn(true);

        KitchenObject kitchenObject = kitchenobjectTransform.GetComponent<KitchenObject>();

        kitchenObjectParrentNetworkObjectReferance.TryGet(out NetworkObject kitchenObjectParrentNetworkObject);
        IkitchenObjectParrent ikitchenObjectParrent = kitchenObjectParrentNetworkObject.GetComponent<IkitchenObjectParrent>();
        kitchenObject.SetkitchenObjectParrent(ikitchenObjectParrent);
    }

    int GetkitchenObjectIndexSO(KitchenObjectSO kitchenObjectSO)
    {
        return kitchenobjectListSO.kitchenObjectListSO.IndexOf(kitchenObjectSO);
    }

    KitchenObjectSO GetkitchenObjectSOfromIndex(int kitchenObjectSOIndex)
    {
        return kitchenobjectListSO.kitchenObjectListSO[kitchenObjectSOIndex];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
