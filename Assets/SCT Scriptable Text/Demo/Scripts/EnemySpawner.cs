using System.Collections;
using UnityEngine;

namespace SCT.Demo
{
	public class EnemySpawner : MonoBehaviour
	{
		[Header("Needed")]
		[SerializeField] private GameObject EnemyTarget = null;

		[SerializeField] private Enemy Enemy = null;

		[Header("Init Settings")]
		[SerializeField] private int InitSpawn = 100;

		[SerializeField] private int SpawnCircles = 3;
		[SerializeField] private float RangeBetweenCircle = 5;

		[Header("Tic Settings")]
		[SerializeField] private float TicRate = 2.5f;

		private void Start()
		{
			StartCoroutine(TicSpawn(InitSpawn, SpawnCircles));
		}

		private void Spawn(Vector3 position)
		{
			var newEnemy = Instantiate(Enemy, position, Quaternion.identity, this.transform);
			newEnemy.GetComponent<Enemy>().SetTarget(EnemyTarget);
		}

		private IEnumerator TicSpawn(int count, int steps)
		{
			for (var i = 0; i < count; i++)
			{
				var angle = i * Mathf.PI * 2 / count;
				var step = (i % steps) + 1;
				var radius = step * RangeBetweenCircle;
				var targetPos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
				targetPos += EnemyTarget.transform.position;
				targetPos.y = 0.5f;

				Spawn(targetPos);
			}
				yield return new WaitForSeconds(TicRate);

			StartCoroutine(TicSpawn(count, steps));
		}
	}
}