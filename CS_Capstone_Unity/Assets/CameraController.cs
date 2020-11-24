using UnityEngine;

public class CameraController : MonoBehaviour
{
	public float panSpeed = 100f;
	public float zoomSPeed = 200f;
	public float panBorderThickness = 80f;
	public Vector2 panLimit;

	void Update()
	{
		Vector3 pos = transform.position;



		if(Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
		{
			pos.z += panSpeed * Time.deltaTime;
		} else if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
		{
			pos.z -= panSpeed * Time.deltaTime;
		} else if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
		{
			pos.x += panSpeed * Time.deltaTime;
		} else if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
		{
			pos.x -= panSpeed * Time.deltaTime;
		}
		else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
		{
			pos.y += zoomSPeed * Time.deltaTime;
		}
		else if (Input.GetAxis("Mouse ScrollWheel") > 0f)
		{
			pos.y -= zoomSPeed * Time.deltaTime;
		}



		transform.position = pos;
	}
}
