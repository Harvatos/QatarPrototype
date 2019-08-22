using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class SkyConstellation : MonoBehaviour
{
	public GameObject linePrefab;
	public GameObject[] defaultStars;
	public GameObject[] additionalStars;
	public Line[] lines;
	public VisualEffect ShineVFX;
	public Renderer[] drawingRenderers;
	public Camera cameraTarget;
	
	public int index = 0;

	private List<GameObject> tracedLines = new List<GameObject>();
	private bool drawingIsVisible;
	private float drawingTimer = 3;
	private Vector3 gameplayCamPosition;
	private Quaternion gameplayCamRotation;
	private float gameplayCamFOV;

	private void Start()
	{
		//Disable stars
		foreach(GameObject go in additionalStars)
		{
			go.SetActive(false);
		}

		//Trace default Lines
		for (int i = 0; i < lines.Length; i++)
		{
			TraceLine(lines[i]);
		}

		//Hide Drawing
		foreach(Renderer r in drawingRenderers)
		{
			r.material.SetColor("_BaseColor", new Color(0,0,0,0));
		}
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.C))
		{
			AddStar();
		}

		//Draw Aquila
		if(drawingIsVisible && drawingTimer > 0)
		{
			drawingTimer -= Time.deltaTime / 3f;
			
			for(int i=0; i<drawingRenderers.Length; i++)
			{
				Renderer r = drawingRenderers[i];
				float a = 1 - (drawingTimer / 3f);
				a = Mathf.Clamp(a, 0, 1);
				r.material.SetColor("_BaseColor", new Color(1, 1, 1, a));
			}
		}
	}

	public void AddStar()
	{
		if (index >= additionalStars.Length)
			return;

		//Activate Star
		additionalStars[index].SetActive(true);
		additionalStars[index].GetComponent<Animator>().SetTrigger("Play");

		//Trace Line
		for(int i=0; i<lines.Length;i++)
		{
			TraceLine(lines[i]);
		}

		//index
		index++;
	}

	private void TraceLine(Line l)
	{
		if (!l.isTraced)
		{
			if (l.starB.activeSelf && l.starA.activeSelf)
			{
				l.isTraced = true;

				//Create Line
				GameObject line = Instantiate(linePrefab, transform);

				//Set Position
				line.transform.position = l.starA.transform.GetChild(0).position;

				//Set Rotation
				line.transform.LookAt(l.starB.transform.GetChild(0), Vector3.zero - line.transform.position);

				//Set Length
				line.transform.localScale = new Vector3(1, 1, Vector3.Distance(l.starA.transform.GetChild(0).position, l.starB.transform.GetChild(0).position) / 10);

				//Add in list
				tracedLines.Add(line);

				//Cam Closeup
				bool lastStar = index == additionalStars.Length - 1;
				CinematicLookAtConstellation(lastStar ? 6 : 3);

				//Display Drawing on last line
				if (lastStar)
					StartCoroutine( ShowDrawing() );
			}
		}
	}

	IEnumerator ShowDrawing()
	{
		yield return new WaitForSeconds(2);
		drawingIsVisible = true;
	}

	//Make that DING when the constellation is alligned
	public void Shine()
	{
		//Default Stars
		foreach(GameObject o in defaultStars)
		{
			o.GetComponentInChildren<SkyStarColor>().Shine(50);
		}

		//Additional Stars
		for(int i=0; i<index; i++)
		{
			additionalStars[i].GetComponentInChildren<SkyStarColor>().Shine(50);
		}

		//Lines
		foreach(GameObject o in tracedLines)
		{
			o.GetComponent<SkyStarLineColor>().Shine(50);
		}

		if(ShineVFX != null)
			ShineVFX.SendEvent("Play");

	}

	//called when a star is liberated
	public void CinematicLookAtConstellation(float time)
	{
		//Store data
		Camera cam = CameraSingleton.instance.GetComponent<Camera>();
		gameplayCamPosition = cam.transform.position;
		gameplayCamRotation = cam.transform.rotation;
		gameplayCamFOV = cam.fieldOfView;

		//Move Cam
		cam.transform.position = cameraTarget.transform.position;
		cam.transform.rotation = cameraTarget.transform.rotation;
		cam.fieldOfView = cameraTarget.fieldOfView;
		cam.GetComponentInParent<CameraControls>().camIsLocked = true;

		//Reset Cam
		StartCoroutine( PlaceCamBack(cam, time) );
	}

	IEnumerator PlaceCamBack(Camera cam, float time)
	{
		yield return new WaitForSeconds(time);
		//cam.transform.position = gameplayCamPosition;
		//cam.transform.rotation = gameplayCamRotation;
		//cam.fieldOfView = gameplayCamFOV;
		cam.GetComponentInParent<CameraControls>().camIsLocked = false;

	}
}


[System.Serializable]
public class Line
{
	public GameObject starA;
	public GameObject starB;
	public bool isTraced;
}
