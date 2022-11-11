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

        GUILayout.Label("Tile Selection", EditorStyles.label);
        GUILayout.BeginVertical();
        EditorGUILayout.LabelField("Tile Type", GUILayout.MaxWidth(125));
        if (GUILayout.Button("Forest"))
        {
            foreach (GameObject i in Selection.gameObjects)
            {
                if (forest == null)
                {
                    Debug.LogError("Forest tile asset null");
                }
                i.gameObject.GetComponent<Tile>().updateAppearance(forest);
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
                i.gameObject.GetComponent<Tile>().updateAppearance(lake);
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
                i.gameObject.GetComponent<Tile>().updateAppearance(mountains);
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
                i.gameObject.GetComponent<Tile>().updateAppearance(plain);
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
                i.gameObject.GetComponent<Tile>().updateAppearance(hills);
            }
        }
        GUILayout.EndVertical();
    }
}
