using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class Camera2DFollow : MonoBehaviour
    {
        public Transform target;
        public float damping = 1;
        public float lookAheadFactor = 3;
        public float lookAheadReturnSpeed = 0.5f;
        public float lookAheadMoveThreshold = 0.1f;
        public float yPosRestriction = -1;

        private float OffsetZ;
        private Vector3 LastTargetPosition;
        private Vector3 CurrentVelocity;
        private Vector3 LookAheadPos;

        float nextTimeToSearch = 0;

        // Use this for initialization
        private void Start()
        {
            LastTargetPosition = target.position;
            OffsetZ = (transform.position - target.position).z;
            transform.parent = null;
        }


        // Update is called once per frame
        private void Update()
        {
            if (target == null)
            {
                FindPlayer ();
                return;
            }
              
            // only update lookahead pos if accelerating or changed direction
            float xMoveDelta = (target.position - LastTargetPosition).x;

            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

            if (updateLookAheadTarget)
            {
                LookAheadPos = lookAheadFactor*Vector3.right*Mathf.Sign(xMoveDelta);
            }
            else
            {
                LookAheadPos = Vector3.MoveTowards(LookAheadPos, Vector3.zero, Time.deltaTime*lookAheadReturnSpeed);
            }

            Vector3 aheadTargetPos = target.position + LookAheadPos + Vector3.forward*OffsetZ;
            Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref CurrentVelocity, damping);

            newPos = new Vector3(newPos.x, Mathf.Clamp (newPos.y, yPosRestriction,Mathf.Infinity), newPos.z);

            transform.position = newPos;

            LastTargetPosition = target.position;
        }

        void FindPlayer() {
            if (nextTimeToSearch <= Time.time) {
              GameObject searchResult =  GameObject.FindGameObjectWithTag("Player");
                if (searchResult != null)
                {
                    target = searchResult.transform;
                    nextTimeToSearch = Time.time + 0.5f;
                }
            }
        }
    }
}
