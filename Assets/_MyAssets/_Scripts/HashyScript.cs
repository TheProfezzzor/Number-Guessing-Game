using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HashyScript : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    [SerializeField] Transform positionA;  // The first position.
    [SerializeField] Transform positionB;  // The second position.
    [SerializeField] float speed = 1f;   // Speed of the bounce.
    [SerializeField] Image diffuse; // Reference to child Image.
    [SerializeField] Image shadow; // Reference to child Image.
    float timer = 0f;  // Timer to track the sine wave progress.
    Vector3 worldPositionA;
    Vector3 worldPositionB;

    private void Start()
    {
        worldPositionA = positionA.position; // This converts the local position of the children into a world position.
        worldPositionB = positionB.position;
    }

    void Update()
    {
        // Increment timer based on speed and deltaTime.
        timer += Time.deltaTime * speed;

        // Calculate the current position using a sine wave.
        // Mathf.Sin oscillates between -1 and 1, but we want 0 to 1, so we map it using (sin(t) + 1) / 2.
        float sineValue = (Mathf.Sin(timer) + 1f) / 2f;

        // Interpolate between positionA and positionB using the sine value.
        transform.position = Vector3.Lerp(worldPositionA, worldPositionB, sineValue);
    }

    public void SetSprite(int index)
    {
        diffuse.sprite = sprites[index];
        shadow.sprite = sprites[index];
    }
}
