using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SetPokeToFingerAttachPoint : MonoBehaviour
{
    [SerializeField]
    private Transform PokeAttachPoint;

    private XRPokeInteractor xrPokeInteractor;

    private void Start()
    {
        xrPokeInteractor = transform.parent.parent.GetComponentInChildren<XRPokeInteractor>();
        SetPokeAttachPoint();
    }

    private void SetPokeAttachPoint()
    {
        if (PokeAttachPoint == null) { Debug.Log("Poke Attach Point is null."); return; }
        if (xrPokeInteractor == null) { Debug.Log("XR Poke Interactor is null."); return; }

        xrPokeInteractor.attachTransform = PokeAttachPoint;
    }
}