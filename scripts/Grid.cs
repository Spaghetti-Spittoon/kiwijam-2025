using Godot;
using System;
using System.Collections.Generic;
using System.Numerics;

public partial class Grid : Node
{
    Vector2I leftTopCell = new Vector2I(2, 3);
    Vector2I leftTop2Cell = new Vector2I(2, 4);

    Vector2I topLeftCell = new Vector2I(5, 0);
    Vector2I topLeft2Cell = new Vector2I(6, 0);

    Vector2I topRightCell = new Vector2I(8, 0);
    Vector2I topRight2Cell = new Vector2I(7, 0);

    Vector2I rightTopCell = new Vector2I(11, 3);
    Vector2I rightTop2Cell = new Vector2I(11, 4);

    Vector2I rightBotCell = new Vector2I(11, 6);
    Vector2I rightBot2Cell = new Vector2I(11, 5);

    Vector2I botRightCell = new Vector2I(8, 9);
    Vector2I botRight2Cell = new Vector2I(7, 9);

    Vector2I botLeftCell = new Vector2I(5, 9);
    Vector2I botLeft2Cell = new Vector2I(6, 9);

    Vector2I leftBotCell = new Vector2I(2, 6);
    Vector2I leftBot2Cell = new Vector2I(2, 5);

    public void ExpandOneLevel(TileMapLayer mapLayer, int tilesetId)
    {
        GD.Print($"{nameof(ExpandOneLevel)}");
        //get atlas coordinates
        var helper = new MapHandler(mapLayer);

        var leftTop1Info = helper.GetTileInfo(leftTopCell);
        GD.Print($"lefttop atlas: {leftTop1Info.AtlasCoordinates}");
        var leftTop2Info = helper.GetTileInfo(leftTop2Cell);
        GD.Print($"lefttop2 atlas: {leftTop2Info.AtlasCoordinates}");
        var topLeft1Info = helper.GetTileInfo(topLeftCell);
        GD.Print($"topLeft atlas: {topLeft1Info.AtlasCoordinates}");
        var topLeft2Info = helper.GetTileInfo(topLeft2Cell);
        GD.Print($"topLeft2 atlas: {topLeft2Info.AtlasCoordinates}");

        var topRight1Info = helper.GetTileInfo(topRightCell);
        GD.Print($"topRight atlas: {topRight1Info.AtlasCoordinates}");
        var topRight2Info = helper.GetTileInfo(topRight2Cell);
        GD.Print($"topRight2 atlas: {topRight2Info.AtlasCoordinates}");
        var rightTop1Info = helper.GetTileInfo(rightTopCell);
        GD.Print($"rightTop atlas: {rightTop1Info.AtlasCoordinates}");
        var rightTop2Info = helper.GetTileInfo(rightTop2Cell);
        GD.Print($"rightTop2 atlas: {rightTop2Info.AtlasCoordinates}");

        var rightBot1Info = helper.GetTileInfo(rightBotCell);
        GD.Print($"rightBot atlas: {rightBot1Info.AtlasCoordinates}");
        var rightBot2Info = helper.GetTileInfo(rightBot2Cell);
        GD.Print($"rightBot2 atlas: {rightBot2Info.AtlasCoordinates}");
        var botRight1Info = helper.GetTileInfo(botRightCell);
        GD.Print($"botRight atlas: {botRight1Info.AtlasCoordinates}");
        var botRight2Info = helper.GetTileInfo(botRight2Cell);
        GD.Print($"botRight2 atlas: {botRight2Info.AtlasCoordinates}");

        var botLeft1Info = helper.GetTileInfo(botLeftCell);
        GD.Print($"botLeft atlas: {botLeft1Info.AtlasCoordinates}");
        var botLeft2Info = helper.GetTileInfo(botLeftCell);
        GD.Print($"botLeft2 atlas: {botLeft2Info.AtlasCoordinates}");
        var leftBot1Info = helper.GetTileInfo(leftBotCell);
        GD.Print($"leftBot atlas: {leftBot1Info.AtlasCoordinates}");
        var leftBot2Info = helper.GetTileInfo(leftBot2Cell);
        GD.Print($"leftBot2 atlas: {leftBot2Info.AtlasCoordinates}");

        //expand top left quadrant
        var newLeftTop1 = new Vector2I(leftTopCell.X - 1, leftTopCell.Y); //translate lefttop leftware
        var newLeftTop2 = new Vector2I(newLeftTop1.X, newLeftTop1.Y + 1); //go adjacent down
        var newTopLeft1 = new Vector2I(topLeftCell.X, topLeftCell.Y - 1); //translate topleft upward
        var newTopLeft2 = new Vector2I(newTopLeft1.X + 1, newTopLeft1.Y); //go adjacent right

        //expand top right quadrant
        var newTopRight1 = new Vector2I(topRightCell.X, topRightCell.Y - 1); //translate topright upward
        var newTopRight2 = new Vector2I(newTopRight1.X - 1, newTopRight1.Y); //go adjacent left
        var newRightTop1 = new Vector2I(rightTopCell.X + 1, rightTopCell.Y); //translate righttop leftware
        var newRightTop2 = new Vector2I(newRightTop1.X, newRightTop1.Y + 1); //go adjacent down

        //expand bot left quadrant
        var newRightBot1 = new Vector2I(rightBotCell.X + 1, rightBotCell.Y); //translate rightbot upward
        var newRightBot2 = new Vector2I(newRightBot1.X, newRightBot1.Y - 1); //go adjacent up
        var newBotRight1 = new Vector2I(botRightCell.X, botRightCell.Y + 1); //translate botright leftware
        var newBotRight2 = new Vector2I(newBotRight1.X - 1, newBotRight1.Y); //go adjacent left

        // expand bot right quadrant
        var newBotLeft1 = new Vector2I(botLeftCell.X, botLeftCell.Y + 1); //translate botleft upward
        var newBotLeft2 = new Vector2I(newBotLeft1.X + 1, newBotLeft1.Y); //go adjacent right
        var newLeftBot1 = new Vector2I(leftBotCell.X - 1, leftBotCell.Y); //translate leftbot leftware
        var newLeftBot2 = new Vector2I(newLeftBot1.X, newLeftBot1.Y - 1); //go adjacent up

        //expand all straight side tiles
        mapLayer.SetCell(newLeftTop1, tilesetId, leftTop1Info.AtlasCoordinates);
        mapLayer.SetCell(newLeftTop2, tilesetId, leftTop2Info.AtlasCoordinates);
        mapLayer.SetCell(newTopLeft1, tilesetId, topLeft1Info.AtlasCoordinates);
        mapLayer.SetCell(newTopLeft2, tilesetId, topLeft2Info.AtlasCoordinates);

        mapLayer.SetCell(newTopRight1, tilesetId, topRight1Info.AtlasCoordinates);
        mapLayer.SetCell(newTopRight2, tilesetId, topRight2Info.AtlasCoordinates);
        mapLayer.SetCell(newRightTop1, tilesetId, rightTop1Info.AtlasCoordinates);
        mapLayer.SetCell(newRightTop2, tilesetId, rightTop2Info.AtlasCoordinates);

        mapLayer.SetCell(newRightBot1, tilesetId, rightBot1Info.AtlasCoordinates);
        mapLayer.SetCell(newRightBot2, tilesetId, rightBot2Info.AtlasCoordinates);
        mapLayer.SetCell(newBotRight1, tilesetId, botRight1Info.AtlasCoordinates);
        mapLayer.SetCell(newBotRight2, tilesetId, botRight2Info.AtlasCoordinates);

        mapLayer.SetCell(newBotLeft1, tilesetId, botLeft1Info.AtlasCoordinates);
        mapLayer.SetCell(newBotLeft2, tilesetId, botLeft2Info.AtlasCoordinates);
        mapLayer.SetCell(newLeftBot1, tilesetId, leftBot1Info.AtlasCoordinates);
        mapLayer.SetCell(newLeftBot2, tilesetId, leftBot2Info.AtlasCoordinates);

        //expand diagonal side top left


        //expand diagonal side top right

        //expand diagonal side bot right

        //expand diagonal side bot left

        //assign expanded references as coin state  
        leftTopCell = newLeftTop1;
        leftTop2Cell = newLeftTop2;

        topLeftCell = newTopLeft1;
        topLeft2Cell = newTopLeft2;

        topRightCell = newTopRight1;
        topRight2Cell = newTopRight2;

        rightTopCell = newRightTop1;
        rightTop2Cell = newRightTop2;

        rightBotCell = newRightBot1;
        rightBot2Cell = newRightBot2;

        botRightCell = newBotRight1;
        botRight2Cell = newBotRight2;

        botLeftCell = newBotLeft1;
        botLeft2Cell = newBotLeft2;

        leftBotCell = newLeftBot1;
        leftBot2Cell = newLeftBot2;
    }
}
