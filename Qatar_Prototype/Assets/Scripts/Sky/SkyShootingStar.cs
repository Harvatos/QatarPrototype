using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyShootingStar : MonoBehaviour
{
	private Vector3 direction;
	private float speed;
	private float lifeTime;

	private bool isDead = false;

	private void Start()
	{
		direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
		speed = Random.Range(100f, 500f);
		lifeTime = Random.Range(0.1f, 0.5f);
		Destroy(gameObject, 1f);
	}

	private void Update()
	{
		if (isDead)
			return;

		lifeTime -= Time.deltaTime;

		if(lifeTime > 0)
		{
			transform.Rotate(direction * speed * Time.deltaTime);
		}

		else
		{
			Destroy(transform.GetChild(0).gameObject);
			isDead = true;
		}
	}
}
