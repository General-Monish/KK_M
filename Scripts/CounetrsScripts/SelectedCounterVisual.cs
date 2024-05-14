using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter basecounter;
    [SerializeField] private GameObject[] visualGameObjectArray;

    private void Start()
    {
        if (Player.Localinstance != null)
        {
            Player.Localinstance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
        }
        else
        {
            Player.OnAnyPlayerSpawned += Player_OnAnyPlayerSpawned;
        }
        
    }

    private void Player_OnAnyPlayerSpawned(object sender, System.EventArgs e)
    {
        if (Player.Localinstance != null)
        {
            Player.Localinstance.OnSelectedCounterChanged -= Player_OnSelectedCounterChanged;
            Player.Localinstance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
        }
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (e.selectedCounter == basecounter)
        {
            show();
        }
        else
        {
            hide();
        }
    }

    private void show()
    {
        foreach (GameObject visualGameobject in visualGameObjectArray)
        {
            visualGameobject.SetActive(true);
        }
   
    }

    private void hide()
    {

        foreach (GameObject visualGameobject in visualGameObjectArray)
        {
            visualGameobject.SetActive(false);
        }

    }
}
