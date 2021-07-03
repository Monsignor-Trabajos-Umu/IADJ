using System;
using UnityEngine;

namespace SCT.Demo
{
	[CreateAssetMenu(menuName = "UI Stats")]
	public class Stats : ScriptableObject
	{
#region Events

		public Action OnKillAdded;
		public Action OnEnemyAdded;
		public Action OnDamageDealt;

#endregion

		private int m_enemiesKilled = 0;
		private int m_enemiesAlive = 0;
		private float m_damageDealt = 0;

		public void Reset()
		{
			m_enemiesKilled = 0;
			m_enemiesAlive = 0;
			m_damageDealt = 0;
		}

		/// <summary>Increase current kill count.</summary>
		public void AddKill()
		{
			m_enemiesKilled++;
			OnKillAdded?.Invoke();
		}

		/// <summary>Increase Enemy count.</summary>
		public void AddEnemy()
		{
			m_enemiesAlive++;
			OnEnemyAdded?.Invoke();
		}

		/// <summary>Increase dealt damage.</summary>
		public void AddDamage(float damage)
		{
			m_damageDealt += damage;
			OnDamageDealt?.Invoke();
		}

		/// <summary>Killed Enemies</summary>
		public int GetKills() => m_enemiesKilled;

		/// <summary>Enemies Alive</summary>
		public int GetEnemyCount() => m_enemiesAlive - m_enemiesKilled;

		/// <summary>All dealt Damage</summary>
		public float GetDealtDamage() => m_damageDealt;
	}
}