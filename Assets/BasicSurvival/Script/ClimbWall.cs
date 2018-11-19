using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Invector.CharacterController
{
    public class ClimbWall : MonoBehaviour
    {
        public float ClimbSpeed = 0.05f;
        bool bCanClimb;
        bool bClimbing = false;

        vThirdPersonController cc;


        // Use this for initialization
        void Start()
        {
            cc = GetComponent<vThirdPersonController>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!bCanClimb)
                return;

            if (!bClimbing)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
                {
                    if (Input.GetKeyDown(KeyCode.E) && hit.collider.tag == "ClimbWall")
                    {
                        bClimbing = true;
                        cc.lockMovement = true;
                        cc.extraGravity = 0;
                    }
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    cc.lockMovement = false;
                    bClimbing = false;
                    cc.extraGravity = -10;
                }
                if (Input.GetKey(KeyCode.W))
                {
                    Vector3 tran = transform.position;                  
                    tran.y += ClimbSpeed;
                    transform.position = tran;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    Vector3 tran = transform.position;
                    tran.y -= ClimbSpeed;
                    transform.position = tran;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "ClimbWall")
                bCanClimb = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "ClimbWall")
            {
                bCanClimb = false;
                cc.lockMovement = false;
                bClimbing = false;
                cc.extraGravity = -10;
            }
        }
    }
}