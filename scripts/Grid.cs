using Godot;
using System.Collections.Generic;
using System.Numerics;

public partial class Grid : Node
{
    MapHandler mapHandler;
    Vector2I topLeftCell = new Vector2I(5, 0);
    Vector2I topRightCell = new Vector2I(8, 0);
    Vector2I leftTopCell = new Vector2I(2, 3);
    Vector2I leftBotCell = new Vector2I(2, 6);
    Vector2I rightTopCell = new Vector2I(11, 3);
    Vector2I rightBotCell = new Vector2I(11, 6);
    Vector2I botLeftCell = new Vector2I(5, 9);
    Vector2I botRightCell = new Vector2I(8, 9);

    Vector2I topTileId = new Vector2I(4, 0);
    Vector2I leftTileId = new Vector2I();
    Vector2I rightTileId = new Vector2I();
    Vector2I botTileId = new Vector2I(4, 0);

    Vector2I topLeftTileId = new Vector2I(0, 0);
    Vector2I topRightTileId = new Vector2I(1, 0);
    Vector2I botLeftTileId = new Vector2I(0, 1);
    Vector2I botRightTileId = new Vector2I(1, 1);


    public void AddStartingLevel(TileMapLayer map)
    {
        mapHandler = new MapHandler(map);
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
