using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public FloorTile[] FloorTiles = new FloorTile[5];
    public List<GameObject> placedGameObjects = new List<GameObject>(5);

    private Vector3 viewportPosition;

    private void Update()
    {
        this.viewportPosition = Camera.main.WorldToViewportPoint(this.transform.position);
        if(this.viewportPosition.z < -10)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        this.ClearChunkContent();
    }

    public void ClearChunkContent()
    {
        foreach(GameObject gameObject in this.placedGameObjects)
        {
            if (gameObject != null)
            {
                gameObject.SetActive(false);
            }
        }
        foreach (FloorTile floorTile in this.FloorTiles)
        {
            floorTile.SetAsHole(false);
        }
        this.placedGameObjects.Clear();
    }

    public void PlaceHole(int idx)
    {
        this.FloorTiles[idx].SetAsHole(true);
    }

    public void PlaceGameobject(GameObject gameObjectToPlace, int floorId)
    {
        gameObjectToPlace.transform.position = this.FloorTiles[floorId].transform.position;
        this.placedGameObjects.Add(gameObjectToPlace);
    }
}
