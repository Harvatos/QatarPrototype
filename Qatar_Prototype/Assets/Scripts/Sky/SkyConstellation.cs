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
	
	public int index = 0;

	private List<GameObject> tracedLines = new List<GameObject>();
	private bool drawingIsVisible;
	private float drawingTimer = 3;

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
			
			foreach (Renderer r in drawingRenderers)
			{
				r.material.SetColor("_BaseColor", new Color(0, 0, 0, 1 - (drawingTimer/ 3f)));
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

				//Display Drawing on last line
				if (index == additionalStars.Length - 1)
					StartCoroutine( ShowDrawing() );
			}
		}
	}

	IEnumerator ShowDrawing()
	{
		yield return new WaitForSeconds(3);
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

}


[System.Serializable]
public class Line
{
	public GameObject starA;
	public GameObject starB;
	public bool isTraced;
}
