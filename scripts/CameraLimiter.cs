using Godot;

public partial class CameraLimiter : Camera2D
{
	private Vector2 screenSize;


	public override void _Ready()
	{
		screenSize = GetViewportRect().Size;
	}

	public void SetCameraLimits(TileMapLayer tileMapLayer)
	{
		var tileMapUsedPosition = tileMapLayer.GetUsedRect().Position;
		var tileMapUsedSize = tileMapLayer.GetUsedRect().Size;
		GD.Print(tileMapLayer.GetUsedRect());

		SetLimit(Side.Left, ConvertLowerCoordinateToPixels(tileMapUsedPosition.X));
		SetLimit(Side.Right, ConvertHigherCoordinateToPixels(tileMapUsedSize.X) > screenSize.X ? ConvertHigherCoordinateToPixels(tileMapUsedSize.X) : (int) screenSize.X);
		SetLimit(Side.Top, ConvertLowerCoordinateToPixels(tileMapUsedPosition.Y));
		SetLimit(Side.Bottom, ConvertHigherCoordinateToPixels(tileMapUsedSize.Y) > screenSize.Y ? ConvertHigherCoordinateToPixels(tileMapUsedSize.Y) : (int) screenSize.Y);
	}

	private int ConvertLowerCoordinateToPixels(int coordinate)
	{
		return (coordinate - 1) * 100;
	}

	private int ConvertHigherCoordinateToPixels(int coordinate)
	{
		return (coordinate + 1) * 100;
	}
}
