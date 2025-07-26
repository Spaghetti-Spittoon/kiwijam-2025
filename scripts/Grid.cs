using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

public partial class Grid : Node
{
    MapHandler mapHandler;

    public void AddStartingLevel(TileMapLayer map)
    {
        mapHandler = new MapHandler(map);
        var allCells = map.GetUsedCells();

        //iterate through all cells
        for (int index = 0; index < allCells.Count; index++)
        {
            var currentCellCoord = allCells[index];

            //create the tile defintion
            var tile = mapHandler.GetTileInfo(currentCellCoord.X, currentCellCoord.Y);
        }
    }


    LinkedListNode<NullablePair> CreateNewColumn(TileDefinition newTile)
    {
        var newColumnItem = new LinkedList<TileDefinition>();
        newColumnItem.AddFirst(newTile);

        var newPair = new NullablePair
        {
            Key = newTile.AtlasCoordinates.X,
            Value = newColumnItem
        };
        var newNode = new LinkedListNode<NullablePair>(newPair);
        return newNode;
    }

    void TryInsertAsRow() {

    }

    // public int GetWidth()
    // {
    // }

    // public int GetHeight()
    // {
    // }

    // public TileSetSource GetTile(int x, int y)
    // {
    // }

    public void ExpandOneLevel(int x, int y)
    {
        //expand the grid with empty cells

        //iterate through all the empty cells

        //detect the cells adjacent to the current cell

        //determine which tile the current should be

        //determine what the adjacent tiles should be
    }
}

public class NullablePair {
    public required int Key;
    public required LinkedList<TileDefinition> Value;
}
