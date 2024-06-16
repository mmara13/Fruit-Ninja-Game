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
        fruitRigidbody = GetComponent<Rigidbody>(); // Get the rigidbody component
        fruitCollider = GetComponent<Collider>(); // Get the collider component
        juicyTime = GetComponentInChildren<ParticleSystem>(); // Get the juice particles
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>(); // Find the audio manager
    }

    private void Slice(Vector3 direction, Vector3 position, float force)    // Slice the fruit
    {
        FindObjectOfType<GameManager>().IncreaseScore(points); // Increase the score

        whole.SetActive(false); // Disable the whole fruit

        audioManager.PlaySFX(audioManager.slice); // Play the slice sound

        sliced.SetActive(true); // Enable the sliced fruit

        
        fruitCollider.enabled = false; // Disable the collider
        juicyTime.Play(); // Play the juice particles
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Calculate the angle of the slice
        sliced.transform.rotation = Quaternion.Euler(0f, 0f, angle); // Rotate the sliced fruit

        Rigidbody[] slices = sliced.GetComponentsInChildren<Rigidbody>(); // Get the rigidbodies of the slices

        foreach (Rigidbody slice in slices)
        {
            slice.velocity = fruitRigidbody.velocity; // Set the velocity of the slice
            slice.AddForceAtPosition(direction * force, position, ForceMode.Impulse); // Add a force to the slice
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // If the fruit collides with the player
        
        {
            Blade blade = other.GetComponent<Blade>(); // Get the blade component
            Slice(blade.Direction, blade.transform.position, blade.sliceForce); // Slice the fruit
        }
    }

}