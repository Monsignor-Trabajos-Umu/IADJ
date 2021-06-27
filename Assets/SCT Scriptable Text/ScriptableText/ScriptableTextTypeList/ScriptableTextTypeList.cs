using System.Collections.Generic;
using UnityEngine;

namespace SCT
{
	[CreateAssetMenu(menuName = "ScriptableTextList/newList")]
	public class ScriptableTextTypeList : ScriptableObject
	{
		public List<ScriptableText> ScriptableTextTyps = new List<ScriptableText>();

		public int ListSize
		{
			get { return ScriptableTextTyps.Count; }
		}

		public string GetName(int index)
		{
			return ScriptableTextTyps[index].TextTypeName;
		}
	}
}