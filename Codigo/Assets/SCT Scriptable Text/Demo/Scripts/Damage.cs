using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SCT.Demo
{
	public class Damage : MonoBehaviour
	{
		[SerializeField] private Stats Stats = null;
		[SerializeField] private Text Counter = null;

		private void OnEnable()
		{
			Stats.Reset();
			Stats.OnDamageDealt += DamageDealt;
		}

		private void DamageDealt()
		{
			Counter.text = Stats.GetDealtDamage().ToString();
		}

		private void OnDisable()
		{
			Stats.OnDamageDealt -= DamageDealt;
		}
	}
}