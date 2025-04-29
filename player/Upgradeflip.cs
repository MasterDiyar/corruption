using Godot;
using System;

public partial class Upgradeflip : Node
{
    CharacterBody2D player;
    Node attack, dash, main;
    int[][] upgrades;
    int[] weapons;

    public override void _Ready()
    {
        main = GetParent().GetParent();
        if (main is Main gay){
            upgrades = CTJ(gay.upgrades);
            weapons = gay.inventory;
        }
        
            for(int i = 0 ; i < 4; i++ ){
                switch (weapons[i]){
                    case 1:
                    ak47(upgrades[i]);
                    break;
                }
            }
        
    }

    public void ak47(int[] al){
        if (attack is Attack tack){
            switch (al[0]){
                case 0:
                    tack.bulletSpeed[0] = 3600;
                    tack.damages[0] = 3;
                    tack.clipSize = 30;
                    tack.tbf = 1;
                break;
                case 1:
                    tack.bulletSpeed[0] = 3800;
                    tack.damages[0] = 4;
                    tack.clipSize = 26;
                break;
                case 2:
                    tack.bulletSpeed[0] = 3450;
                    tack.damages[0] = 2;
                    tack.tbf = 2;
                break;
                case 3:
                    
                break;
            }
        }
    }

    public static int[][] CTJ(int[,] array){ //ConvertToJagged
        int rows = array.GetLength(0);
        int cols = array.GetLength(1);
        int[][] result = new int[rows][];
        for (int i = 0; i < rows; i++){
            result[i] = new int[cols];
            for (int j = 0; j < cols; j++)
            {
                result[i][j] = array[i, j];
            }}return result;
    }

}
