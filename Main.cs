using Godot;
using System;

public partial class Main : Node
{
    public int[] inventory = {1, 2, 3};
    public int[,] upgrades = {{0,0},{0,0},{0,0},{0,0},{0,0}};
    public float modifier = 1;
    public int soul = 0;

    public override void _Ready()
    {
        
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionPressed("test")) //debug testing
        {
            foreach (var item in inventory) GD.Print(item);
            GD.Print(upgrades[0,0], " ", upgrades[0,1], " ",
                upgrades[1,0], " ", upgrades[1,1], " ",
                upgrades[2,0], " ", upgrades[2,1], " ", upgrades[3,0], " ", upgrades[3,1]," ",
                upgrades[4,0], " ", upgrades[4,1]);
            GD.Print(modifier);

            foreach (var VARIABLE in GetChildren())
            {
                GD.Print(VARIABLE.Name);                
            }

            if (GetNode("Zhukov/attack") is Attack attack)
            {
                GD.Print($"Dullcount {attack.dullcount}, and {attack.dispersion}");
            }
        }
    }
}
