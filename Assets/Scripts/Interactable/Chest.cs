using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : InteractableUnit
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void DisplayUI()
    {
        base.DisplayUI();
    }

    protected override void HideUI()
    {
        base.HideUI();
    }

    protected override void Interact()
    {
        base.Interact();
        Debug.Log("Child");
        Destroy(gameObject);
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
    }
}
