using Godot;
using System;
using System.Collections.Generic;

public partial class Grid : Node
{
    List<List<TileSetSource>> AllGridTiles;
    Dictionary<Vector2I, TileData> 

    public override void _Ready()
    {
        AllGridTiles = new List<List<TileSetSource>>(1000);
    }

    public void AddStartingLevel(TileMapLayer map)
    {
        var allCells = map.GetUsedCells();

        for (int index = 0; index < allCells.Count; index++)
        {
            var currentCellCoord = allCells[index];
            var currentTile = map.GetCellTileData(currentCellCoord);
            currentTile.
        }
        int sourceId = map.GetCellSourceId(coords);
        Vector2i atlasCoords = tileMapLayer.GetCellAtlasCoords(coords);
        int alternativeId = tileMapLayer.GetCellAlternative(coords);
    }

    public int GetWidth()
    {
        return AllGridTiles.Count;
    }

    public int GetHeight()
    {
        return AllGridTiles[0].Count;
    }

    public TileSetSource GetTile(int x, int y)
    {
        return AllGridTiles[x][y];
    }

    public void ExpandOneLevel(int x, int y)
    {
        //expand the grid with empty cells

        //iterate through all the empty cells

        //detect the cells adjacent to the current cell

        //determine which tile the current should be

        //determine what the adjacent tiles should be
    }
}
