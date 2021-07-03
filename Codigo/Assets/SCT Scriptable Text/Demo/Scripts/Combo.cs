using UnityEngine;
using SCT;

namespace SCT.Demo
{
	public class Combo : MonoBehaviour
	{
		[SerializeField] private Stats Stats = null;

		private void OnEnable()
		{
			Stats.OnKillAdded = null;
			Stats.OnKillAdded += ShowComboPoints;
		}

		private void ShowComboPoints()

		{
			ScriptableTextDisplay.Instance.InitializeStackingScriptableText(3
																			, new Vector3(0, 0, 0)
																			, 1.ToString()
																			, "comboPoints");
		}

		private void OnDisable()
		{
			Stats.OnKillAdded -= ShowComboPoints;
		}
	}
}