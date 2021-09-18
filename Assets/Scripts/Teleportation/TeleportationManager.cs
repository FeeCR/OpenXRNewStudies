using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationManager : MonoBehaviour
{

    [SerializeField] private InputActionAsset actionAsset;
    [SerializeField] private XRRayInteractor rayInteractorL, rayInteractorR;
    [SerializeField] private TeleportationProvider provider;
    private InputAction _thumbstickL, _thumbstickR;
    private bool _isActiveL, _isActiveR;

    public bool leftHandTeleport = true, rightHandTeleport = true;

    // Start is called before the first frame update
    void Start()
    {

        rayInteractorL.enabled = false;
        rayInteractorR.enabled = false;

        InputAction activateL, cancelL;
        InputAction activateR, cancelR;

        if (leftHandTeleport){

            //Map Left Hand Input to Teleportation
            activateL = actionAsset.FindActionMap("XRI LeftHand").FindAction("Teleport Mode Activate");
            activateL.Enable();
            activateL.performed += OnTeleportLeftHandActivate;

            //Map Left Hand Input to Cancel Teleportation
            cancelL = actionAsset.FindActionMap("XRI LeftHand").FindAction("Teleport Mode Cancel");
            cancelL.Enable();
            cancelL.performed += OnTeleportLeftHandCancel;

            _thumbstickL = actionAsset.FindActionMap("XRI LeftHand").FindAction("Move");
            _thumbstickL.Enable();

        }

        if (rightHandTeleport) {

            //Map Right Hand Input to Teleportation
            activateR = actionAsset.FindActionMap("XRI RightHand").FindAction("Teleport Mode Activate"); 
            activateR.Enable();
            activateR.performed += OnTeleportRightHandActivate;


            //Map Right Hand Input to Cancel Teleportation
            cancelR = actionAsset.FindActionMap("XRI RightHand").FindAction("Teleport Mode Cancel");
            cancelR.Enable();
            cancelR.performed += OnTeleportRightHandCancel;

            _thumbstickR = actionAsset.FindActionMap("XRI RightHand").FindAction("Move");
            _thumbstickR.Enable();
        }
        

        
    }

    private void Update () {

        if (!_isActiveL && !_isActiveR)
            return;

        if(leftHandTeleport)
            CheckLeftHandTeleportation();

        if (rightHandTeleport)
            CheckRightHandTeleportation();

    }

    void CheckLeftHandTeleportation () {

        if (_isActiveR)
            return;

        if (_thumbstickL.triggered)
            return;

        if (!rayInteractorL.TryGetCurrent3DRaycastHit(out RaycastHit hit)) { //Updated the method
            SetTeleportationState(Hands.LEFT, false);
            return;
        }

        TeleportRequest request = new TeleportRequest() {
            destinationPosition = hit.point,
            //destinationRotation = ?? //Try to fix the rotation based on the update of the thumbstick

        };

        provider.QueueTeleportRequest(request);
        SetTeleportationState(Hands.LEFT, false);
    }

    void CheckRightHandTeleportation () {

        if (_isActiveL)
            return;

        if (_thumbstickR.triggered)
            return;

        //Debug.Log("Not here");

        if (!rayInteractorR.TryGetCurrent3DRaycastHit(out RaycastHit hit)) { //Updated the method
            SetTeleportationState(Hands.RIGHT, false);
            return;
        }

        TeleportRequest request = new TeleportRequest() {
            destinationPosition = hit.point,
            //destinationRotation = ?? //Try to fix the rotation based on the update of the thumbstick

        };

        provider.QueueTeleportRequest(request);
        SetTeleportationState(Hands.RIGHT, false);

    }

    private void OnTeleportLeftHandActivate (InputAction.CallbackContext context) {
        SetTeleportationState(Hands.LEFT, true);
    }

    private void OnTeleportLeftHandCancel (InputAction.CallbackContext context) {
        SetTeleportationState(Hands.LEFT, false);
    }

    private void OnTeleportRightHandActivate (InputAction.CallbackContext context) {
        SetTeleportationState(Hands.RIGHT, true);
    }

    private void OnTeleportRightHandCancel (InputAction.CallbackContext context) {
        SetTeleportationState(Hands.RIGHT, false);
    }

    void SetTeleportationState(Hands hand,bool state) {

        if(hand == Hands.LEFT) {
            rayInteractorL.enabled = state;
            _isActiveL = state;
        } else {
            rayInteractorR.enabled = state;
            _isActiveR = state;
        }
    }

}

enum Hands {
    LEFT,
    RIGHT
}
