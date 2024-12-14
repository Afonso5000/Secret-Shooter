using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keypad : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject Gate;
    private bool gateOpen;
    public void Interact(){

        gateOpen = !gateOpen;

        Gate.GetComponent<Animator>().SetBool("IsOpen", gateOpen);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
