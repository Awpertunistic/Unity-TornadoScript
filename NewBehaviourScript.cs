using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject squarePrefab;    // The prefab of the little spinning square
    public GameObject trailPrefab;     // The prefab of the trail object
    public float spawnRate = 1.0f;     // How often to spawn a new square (in seconds)
    public float spinSpeed = 180.0f;   // The speed at which the squares will spin (in degrees per second)
    public float maxTornadoHeight = 10.0f;  // The maximum height of the tornado
    public float trailDuration = 1.0f; // The duration of the trail effect (in seconds)
    public float upwardForce = 15.0f;  // The upward force applied to the squares

    private float timeSinceLastSpawn = 0.0f;

    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnRate)
        {
            SpawnSquare();
            timeSinceLastSpawn = 0.0f;
        }
    }

    private void SpawnSquare()
    {
        float spawnRadius = Random.Range(0.0f, maxTornadoHeight); // Randomize the starting radius

        // Calculate the initial position using polar coordinates
        float angle = Random.Range(0.0f, 360.0f);
        Vector3 spawnPosition = new Vector3(spawnRadius * Mathf.Cos(angle * Mathf.Deg2Rad), 0.0f, spawnRadius * Mathf.Sin(angle * Mathf.Deg2Rad));

        GameObject newSquare = Instantiate(squarePrefab, transform.position + spawnPosition, Quaternion.identity);
        newSquare.transform.SetParent(transform);

        // Apply spinning rotation
        newSquare.GetComponent<Rigidbody>().angularVelocity = new Vector3(0.0f, spinSpeed, 0.0f);

        // Add the Trail Renderer component to the square
        TrailRenderer squareTrail = newSquare.GetComponentInChildren<TrailRenderer>();
        if (squareTrail == null)
        {
            squareTrail = Instantiate(trailPrefab, newSquare.transform).GetComponent<TrailRenderer>();
            squareTrail.material = new Material(Shader.Find("Standard"));
            squareTrail.material.color = Color.white;
        }

        // Set the trail duration and start the trail effect
        squareTrail.time = trailDuration;
        squareTrail.enabled = true;

        // Destroy the square and the trail once it reaches the top of the tornado
        float destroyHeight = maxTornadoHeight + spawnPosition.y;
        Destroy(newSquare, destroyHeight / upwardForce);
        Destroy(squareTrail.gameObject, destroyHeight / upwardForce);

        // Start the movement coroutine
        StartCoroutine(MoveSquareUpward(newSquare, destroyHeight, spawnRadius));
    }

    private System.Collections.IEnumerator MoveSquareUpward(GameObject square, float destroyHeight, float startRadius)
    {
        float elapsedTime = 0.0f;

        while (square != null && square.transform.position.y < destroyHeight)
        {
            // Increase the radius of the circular motion based on time
            float radius = Mathf.Lerp(startRadius, destroyHeight, elapsedTime / trailDuration);

            // Calculate the position using polar coordinates
            float angle = elapsedTime * 180.0f; // Adjust the angle for speed
            Vector3 newPosition = new Vector3(radius * Mathf.Cos(angle * Mathf.Deg2Rad), square.transform.position.y + upwardForce * Time.deltaTime, radius * Mathf.Sin(angle * Mathf.Deg2Rad));

            square.transform.position = transform.position + newPosition;

            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }
}