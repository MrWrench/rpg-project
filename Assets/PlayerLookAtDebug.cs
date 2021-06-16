using UnityEngine;

public class PlayerLookAtDebug : MonoBehaviour
{
	private Camera _cam;

	private void Awake()
	{
		_cam = Camera.main;
	}

	// Start is called before the first frame update
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		var point = _cam.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * Vector3.Distance(_cam.transform.position, transform.position));
		var dir = (point - transform.position).normalized;
		float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0, angle, 0); 
	}
}