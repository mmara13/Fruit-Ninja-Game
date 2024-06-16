using UnityEngine;

public class Blade : MonoBehaviour
{
    public static Blade Instance { get; private set; } 

    public float sliceForce = 5f; // Force applied to the sliced objects
    public float minSliceVelocity = 0.01f; // Minimum velocity to start slicing

    private Camera mainCamera;
    private Collider sliceCollider;
    private TrailRenderer sliceTrail;

    private Vector3 direction;
    public Vector3 Direction => direction;

    private bool slicing;
    public bool Slicing => slicing; 

    private void Awake() // Singleton pattern
    {
        if (Instance != null && Instance != this) // If there is already an instance of Blade, destroy this one
        {
            Destroy(gameObject);  
            return;
        }
        Instance = this; // Set the instance to this object
        
        mainCamera = Camera.main;
        sliceCollider = GetComponent<Collider>();
        sliceTrail = GetComponentInChildren<TrailRenderer>();

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        StopSlice();
    }

    private void OnDisable()
    {
        StopSlice();
    }

    private void Update() // Check for input
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (mainCamera != null)
        {
            if (Input.GetMouseButtonDown(0)) // If the left mouse button is pressed
            {
                StartSlice();
            }
            else if (Input.GetMouseButtonUp(0)) // If the left mouse button is released
            {
                StopSlice();
            }
            else if (slicing) // If the left mouse button is held down
            {
                ContinueSlice();
            }
        }
    }


    private void StartSlice() // Start slicing
    {
        Vector3 position = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        position.z = 0f;
        transform.position = position;

        slicing = true;
        sliceCollider.enabled = true;
        sliceTrail.enabled = true;
        sliceTrail.Clear();
    }

    private void StopSlice() // Stop slicing
    {
        slicing = false;

        if (sliceCollider != null)
            sliceCollider.enabled = false;
        
        if (sliceTrail != null)
        {
            sliceTrail.enabled = false;
            sliceTrail.Clear();
        }
    }


    private void ContinueSlice() // Continue slicing
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null) return; 
        }

        Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition); // Get the new position of the mouse
        newPosition.z = 0f; // Set the z position to 0
        direction = newPosition - transform.position; // Calculate the direction of the slice

        float velocity = direction.magnitude / Time.deltaTime; // Calculate the velocity of the slice
        sliceCollider.enabled = velocity > minSliceVelocity; // Enable the collider if the velocity is greater than the minimum slice velocity

        transform.position = newPosition;
    }

    public void SetTrailColor(Color color) // Set the color of the slice trail
    {
        sliceTrail.startColor = color;
        sliceTrail.endColor = color;
    }
}