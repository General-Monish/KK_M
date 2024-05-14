using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DeliveryManager : NetworkBehaviour
{

    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFail;
    private int successfulrecepiesAmount;
    public static DeliveryManager Instance { get; private set; }


    [SerializeField] private RecepySOList RecepySOList;
    private List<RecipySO> WaitingRecepySOlist;
    private float spawnRecepyTimer=4f;
    private float spawnRecepyTimerMax = 4f;
    private int WaitingRecepyMax = 4;
    private void Awake()
    {
        Instance = this;
        WaitingRecepySOlist = new List<RecipySO>();
    }

    private void Update()
    {
        if (!IsServer)
        {
            return;
        }
        spawnRecepyTimer -= Time.deltaTime;
        if (spawnRecepyTimer <= 0f)
        {
            spawnRecepyTimer = spawnRecepyTimerMax;
            if (KitchenGameManager.Instance.isGamePlaying()  && WaitingRecepySOlist.Count < WaitingRecepyMax)
            {
                int watingRecepySO = UnityEngine.Random.Range(0, RecepySOList.RecepySOLists.Count);

                SpawnWaitingRecipeClientRpc(watingRecepySO);

            }
        }
    }

    [ClientRpc]
    void SpawnWaitingRecipeClientRpc(int waitingRecipeSOIndex)
    {
        RecipySO waitingRecipeSO = RecepySOList.RecepySOLists[waitingRecipeSOIndex];

        WaitingRecepySOlist.Add(waitingRecipeSO);
        OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
    }
    public void DeliverRecepy(PlateKitchenObject plateKitchenObject)
    {
        for(int i = 0; i < WaitingRecepySOlist.Count; i++)
        {
            RecipySO waitingRecepySO = WaitingRecepySOlist[i];

            if (waitingRecepySO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                // has same no of ingredients
                bool platecontentMatchesRecepy = true;

                foreach (KitchenObjectSO RecepykitchenObjectSO in waitingRecepySO.kitchenObjectSOList)
                {
                    // cycling through all ingredients in recepy
                    bool ingredientfound = false;
                    foreach (KitchenObjectSO platekitchenobjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        // cycling through all ingredients in plate
                        if (platekitchenobjectSO == RecepykitchenObjectSO)
                        {
                            // ingredients matches
                            ingredientfound = true;
                            break;
                        }
                    }
                    if (!ingredientfound)
                    {
                        // this recepy was not ound on the plate
                        platecontentMatchesRecepy = false;
                    }
                }
                if (platecontentMatchesRecepy)
                {
                    // player delivered correct recepy

                    DeliverCorrectRecipeServerRpc(i);

                    return;
                }
            }
        }
        // no matches found
        // player did not deliver correct recepy

        DeliverIncorrectRecipeServerRpc();
    }

    [ServerRpc(RequireOwnership =false)]
    void DeliverIncorrectRecipeServerRpc()
    {
        DeliverIncorrectRecipeClientRpc();
    }
    [ClientRpc]
    void DeliverIncorrectRecipeClientRpc()
    {
        OnRecipeFail?.Invoke(this, EventArgs.Empty);
    }
    [ServerRpc(RequireOwnership =false)]
    void DeliverCorrectRecipeServerRpc(int waitingRecipeSoListIndex)
    {
        DeliverCorrectRecipeClientRpc(waitingRecipeSoListIndex);
    }

    [ClientRpc]
    void DeliverCorrectRecipeClientRpc(int waitingRecipeSoListIndex)
    {
        successfulrecepiesAmount++;
        WaitingRecepySOlist.RemoveAt(waitingRecipeSoListIndex);

        OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
        OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipySO> GetWaitingRecipeSOList()
    {
        return WaitingRecepySOlist;
    }
    public int getsuccessfulrecepiesAmount()
    {
        return successfulrecepiesAmount;
    }
}
