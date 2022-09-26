using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;


public enum Character
{
	sýfýr=0,
	iki=2

}
public class CharacterAI : MonoBehaviour
{

	public GameObject targetsParent;
	public List<GameObject> targets = new List<GameObject>();
    public Transform collect_Main_Parrent;
    public GameObject prev_Object;
	public List<GameObject> cubes = new List<GameObject>();
	public Transform[] ropes;



	private Animator animator;
	private NavMeshAgent agent;

	private Vector3 targetTransform;

	private bool have_Target = false;
	public float radius=2;


	public Character character_Enum;
	

	void Start()
	{
		animator = GetComponent<Animator>(); 
		for (int i = 0; i < targetsParent.transform.childCount; i++)
		{
			targets.Add(targetsParent.transform.GetChild(i).gameObject);
		}
		agent = GetComponent<NavMeshAgent>();

	}

	// Update is called once per frame
	void Update()
	{
		if (!have_Target&&targets.Count>0)
		{
			ChooseTarget();	
		}

	}
	void ChooseTarget()
	{
		int randomNumber = Random.Range(0, 3);
		
		if(randomNumber==0&&cubes.Count>=5)
		{
			int randomRope = Random.Range(0, ropes.Length);
			List<Transform> ropesNonActiveChild = new List<Transform>();
			foreach (Transform item in ropes[randomRope])
			{
				if(!item.GetComponent<MeshRenderer>().enabled || item.GetComponent<MeshRenderer>().enabled &&item.gameObject.tag !="Diz"+transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material.name.Substring(0, 1)){
					ropesNonActiveChild.Add(item);	
				}
			}
			targetTransform = cubes.Count > ropesNonActiveChild.Count ? ropesNonActiveChild [ropesNonActiveChild.Count - 1].position : ropesNonActiveChild[cubes.Count].position;




		}
		else
		{
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
            List<Vector3> ourColors = new List<Vector3>();
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].tag.StartsWith(transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material.name.Substring(0, 1)))
                {
                    ourColors.Add(hitColliders[i].transform.position);
                }
            }
            if (ourColors.Count > 0)
            {
                targetTransform = ourColors[0];

            }
            else
            {
                int random = Random.Range(0, targets.Count);
                targetTransform = targets[random].transform.position;

            }

        }




        agent.SetDestination(targetTransform);
		if (!animator.GetBool("running"))
		{
			animator.SetBool("running", true);

		}
		have_Target = true;


		
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

			targets.Remove(target.gameObject);
			target.tag = "Untagged";
			have_Target = false;

			GenerateCubes.instance.GenerateCube((int)character_Enum ,this );

			
		}
		else if(target.gameObject.tag == "DizR" || target.gameObject.tag != "Diz"
            + transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material.name.Substring(0, 1)
            && target.gameObject.tag.StartsWith("Diz"))
		{

			if(cubes.Count > 1)
			{
				GameObject obje=cubes[cubes.Count-1];
				cubes.RemoveAt(cubes.Count-1);
				Destroy(obje);

				target.GetComponent<MeshRenderer>().material = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material;
				target.GetComponent<MeshRenderer>().enabled = true;


				target.tag="Diz"+ transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material.name.Substring(0, 1);



			}
			else
			{
				prev_Object = cubes[0].gameObject;
				have_Target = false;

			}

		}
	}
	

}
