using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController INSTANCE;

    [Header("UI Screen")]
    public GameoverScreen GameOverScreen;
    public InGameScreen InGameScreen;
    public int PlayerScore = 0;

    [Header("Material")]
    public Material SkyboxMaterial;
    public float skyboxColorChangeSec = 5;
    public Color[] skyboxColor;
    public Material[] ObstacleMaterials = new Material[2];

    [Header("Always Present in Scene")]
    public CameraController CameraController;
    public Vector3 CameraStartPoint = new Vector3(0, 3, -5);

    [Header("Prefabs and Pools")]
    public Player PlayerPrefab;
    public SimplePool ChunkPool;
    public SimplePool ObstaclePool;
    public SimplePool OrbPool;
    public int spawnOffset = 100;
    public int worldZLimit = 1000;

    [Header("Random")]
    public int orbSpawnPercentage = 5;
    public int holeSpawnPercentage = 5;
    public int obstacleSpawnPercentage = 30;
    public int orbMinSpace = 5;

    private int zSpawnPosition = 0;
    private int lastOrbZPosition = 0;
    private Player playerInstanciated;

    private int playerColorIndex;
    private int skyboxColorIndex;
    private string highScoreKey = "highScore";

    private bool stopCoroutine = false;

    private void Awake()
    {
        INSTANCE = this;
        this.InGameScreen.gameObject.SetActive(false);
        this.GameOverScreen.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        this.skyboxColorIndex = Random.Range(0, this.skyboxColor.Length);
        this.SkyboxMaterial.color = this.skyboxColor[this.skyboxColorIndex];
        StartCoroutine(this.UpdateSkyBoxColor());
    }

    private void OnDisable()
    {
        this.stopCoroutine = true;
    }

    private IEnumerator UpdateSkyBoxColor()
    {
        float time = 1;
        Color currentColor = Color.white;
        Color nextColor = Color.white;
        while (!this.stopCoroutine)
        {
            if (time >= 1)
            {
                time = 0;
                this.skyboxColorIndex++;
                if (this.skyboxColorIndex >= this.skyboxColor.Length)
                {
                    this.skyboxColorIndex = 0;
                }
                currentColor = this.SkyboxMaterial.color;
                nextColor = this.skyboxColor[this.skyboxColorIndex];
            }

            time += Time.deltaTime / this.skyboxColorChangeSec;
            this.SkyboxMaterial.color = Color.Lerp(currentColor, nextColor, time);
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.playerInstanciated != null)
        {
            if (this.playerInstanciated.transform.position.z + this.spawnOffset > this.zSpawnPosition)
            {
                Chunk chunk = this.InstanciateChunk();
                if(chunk != null && this.zSpawnPosition > 20)
                {
                    this.PlaceItemOnChunk(chunk);
                }
            }
            if(this.playerInstanciated.transform.position.z > worldZLimit)
            {
                this.ResetWorldPosition();
            }
        }
    }

    public void ChangeColor()
    {
        if(this.playerInstanciated != null)
        {
            this.playerColorIndex++;
            if(this.playerColorIndex >= this.ObstacleMaterials.Length)
            {
                this.playerColorIndex = 0;
            }
            this.ApplyColor(this.ObstacleMaterials[this.playerColorIndex]);
        }
    }

    private void ApplyColor(Material material)
    {
        Color color = material.GetColor("_CellColor");
        this.playerInstanciated.SetColor(color);
    }

    public void AddScore(int scoreValue)
    {
        this.PlayerScore += scoreValue;
        this.InGameScreen.ScoreText.text = this.PlayerScore.ToString();
        if(this.PlayerScore / 10 >= this.playerInstanciated.Tails.Count)
        {
            this.playerInstanciated.AddTail();
            this.CameraController.UpdateOffset(Vector3.forward * -1);
        }
    }

    public void StartGame()
    {
        this.ResetWorld();
        this.PlayerScore = 0;
        this.InGameScreen.gameObject.SetActive(true);
        this.playerInstanciated = GameObject.Instantiate(this.PlayerPrefab, new Vector3(0, 0.5f, 0), this.PlayerPrefab.transform.rotation);

        this.CameraController.TrackPlayer(this.playerInstanciated, this.CameraStartPoint);
        this.ApplyColor(this.ObstacleMaterials[this.playerColorIndex]);
        this.AddScore(0);

    }

    public void StopGame()
    {
        this.InGameScreen.gameObject.SetActive(false);
        this.GameOverScreen.gameObject.SetActive(true);
        int highScore = PlayerPrefs.GetInt(this.highScoreKey, 0);
        if(this.PlayerScore > highScore)
        {
            PlayerPrefs.SetInt(this.highScoreKey, this.PlayerScore);
            highScore = this.PlayerScore;
        }
        this.GameOverScreen.SetScore(this.PlayerScore, highScore);
    }

    #region Reset

    public void ResetWorld()
    {
        this.ChunkPool.ResetPool();
        this.ObstaclePool.ResetPool();
        this.playerColorIndex = 0;
        this.zSpawnPosition = 0;
        this.lastOrbZPosition = 0;
    }

    private void ResetWorldPosition()
    {
        float offset = (Vector3.zero - this.playerInstanciated.transform.position).z;
        this.playerInstanciated.transform.position += Vector3.forward * offset;
        this.CameraController.transform.position += Vector3.forward * offset;
        foreach(GameObject gameObject in this.ChunkPool.PooledObjects)
        {
            if (gameObject.activeInHierarchy)
            {
                gameObject.transform.position += Vector3.forward * offset;
            }
        }
        foreach (GameObject gameObject in this.ObstaclePool.PooledObjects)
        {
            if (gameObject.activeInHierarchy)
            {
                gameObject.transform.position += Vector3.forward * offset;
            }
        }
        this.zSpawnPosition += (int)offset;
    }

    #endregion

    #region Placing

    private void PlaceItemOnChunk(Chunk chunk)
    {
        bool trapPlaced;
        bool orbPlaced = this.zSpawnPosition - this.lastOrbZPosition < this.orbMinSpace;
        for (int floorIdx = 0; floorIdx < chunk.FloorTiles.Length; floorIdx++)
        {
            int trapValue = Random.Range(0, 100);
            trapPlaced = trapValue < this.holeSpawnPercentage || trapValue < this.obstacleSpawnPercentage;
            if(trapValue < this.holeSpawnPercentage)
            {
                chunk.PlaceHole(floorIdx);
            }
            else if (trapValue < this.obstacleSpawnPercentage)
            {
                Obstacle obstacle = this.InstanciateObstacle(Random.Range(0, this.ObstacleMaterials.Length));
                chunk.PlaceObstacle(obstacle, floorIdx);
            }

            if(!orbPlaced && !trapPlaced && Random.Range(0, 100) < this.orbSpawnPercentage)
            {
                Orb orb = this.InstanciateOrb();
                chunk.PlaceOrb(orb, floorIdx);
                this.lastOrbZPosition = this.zSpawnPosition;
                orbPlaced = true;
            }
        }
    }

    private Orb InstanciateOrb()
    {
        Orb orb = null;
        GameObject orbGameObject = this.OrbPool.GetPooledObject();
        if(orbGameObject != null)
        {
            orb = orbGameObject.GetComponent<Orb>();
            orbGameObject.SetActive(true);
        }
        return orb;
    }

    private Obstacle InstanciateObstacle(int materialIdx)
    {
        Obstacle obstacle = null;
        GameObject obstacleGameObject = this.ObstaclePool.GetPooledObject();
        if(obstacleGameObject != null)
        {
            obstacle = obstacleGameObject.GetComponent<Obstacle>();
            obstacle.AssignMaterial(this.ObstacleMaterials[materialIdx]);
            obstacleGameObject.SetActive(true);
        }
        return obstacle;
    }

    private Chunk InstanciateChunk()
    {
        Chunk chunk = null;
        GameObject chunkGameObject = this.ChunkPool.GetPooledObject();
        if (chunkGameObject != null)
        {
            chunk = chunkGameObject.GetComponent<Chunk>();
            chunk.transform.position = Vector3.forward * this.zSpawnPosition;
            chunkGameObject.SetActive(true);
            this.zSpawnPosition++;
        }
        return chunk;
    }

    #endregion
}
