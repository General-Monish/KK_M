using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class animation_controller : NetworkBehaviour
{

    private const string walk = "iswalk";

    [SerializeField] private Player player;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        anim.SetBool(walk, player.IsWalk());
    }
}
