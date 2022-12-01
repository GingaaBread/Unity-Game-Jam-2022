#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

//<summary> Editor tool that allows for changing the tiles easily through a window </summary>

[ExecuteInEditMode]
public class MapManager : EditorWindow
{
    [SerializeField]
    TileTypeSO plain;
    TileTypeSO mountains;
    TileTypeSO lake;
    TileTypeSO forest;
    TileTypeSO hills;

    GameObject tile;
    int rows = 0;
    int columns = 0;
    Vector2 mapSpawnOrigin;
    Transform parentObject;

    [MenuItem("Window/Map Manager")]
    public static void ShowWindow()
    {
        GetWindow<MapManager>("Map Manager");
    }

    public void OnGUI()
    {
        plain = (TileTypeSO)AssetDatabase.LoadAssetAtPath("Assets/Scripts/Grid/TileTypes/Plains.asset", typeof(TileTypeSO));
        mountains = (TileTypeSO)AssetDatabase.LoadAssetAtPath("Assets/Scripts/Grid/TileTypes/Mountains.asset", typeof(TileTypeSO));
        lake = (TileTypeSO)AssetDatabase.LoadAssetAtPath("Assets/Scripts/Grid/TileTypes/Lake.asset", typeof(TileTypeSO));
        forest = (TileTypeSO)AssetDatabase.LoadAssetAtPath("Assets/Scripts/Grid/TileTypes/Forest.asset", typeof(TileTypeSO));
        hills = (TileTypeSO)AssetDatabase.LoadAssetAtPath("Assets/Scripts/Grid/TileTypes/Hills.asset", typeof(TileTypeSO));
        // tile =  PrefabUtility.LoadPrefabContents("Assets/Prefabs/Map/Tile.prefab");
        GUILayout.Label("Tile Selection", EditorStyles.boldLabel);
        EditorGUILayout.Space(30);
        GUILayout.BeginVertical();
        EditorGUILayout.LabelField("Tile Type",EditorStyles.boldLabel, GUILayout.MaxWidth(125));
        if (GUILayout.Button("Forest"))
        {
            foreach (GameObject i in Selection.gameObjects)
            {
                if (forest == null)
                {
                    Debug.LogError("Forest tile asset null");
                }
                i.gameObject.GetComponent<Tile>().UpdateEditorTileAppearance(forest);
            }
        }
        if (GUILayout.Button("Lake"))
        {
            foreach (GameObject i in Selection.gameObjects)
            {
                if ( lake == null)
                {
                    Debug.LogError("Lake tile asset null");
                }
                i.gameObject.GetComponent<Tile>().UpdateEditorTileAppearance(lake);
            }
        }
        if (GUILayout.Button("Mountains"))
        {
            foreach (GameObject i in Selection.gameObjects)
            {
                if (mountains == null)
                {
                    Debug.LogError("Mountain tile asset null");
                }
                i.gameObject.GetComponent<Tile>().UpdateEditorTileAppearance(mountains);
            }
        }
        if (GUILayout.Button("Plains"))
        {
            foreach (GameObject i in Selection.gameObjects)
            {
                if (plain == null)
                {
                    Debug.LogError("Plain tile asset null");
                }
                i.gameObject.GetComponent<Tile>().UpdateEditorTileAppearance(plain);
            }
        }

        if (GUILayout.Button("Hills"))
        {
            foreach (GameObject i in Selection.gameObjects)
            {
                if (hills == null)
                {
                    Debug.LogError("Hills tile asset null");
                }
                i.gameObject.GetComponent<Tile>().UpdateEditorTileAppearance(hills);
            }
        }

        GUILayout.Space(30);
        EditorGUILayout.LabelField("Generate Map",EditorStyles.boldLabel ,GUILayout.MaxWidth(125));
        rows = EditorGUILayout.IntField("Rows", rows);
        columns = EditorGUILayout.IntField("Columns", columns);
        mapSpawnOrigin = EditorGUILayout.Vector2Field("Top Left Corner", mapSpawnOrigin);
        tile = EditorGUILayout.ObjectField("Tile", tile, typeof(GameObject), true) as GameObject;
        parentObject = EditorGUILayout.ObjectField("Parent Object", parentObject, typeof(Transform), true) as Transform;
        if(GUILayout.Button("Generate Map"))
        {
            GenerateMap(rows, columns);
        }
        if(GUILayout.Button("Delete Map"))
        {
            if(parentObject!= null){
                Transform[] children = parentObject.GetComponentsInChildren<Transform>();
               for(int i = 1 ; i < children.Length; i++){ // index set at 1 to not delete parent transform
                    DestroyImmediate(children[i].gameObject);
                }
            }
        }
        GUILayout.EndVertical();
    }

    private void GenerateMap(int rows, int columns){
         float spriteWidth = tile.GetComponent<SpriteRenderer>().size.x;
        float spriteHeight = tile.GetComponent<SpriteRenderer>().size.y;
        for (int width = 0; width < columns; width++){
            for(int height= 0; height < rows; height++){
                if(tile!= null){
                    if(width % 2 == 0 ){
                        GameObject spawned = Instantiate(tile, new Vector3( mapSpawnOrigin.x+(width*1.5f*spriteWidth),  mapSpawnOrigin.y-(height*1.85f*spriteHeight),0f), Quaternion.identity, parentObject);
                        int currentRow = (height * 2) + 1 ;
                       spawned.GetComponent<Tile>().SetSpriteOrderLayer(currentRow);
                    } else {
                        GameObject spawned = Instantiate(tile, new Vector3( mapSpawnOrigin.x+(width*1.5f*spriteWidth),  mapSpawnOrigin.y+.9f-(height*1.85f*spriteHeight),0f), Quaternion.identity, parentObject);
                        int currentRow = height * 2;
                        spawned.GetComponent<Tile>().SetSpriteOrderLayer(currentRow);
                    }
                    
                }
                
            }
        }
    }
}
#endif
