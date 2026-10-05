using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    private bool isPickedUp = false;
    private Transform target;
    private float speed = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (isPickedUp) return; // Dennis
        EXPHandler handler = other.GetComponentInParent<EXPHandler>(); // Dennis
        if (handler != null) // Dennis
        {
            Debug.Log("Player entered pickup range");
            isPickedUp = true;
            target = handler.transform; // Dennis
        }
    }

    private void Update()
    {
        if(!isPickedUp) return;

        // Move the pickup towards the player
        float step = speed * Time.deltaTime; // Adjust speed as needed
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        speed += Time.deltaTime * 2; // Increase speed over time

        // Check if the pickup has reached the player
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            PickupAction(target.gameObject);
            Destroy(gameObject); // Destroy the pickup after it's collected
        }
    }

    protected abstract void PickupAction(GameObject player);
}
