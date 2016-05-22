using UnityEngine;
using UnityEditor;
using MineSweeper.Game;

namespace MineSweeper.Editor{
    public class Tools : ScriptableObject
    {
    
        [MenuItem("Tools/Spawn Cells")]
        static void SpawnCells()
        {
            GameObject obj = Resources.Load(Board.CEL_PLACE) as GameObject;
            GameObject parent = GameObject.Find(Board.BOARD);
            
            for (int i = 0; i < Board.GRID_WIDTH; i++)
            {
                for (int j = 0; j < Board.GRID_HEIGHT; j++)
                {
                    GameObject temp = GameObject.Instantiate(obj, new Vector3(40 + 80 * i, -40 + -80*j, 0), Quaternion.identity) as GameObject;
                    temp.name = Board.CEL + (i + j*Board.GRID_WIDTH);
                    temp.transform.SetParent(parent.transform, false);
                }
            }
        }
    }
}
