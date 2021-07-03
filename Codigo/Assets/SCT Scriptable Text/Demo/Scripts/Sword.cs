using System.Collections.Generic;
using UnityEngine;

namespace SCT.Demo
{
	[RequireComponent(typeof(BoxCollider))]
	public class Sword : MonoBehaviour
	{
		[SerializeField] private List<Sprite> m_icon;
		[SerializeField] private Stats Stats = null;
		[SerializeField] private float Damage = 15;
		[SerializeField, Range(0, 1)] private float MissRate = 0.15f;
		[SerializeField, Range(0, 1)] private float CritRate = 0.5f;
		[SerializeField] private float CritMultiplier = 3.5f;

		private void Start() { }

		private void OnTriggerEnter(Collider other)
		{
			if (Random.value <= MissRate)
			{
				ScriptableTextDisplay.Instance.InitializeScriptableText(2, transform.position, "miss");
				ScriptableTextDisplay
					.Instance.InitializeStackingScriptableText(1, transform.position,
															   Damage.ToString(), "ability name");
			}
			else
			{
				var target = other.GetComponent<IDamageable>();
				if (target == null) return;

				var crit = Random.value <= CritRate;
				var damage = crit == true ? Damage * CritMultiplier : Damage;
				Stats.AddDamage(damage);

				var rb = other.transform.GetComponent<Rigidbody>();

				var targetPos = rb.position;
				var currentPos = transform.position;
				var dir = (targetPos - currentPos).normalized;
				rb.AddForce(Vector3.up * 20, ForceMode.VelocityChange);
				rb.AddForce(dir * 20, ForceMode.VelocityChange);

				if (crit)
				{
					ScriptableTextDisplay.Instance.InitializeScriptableText(
						1, other.transform.position, damage.ToString(),m_icon[Random.Range(0,m_icon.Count)]);
				}
				else
				{
					ScriptableTextDisplay.Instance.InitializeScriptableText(
						0, other.transform.position, damage.ToString());
				}

				target.AddDamage(Damage);
			}
		}
	}
}