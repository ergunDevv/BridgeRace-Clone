

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarakterKontrol : MonoBehaviour
{
    public Transform collect_Main_Parrent;
    public GameObject prev_Object;
    public List<GameObject> cubes = new List<GameObject>();

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
                animator.SetBool("running", false);
            }
        }


    }

    private void Movement()

    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = cam.transform.localPosition.z;

        Ray ray = cam.ScreenPointToRay(mousePos);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
        {

            Vector3 hitVec = hit.point;

            hitVec.y = transform.position.y;

            transform.position = Vector3.MoveTowards(transform.position, Vector3.Lerp(transform.position, hitVec, lerpValue), Time.deltaTime * speed);
            Vector3 newMovePoint = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(newMovePoint - transform.position), turnSpeed * Time.deltaTime);
            if (!animator.GetBool("running"))
            {
                animator.SetBool("running", true);
            }




        }


    }
    private void OnTriggerEnter(Collider target)
    {
        if (target.gameObject.tag.StartsWith(transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material.name.Substring(0, 1)))
        {
            target.transform.SetParent(collect_Main_Parrent);
            Vector3 pos = prev_Object.transform.localPosition;

            pos.y += 0.22f;
            pos.z = 0;
            pos.x = 0;

            target.transform.localRotation = new Quaternion(0, 0.70711068f, 0, 0.70711068f);

            target.transform.DOLocalMove(pos, 0.2f);
            prev_Object = target.gameObject;
            cubes.Add(target.gameObject);

            target.tag = "Untagged";

            GenerateCubes.instance.GenerateCube(1);

        }
        if (cubes.Count > 0 && target.gameObject.tag == "DizR" || cubes.Count > 0 && target.gameObject.tag != "Diz" +
               transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material.name.Substring(0, 1)
               && target.gameObject.tag.StartsWith("Diz"))
        {

            GameObject obje = cubes[cubes.Count - 1];
            cubes.RemoveAt(cubes.Count - 1);
            Destroy(obje);

            target.GetComponent<MeshRenderer>().material = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material;
            target.GetComponent<MeshRenderer>().enabled = true;


            target.tag = "Diz" + transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material.name.Substring(0, 1);

            prev_Object = cubes[cubes.Count - 1];




        }
    }

}
