using UnityEngine;

namespace SCT.Demo
{
	public class Player : MonoBehaviour
	{
		[SerializeField] private float Speed = 10.0f;
		private Vector3 m_moveDir = new Vector3(0, 0, 0);

		private void Update()
		{
			Input();
			Movement();
		}

		private void Input()
		{
			var horizontal = UnityEngine.Input.GetAxis("Horizontal");
			var vertical = UnityEngine.Input.GetAxis("Vertical");
			m_moveDir.x = horizontal;
			m_moveDir.z = vertical;
		}

		private void Movement()
		{
			transform.position += Time.deltaTime * Speed * m_moveDir;
		}
	}
}