using Godot;
using System;

public partial class SignalBus : Node
{
    [Signal]
    public delegate void Del_LevelExpandedEventHandler();
    public event Del_LevelExpandedEventHandler LevelExpanded;

    public void ExpandLevel()
    {
        LevelExpanded?.Invoke();
    }
}
