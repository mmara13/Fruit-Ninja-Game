using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private Collider spawnArea;

    public GameObject[] fruitPrefabs;

    public float minSpawnDelay = 0.25f; //1/4 sec
    public float maxSpawnDelay; //1s for spawining

    //angle at which the fruits are shot
    public float minAngle = -15f;
    public float maxAngle = 15f;

    //how much strength is there when the prog lang. are launched
    public float minForce = 18f;
    public float maxForce = 22f;

    public float maxLifetime = 5f; //alive for at most 5s before it s despawned
    //enough time so that it will be sliced or fallen off the screen

    private void Awake()
    {
        spawnArea = GetComponent<Collider>();
    }

    private void OnEnable()
    {
       StartCoroutine(Spawn());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator Spawn()
    {
        //initial delay for cases like transitioning between game over and new game so that the game wont start right after
        yield return new WaitForSeconds(2f); //2s delay before spwaning things


        while (enabled)
        {
            //fruit respawn
            GameObject prefab = fruitPrefabs[Random.Range(0, fruitPrefabs.Length)];

            Vector3 position = new Vector3();
            position.x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
            position.y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
            position.z = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z);

            //rotation - angle of the prog langs
            Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(minAngle, maxAngle));

            //creating(spawning) the p lang
            GameObject frufru = Instantiate(prefab, position, rotation);

            //destroying it after the maxlifetime was reached
            Destroy(frufru, maxLifetime);

            //add a force to the body of the prefabs(prog languages)
            float force = Random.Range(minForce, maxForce);

            //prog.transform.up - the direction of the programming lang
            frufru.GetComponent<Rigidbody>().AddForce(frufru.transform.up * force, ForceMode.Impulse);

            //wait a random number between the values selected for spawning before starting to loop again
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
        }
    }
}


