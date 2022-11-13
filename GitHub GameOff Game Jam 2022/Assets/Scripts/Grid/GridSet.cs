using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


//<summary> Generates a grid of the specified rows and columns. Creates a new one everytime the script is loaded.</summary>
[ExecuteInEditMode]

public class GridSet : MonoBehaviour 
{
     [SerializeField]
    private int width = 2;
    [SerializeField]
    private int height = 2;
    
    [SerializeField]
    private GameObject tile;
    void OnEnable()
    {
        float spriteWidth = tile.GetComponent<SpriteRenderer>().size.x;
        float spriteHeight = tile.GetComponent<SpriteRenderer>().size.y;
        for (int rows = 0; rows < width; rows++){
            for(int columns = 0; columns< height; columns++){
                if(tile!= null){
                    if(rows % 2 == 0 ){
                        GameObject spawned = Instantiate(tile, new Vector3(transform.position.x+(rows*1.5f*spriteWidth), transform.position.y-(columns*1.85f*spriteHeight),0f), Quaternion.identity, transform);
                        int currentRow = (columns * 2) + 1 ;
                       spawned.GetComponent<Tile>().SetSpriteOrderLayer(currentRow);
                    } else {
                        GameObject spawned = Instantiate(tile, new Vector3(transform.position.x+(rows*1.5f*spriteWidth), transform.position.y+.9f-(columns*1.85f*spriteHeight),0f), Quaternion.identity, transform);
                        int currentRow = columns * 2;
                        spawned.GetComponent<Tile>().SetSpriteOrderLayer(currentRow);
                    }
                    
                }
                
            }
        }
    }

}
