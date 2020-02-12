using UnityEngine;

public class Respawner : MonoBehaviour
{
    [Tooltip("Set the Player Controller here")]
    [SerializeField] private Transform player;
    [Tooltip("Set the Respawn GameObject here")]
    [SerializeField] private Transform respawnPoint;

    /// <summary>
    /// If the player triggers/hits the colliders of
    /// the GameObject, it will send the player back to the original starting
    /// position. Player does not have any health at this point.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        //getting the players transform position and setting it to the 
        //transform position point for respawning.
        player.transform.position = respawnPoint.transform.position;
    }
}
