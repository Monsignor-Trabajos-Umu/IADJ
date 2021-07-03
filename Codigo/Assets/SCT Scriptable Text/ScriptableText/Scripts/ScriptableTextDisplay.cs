using System.Collections.Generic;
using UnityEngine;

namespace SCT
{
	public class ScriptableTextDisplay : MonoBehaviour
	{
#region Singelton

		private static ScriptableTextDisplay m_instance = null;

		public static ScriptableTextDisplay Instance
		{
			get
			{
				if (m_instance == null)
				{
					var target = FindObjectOfType<ScriptableTextDisplay>();
					m_instance = target;
				}

				return m_instance;
			}
		}

#endregion

		[Tooltip("Overlay Camera")]
		[SerializeField] private Camera m_targetCamera = null;

		[SerializeField,
		 Tooltip("Pool is not increasing and recycle already instantiated objects.")]
		private bool m_forceRecycle = false;

		[Tooltip("Initial Pool size.")]
		[SerializeField] private int m_poolSize = 10;

		[Tooltip("Used to fill pools.")]
		[SerializeField] private ScriptableTextComponent m_scriptableTextComponent = null;

		[Tooltip("List with all the Text Types you wan to use.")]
		[SerializeField] private ScriptableTextTypeList m_textTypeList = null;

		public ScriptableTextTypeList TextTypeList
		{
			get { return m_textTypeList; }
		}

		private Dictionary<string, ScriptableTextComponent> m_stackingText =
			new Dictionary<string, ScriptableTextComponent>();

		//for each pool one specific parent and List
		[System.Serializable]
		public class TextPool
		{
			public GameObject PoolHolder;
			public List<GameObject> Pool = new List<GameObject>();
			public List<ScriptableTextComponent> Component = new List<ScriptableTextComponent>();
			public int Idx = 0;
		}

		private TextPool[] m_objectPool;

#region Setup

		private void Awake()
		{
			if (Instance != null && Instance != this)
			{
				Destroy(this.gameObject);
			}
			else
			{
				m_instance = this;
			}
		}

		private void Start()
		{
			InitializePools();
		}

		void CreateParent()
		{
			//Initialize Array with Length of TextTypeList
			m_objectPool = new TextPool[m_textTypeList.ListSize];

			for (var i = 0; i < m_textTypeList.ListSize; i++)
			{
				var newOP = new TextPool
				{
					PoolHolder = new GameObject(m_textTypeList.GetName(i) + "Parent"
											  , typeof(RectTransform)
											  , typeof(Canvas))
				};

				newOP.PoolHolder.transform.SetParent(gameObject.transform);
				newOP.PoolHolder.transform.localScale = new Vector3(1, 1, 1);
				m_objectPool[i] = newOP;
			}
		}

		void InitializePools()
		{
			//Create Parent for each TextType 
			CreateParent();

			//Create x Amount of Objects
			//store each Object in the right pool and list
			//set Pool Parent
			for (var t = 0; t < m_textTypeList.ScriptableTextTyps.Count; t++)
			{
				for (var i = 0; i < m_poolSize; i++)
				{
					var sctComponent = Instantiate(m_scriptableTextComponent,
												   m_objectPool[t].PoolHolder.transform,
												   false);
					sctComponent.gameObject.SetActive(false);
					m_objectPool[t].Pool.Add(sctComponent.gameObject);
					m_objectPool[t].Component.Add(sctComponent);
				}
			}
		}

#endregion

