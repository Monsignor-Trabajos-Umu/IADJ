using UnityEngine;
using UnityEngine.UI;

namespace SCT.Demo
{
	public class KillCounter : MonoBehaviour
	{
		[SerializeField] private Stats Stats = null;
		[SerializeField] private Text Counter = null;

		private void OnEnable()
		{
			Stats.Reset();
			Stats.OnKillAdded += ShowKills;
		}

		private void ShowKills()
		{
			Counter.text = Stats.GetKills().ToString();
		}

		private void OnDisable()
		{
			Stats.OnKillAdded -= ShowKills;
		}
	}
}