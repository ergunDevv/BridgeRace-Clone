using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarakterKontrol : MonoBehaviour
{


    private Camera cam;
    private Animator animator;

    public float turnSpeed, speed, lerpValue;
    public LayerMask layer;



    void Start()
    {

        cam = Camera.main;
        animator = GetComponent<Animator>();

        
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            Movement();
        }
        else
        {
            if (animator.GetBool("running"))
            {
                animator.SetBool("running",false);
            }
        }


    }

    private void Movement()
    
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = cam.transform.localPosition.z;

        Ray ray = cam.ScreenPointToRay(mousePos);

        RaycastHit hit;

        if(Physics.Raycast(ray,out hit , Mathf.Infinity, layer))
        {

            Vector3 hitVec = hit.point;

            hitVec.y = transform.position.y;

            transform.position = Vector3.MoveTowards(transform.position, Vector3.Lerp(transform.position, hitVec, lerpValue),Time.deltaTime*speed);
            Vector3 newMovePoint = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(newMovePoint - transform.position), turnSpeed * Time.deltaTime);
            if (!animator.GetBool("running"))
            {
                animator.SetBool("running", true);
            }

                
         

        }


    }


}
