using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunksManager : MonoBehaviour
{
    [SerializeField] private GameObject chunkPrefab;
    [SerializeField] private int chunkSize = 15;
    [SerializeField] private int loadDistance = 3;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private DifficultyManager difficultyManager;

    public float ChunkSize => chunkSize;

    private Transform[] chunksGameObjects;
    private Vector3 prevPlayerPosition;

    private void Start()
    {
        chunksGameObjects = new Transform[(loadDistance * 2 + 1) * (loadDistance * 2 + 1)];
        prevPlayerPosition = playerTransform.position;

        SpawnInitialChunks();
    }

    private void Update()
    {

        if (playerTransform.position != prevPlayerPosition)
        {
            UpdateChunks();
            prevPlayerPosition = playerTransform.position;
        }
    }

    private void SpawnInitialChunks()
    {
        int i = 0;
        for (int xOffset = -loadDistance; xOffset <= loadDistance; xOffset++)
        {
            for (int yOffset = -loadDistance; yOffset <= loadDistance; yOffset++)
            {
                Vector2 chunkCoordinates = new Vector2(xOffset, yOffset);

                GameObject chunkGameObject = Instantiate(chunkPrefab, chunkCoordinates * chunkSize, Quaternion.identity, transform);
                chunkGameObject.GetComponent<Chunk>().resourceManager = resourceManager;
                chunkGameObject.GetComponent<Chunk>().difficultyManager = difficultyManager;

                chunksGameObjects[i] = chunkGameObject.transform;
                i++;
            }
        }
    }

    private void UpdateChunks()
    {
        foreach (Transform chunk in chunksGameObjects)
        {
            Vector2 distanceFromPlayer = chunk.position - playerTransform.position;

            if (Mathf.Abs(distanceFromPlayer.x) > chunkSize * (loadDistance + 1))
            {
                if (playerTransform.position.x < prevPlayerPosition.x)
                {
                    chunk.transform.position -= new Vector3((loadDistance * 2 + 1) * chunkSize, 0f, 0f);
                }
                else if (playerTransform.position.x > prevPlayerPosition.x)
                {
                    chunk.transform.position += new Vector3((loadDistance * 2 + 1) * chunkSize, 0f, 0f);
                }
            }
            else if (Mathf.Abs(distanceFromPlayer.y) > chunkSize * (loadDistance + 1))
            {
                if (playerTransform.position.y < prevPlayerPosition.y)
                {
                    chunk.transform.position -= new Vector3(0f, (loadDistance * 2 + 1) * chunkSize, 0f);
                }
                else if (playerTransform.position.y > prevPlayerPosition.y)
                {
                    chunk.transform.position += new Vector3(0f, (loadDistance * 2 + 1) * chunkSize, 0f);
                }
            }
        }
    }
}
