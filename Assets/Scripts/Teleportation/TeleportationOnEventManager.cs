using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationOnEventManager : MonoBehaviour {
    [SerializeField] private InputActionReference activate;
    [SerializeField] private XRRayInteractor _RayInteractor;
    [SerializeField] TeleportationProvider _teleportationProvider;

    [Space(10)]
    public UnityEvent OnTeleportActivated, OnTeleportCanceled;

    void Start () {
        _RayInteractor.enabled = false;
        activate.action.performed += TeleportActivate;
        activate.action.canceled += TeleportCanceled;
    }

    private void TeleportCanceled (InputAction.CallbackContext obj) {

        if (_RayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit ray) &&
             _RayInteractor.enabled == true) {
            TeleportRequest teleportRequest = new TeleportRequest();
            teleportRequest.destinationPosition = ray.point;
            _teleportationProvider.QueueTeleportRequest(teleportRequest);
        }
        _RayInteractor.enabled = false;

        if(OnTeleportCanceled != null)
        OnTeleportCanceled.Invoke();
    }

    private void TeleportActivate (InputAction.CallbackContext obj) {

        if (OnTeleportActivated != null)
            OnTeleportActivated.Invoke();

        _RayInteractor.enabled = true;
    }
}
