using UnityEngine;

public class DrawAvailableMoves : MonoBehaviour
{

    [Header("Character settings")]
    public int movesPerTurn = 3;

    [Header("Pass game objects")]
    public float tileSize = 2f;
    public GameObject availableTile;
    public GameObject unAvailableTile;

    GameObject firstTile;

    private void OnMouseOver()
    {
        
        if (Input.GetMouseButtonUp(0) && firstTile==null )
        {   
            CleanTiles();
            DrawTiles();
            
        }
    }

    // pokazuje pola na które moze iść dana postać. 
    public void DrawTiles()
    {
        DrawFirstTile();
        for (int i = 0; i < movesPerTurn; i++)
        {
            SpawnNearbyTiles();
        }
    }

    
    // czyści pola wyświetlone dla innych postaci
    public void CleanTiles()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        foreach (GameObject tile in tiles)
        {
            Destroy(tile);
        }
    }

    //tworzy pierwsze pole pod nogami postaci
    public GameObject GetFirstTile()
    {
        return firstTile;
    }

    // tworzy pola w mijschach gdzie mozna stanac i ktore sasiaduja z dostepnymi juz obszarami
    void SpawnNearbyTiles()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        foreach (GameObject tile in tiles)
        {
            TileProperties tp = tile.GetComponent<TileProperties>();          
            if (string.Equals(this.transform.name, tp.getNameOfCharacterToBeMoved()))
            {
                //tworzy tile przed zadanym obszarem
                SpawnStepableTileInGivenOffset(2f, 0f, tile);
                // tworzy tile za obszarem
                SpawnStepableTileInGivenOffset(-2f, 0f, tile);
                // na lewo 
                SpawnStepableTileInGivenOffset(0f, 2f, tile);
                //na prawo
                SpawnStepableTileInGivenOffset(0f, -2f, tile);
            }             
        }
    }


    void SpawnStepableTileInGivenOffset(float xOffset, float zOffset, GameObject tile)
    {
        RaycastHit hit;
        Vector3 rayOrigin = new Vector3(tile.transform.position.x + xOffset, tile.transform.position.y + 10f, tile.transform.position.z + zOffset);
        Vector3 rayTarget = new Vector3(tile.transform.position.x + xOffset, tile.transform.position.y + 20f, tile.transform.position.z + zOffset);
        Vector3 rayDirection = rayOrigin - rayTarget;
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, 50f))
        {
            if (string.Equals(hit.transform.tag, "Stepable") || string.Equals(hit.transform.tag, "Tile"))
            {
                if (string.Equals(hit.transform.tag, "Tile"))
                {
                    Destroy(hit.transform.gameObject);
                }
                Vector3 newTilePosition = new Vector3(tile.transform.position.x + xOffset, tile.transform.position.y , tile.transform.position.z + zOffset);
                GameObject newTile = Instantiate(availableTile, newTilePosition, Quaternion.identity) as GameObject;
                TileProperties propertiesOfNewTile  =  newTile.GetComponent<TileProperties>();
                propertiesOfNewTile.setCharacterToBeMoved(this.transform.gameObject);
                newTile.transform.parent = transform;
            }
            
        }
    }
    private void DrawFirstTile()
    {
        Vector3 tilePosition = new Vector3(transform.position.x, 0.1f, transform.position.z);
        firstTile = Instantiate(availableTile, tilePosition, Quaternion.identity) as GameObject;
        TileProperties propertiesOfNewTile = firstTile.GetComponent<TileProperties>();
        propertiesOfNewTile.setCharacterToBeMoved(this.transform.gameObject);
        firstTile.transform.parent = transform;
    }
}
