// using Godot;
// using Godot.Collections;
// using System;
// using System.Collections.Generic;

// public partial class Grid : Node
// {
//     LinkedList<LinkedList<TileDefinition>> AllGridTiles;

//     public override void _Ready()
//     {
//         AllGridTiles = new LinkedList<LinkedList<TileDefinition>>();
//     }

//     public void AddStartingLevel(TileMapLayer map)
//     {
//         var allCells = map.GetUsedCells();

//         //iterate through all cells
//         for (int index = 0; index < allCells.Count; index++)
//         {
//             var currentCellCoord = allCells[index];
//             var currentTile = map.GetCellTileData(currentCellCoord);

//             //add a TileDefinition in the ordered linkedlists
//             var newTile = new TileDefinition();
//             InsertTile();
//         }
//         // int sourceId = map.GetCellSourceId(coords);
//         // Vector2i atlasCoords = tileMapLayer.GetCellAtlasCoords(coords);
//         // int alternativeId = tileMapLayer.GetCellAlternative(coords);
//     }

//     void InsertTile(TileDefinition newTile)
//     {
        
//     }

//     public int GetWidth()
//     {
//         return AllGridTiles.Count;
//     }

//     public int GetHeight()
//     {
//         return AllGridTiles[0].Count;
//     }

//     public TileSetSource GetTile(int x, int y)
//     {
//         return AllGridTiles[x][y];
//     }

//     public void ExpandOneLevel(int x, int y)
//     {
//         //expand the grid with empty cells

//         //iterate through all the empty cells

//         //detect the cells adjacent to the current cell

//         //determine which tile the current should be

//         //determine what the adjacent tiles should be
//     }
// }

