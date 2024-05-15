using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ContainerCounter : BaseCounter

    
{

    public event EventHandler OnPlayerGrabbedObject;
    [SerializeField] private KitchenObjectSO kitchenObjSO;
    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            // Player isnt carrying anything


            KitchenObject.SpawnKitchenObject(kitchenObjSO, player);
            InteractLogicServerRpc();
        }
        
    }

    [ServerRpc(RequireOwnership = false)]
    void InteractLogicServerRpc()
    {
        InteractLogicClientRpc();
    }
    [ClientRpc]
    void InteractLogicClientRpc()
    {
        OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
    }
}