		private int GetIndex(int index)
		{
			if (m_forceRecycle)
			{
				m_objectPool[index].Idx =
					(m_objectPool[index].Idx + 1) % m_objectPool[index].Pool.Count;
				m_objectPool[index].Pool[m_objectPool[index].Idx].SetActive(true);
				return m_objectPool[index].Idx;
			}

			//look through all GameObjects in the array
			for (var i = 0; i < m_objectPool[index].Pool.Count; i++)
			{
				//If one GameObject is not Active, return this GameObject
				if (!m_objectPool[index].Pool[i].activeInHierarchy)
				{
					//enable the GameObject before return
					m_objectPool[index].Pool[i].SetActive(true);
					m_objectPool[index].Idx = i;
					return i;
				}
			}

			//If there is no active GameObject the for loop just return nothing and this code get called
			//creates an GameObject, parent it,add it to the Pool(object itself and Component), activate it and return it
			var sctComponent =
				Instantiate(m_scriptableTextComponent, m_objectPool[index].PoolHolder.transform,
							false);
			m_objectPool[index].Pool.Add(sctComponent.gameObject);
			m_objectPool[index]
				.Component.Add(sctComponent.GetComponent<ScriptableTextComponent>());
			sctComponent.gameObject.SetActive(true);
			return m_objectPool[index].Pool.Count - 1;
		}

		/// <summary>
		/// Initialize a Scriptable Text.
		/// </summary>
		/// <param Name="pos">Just paste your Position, handle offset and Randomness your SCT</param>
		/// <param Name="msg">What you want to output.</param>
		/// <param name="listPosition">From which CustomType in your List. eg [0]DamageText</param>
		public void InitializeScriptableText(int listPosition, Vector3 pos, string msg)
		{
			//Get the position from an active and ready to use GameObject
			var poolArrayIndex = GetIndex(listPosition);

			//prepare start Positions
			var sct = m_textTypeList.ScriptableTextTyps[listPosition];

			// call Initialize Method 
			m_objectPool[listPosition].Component[poolArrayIndex]
									  .Initialize(sct, pos, msg, m_targetCamera);
		}

		public void InitializeScriptableText(int listPosition, Vector3 pos, string msg, Sprite icon)
		{
			//Get the position from an active and ready to use GameObject
			var poolArrayIndex = GetIndex(listPosition);

			//prepare start Positions
			var sct = m_textTypeList.ScriptableTextTyps[listPosition];

			// call Initialize Method 
			var sctComponent = m_objectPool[listPosition].Component[poolArrayIndex];
			sctComponent.Initialize(sct, pos, msg, icon, m_targetCamera);
		}

		public void InitializeStackingScriptableText(int listPosition,
													 Vector3 pos,
													 string value,
													 string name)
		{
			if (m_stackingText.ContainsKey(name) == true)
			{
				if (!m_stackingText[name].gameObject.activeInHierarchy)
					m_stackingText.Remove(name);
			}

			var sct = m_textTypeList.ScriptableTextTyps[listPosition];
			if (m_stackingText.ContainsKey(name) == false)
			{
				//Get the position from an active and ready to use GameObject
				int poolArrayIndex = GetIndex(listPosition);

				//prepare start Positions

				// call Initialize Method 

				var scriptableTextComponent = m_objectPool[listPosition].Component[poolArrayIndex];
				scriptableTextComponent.Initialize(sct, pos, value, m_targetCamera);
				m_stackingText.Add(name, scriptableTextComponent);
			}
			else
			{
				m_stackingText[name].SetStackValue(sct, value, pos);
			}
		}

		/// <summary>
		/// Disable all ScriptableTextDisplay GameObjects.
		/// </summary>
		public void DisableAll()
		{
			for (var i = 0; i < m_textTypeList.ListSize; i++)
			{
				for (var p = 0; p < m_objectPool[i].Pool.Count - 1; p++)
				{
					m_objectPool[i].Pool[p].SetActive(false);
				}
			}
		}

		/// <summary>
		/// /// Disable all ScriptableTextDisplay GameObjects from a specific type
		/// </summary>
		/// <param name="idx">Text Type index</param>
		public void DisableAll(int idx)
		{
			for (var p = 0; p < m_objectPool[idx].Pool.Count - 1; p++)
			{
				m_objectPool[idx].Pool[p].SetActive(false);
			}
		}
	}
}