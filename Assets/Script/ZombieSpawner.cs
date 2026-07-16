using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
	[SerializeField] GameObject enemyPrefab;
	[SerializeField] Transform player;
	[SerializeField] float spawnInterval = 2f;
	[SerializeField] float spawnRadius = 20f;

	float timer;
	private void Update()
	{
		timer += Time.deltaTime;
		if(timer >= spawnInterval)
		{
			timer = 0f;
			SpawnZombie();
		}
	}

	void SpawnZombie()
	{
		Vector2 random = Random.insideUnitCircle.normalized * spawnRadius;
		Vector3 pos = player.position + new Vector3(random.x,0f,random.y);
		GameObject zombie = Instantiate(enemyPrefab,pos,Quaternion.identity);
		zombie.GetComponent<ZombieController>().SetPlayer(player);
	}
}
