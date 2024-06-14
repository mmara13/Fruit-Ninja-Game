using UnityEngine;

public class Fruit : MonoBehaviour
{
    public GameObject whole;
    public GameObject sliced;

    private Rigidbody fruitRigidbody;
    private Collider fruitCollider;
    private ParticleSystem juicyTime; //juice particles

    public int points = 1;

    AudioManager audioManager;


    private void Awake()
    {
        fruitRigidbody = GetComponent<Rigidbody>();
        fruitCollider = GetComponent<Collider>();
        juicyTime = GetComponentInChildren<ParticleSystem>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Slice(Vector3 direction, Vector3 position, float force)
    {
        FindObjectOfType<GameManager>().IncreaseScore(points);

        whole.SetActive(false);

        audioManager.PlaySFX(audioManager.slice);

        sliced.SetActive(true);

        
        fruitCollider.enabled = false;
        juicyTime.Play();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        sliced.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Rigidbody[] slices = sliced.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody slice in slices)
        {
            slice.velocity = fruitRigidbody.velocity;
            slice.AddForceAtPosition(direction * force, position, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Blade blade = other.GetComponent<Blade>();
            Slice(blade.Direction, blade.transform.position, blade.sliceForce);
        }
    }

}