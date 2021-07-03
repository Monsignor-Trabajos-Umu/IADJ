using System;
using UnityEngine;

namespace SCT.Demo
{
	[RequireComponent(typeof(Rigidbody))]
	public class Enemy : MonoBehaviour, IDamageable
	{
		[SerializeField] private Stats Stats = null;
		[SerializeField] private GameObject Target = null;
		[SerializeField] private float Speed = 5;
		[SerializeField] private float MaxHealth = 1000;

		private Rigidbody m_rigidbody = null;
		private float m_currenthealth = 0.0f;

		private void Start()
		{
			Stats.AddEnemy();
			m_currenthealth = MaxHealth;
			m_rigidbody = GetComponent<Rigidbody>();
		}

		private void Update()
		{
			if (Target == null) return;
			Movement();
		}

		public void SetTarget(GameObject target)
		{
			Target = target;
		}

		private void Movement()
		{
			var currentPos = m_rigidbody.position;
			var targetPosition = Target.transform.position;

			var distance = Vector3.Distance(targetPosition, currentPos);
			var follow = distance >= 5;

			if (!follow) return;

			var dir = (targetPosition - currentPos).normalized;
			m_rigidbody.MovePosition(currentPos + Speed * Time.deltaTime * dir);
		}

		public void AddDamage(float value)
		{
			m_currenthealth -= value;

			if (m_currenthealth <= 0)
			{
				Destroy(this.gameObject);
			}
		}

		private void OnDestroy()
		{
			Stats.AddKill();
		}
	}
}