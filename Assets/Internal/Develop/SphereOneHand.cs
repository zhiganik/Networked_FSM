using RootMotion.FinalIK;
using UnityEngine;

namespace Shakhtarsk.Develop
{
    public class SphereOneHand : MonoBehaviour
    {
        [SerializeField] private InteractionSystem interactionSystem;
        [SerializeField] private InteractionObject interactionObject;
        [SerializeField] private FullBodyBipedEffector effector;
        
        public Transform holdPoint; // The point where the object will lerp to when picked up
        public float pickUpTime = 0.3f; // Maximum lerp speed of the object. Decrease this value to give the object more weight

        private float holdWeight, holdWeightVel;
        private Vector3 pickUpPosition;
        private Quaternion pickUpRotation;
        
        void Start() {
            // Listen to interaction events
            interactionSystem.OnInteractionStart += OnStart;
            interactionSystem.OnInteractionPause += OnPause;
            interactionSystem.OnInteractionResume += OnDrop;
        }

        // Are we currently holding the object?
        private bool holding {
            get {
                return holdingLeft || holdingRight;
            }
        }

        // Are we currently holding the object with left hand?
        private bool holdingLeft
        {
            get
            {
                return interactionSystem.IsPaused(FullBodyBipedEffector.LeftHand) && interactionSystem.GetInteractionObject(FullBodyBipedEffector.LeftHand) == interactionObject;
            }
        }

        // Are we currently holding the object with right hand?
        private bool holdingRight
        {
            get
            {
                return interactionSystem.IsPaused(FullBodyBipedEffector.RightHand) && interactionSystem.GetInteractionObject(FullBodyBipedEffector.RightHand) == interactionObject;
            }
        }
        
        // Called by the InteractionSystem when an interaction is paused (on trigger)
        private void OnPause(FullBodyBipedEffector effectorType, InteractionObject interactionObject) {
            if (effectorType != FullBodyBipedEffector.LeftHand) return;

            // Make the object inherit the character's movement
            interactionObject.transform.parent = interactionSystem.transform;
			
            // Make the object kinematic
            var r = interactionObject.GetComponent<Rigidbody>();
            if (r != null) r.isKinematic = true;

            // Set object pick up position and rotation to current
            pickUpPosition = interactionObject.transform.position;
            pickUpRotation = interactionObject.transform.rotation;
            holdWeight = 0f;
            holdWeightVel = 0f;
        }

        // Clean up delegates
        void OnDestroy() {
            if (interactionSystem == null) return;

            interactionSystem.OnInteractionStart -= OnStart;
            interactionSystem.OnInteractionPause -= OnPause;
            interactionSystem.OnInteractionResume -= OnDrop;
        }

        private void OnDrop(FullBodyBipedEffector effectorType, InteractionObject interactionObject) {
            if (holding) return;

            // Make the object independent of the character
            this.interactionObject.transform.parent = null;
			
            // Turn on physics for the object
            if (this.interactionObject.GetComponent<Rigidbody>() != null) this.interactionObject.GetComponent<Rigidbody>().isKinematic = false;
        }

        void OnGUI() {
            if (GUI.Button(new Rect(200,200,20,20), "Start Interaction With " + interactionObject.name)) {

                interactionSystem.StartInteraction(effector, interactionObject, false);
            }
        }
        
        // Called by the InteractionSystem when an interaction starts
        private void OnStart(FullBodyBipedEffector effectorType, InteractionObject interactionObject) {
            if (effectorType != FullBodyBipedEffector.LeftHand) return;
            //if (interactionObject != obj) return;

            //holdPoint.rotation = obj.transform.rotation;
        }
        
        void LateUpdate() {
            if (holding) {
                // Smoothing in the hold weight
                holdWeight = Mathf.SmoothDamp(holdWeight, 1f, ref holdWeightVel, pickUpTime);

                // Interpolation
                interactionObject.transform.position = Vector3.Lerp(pickUpPosition, holdPoint.position, holdWeight);
                interactionObject.transform.rotation = Quaternion.Lerp(pickUpRotation, holdPoint.rotation, holdWeight);
            }
        }
    }
}