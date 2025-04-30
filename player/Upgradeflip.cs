using Godot;
using System;

public partial class Upgradeflip : Node
{
    CharacterBody2D player;
    Node attack, dash, main;
    int[][] upgrades;
    int[] weapons;
    float passive_multiplier = 1;

    public override void _Ready()
    {
        player = GetParent<CharacterBody2D>();
        attack = player.GetNode("attack");
        main = GetParent().GetParent();
        if (main is Main gay){
            upgrades = CTJ(gay.upgrades);
            weapons = gay.inventory;
        }
        GD.Print("Upgradenums",upgrades[1][0],upgrades[1][1]);
            for(int i = 0 ; i < 3; i++ ){
                GD.Print("Weapons",weapons[i]);
                switch (weapons[i]){
                    case 1:
                    ak47(upgrades[i]);
                    break;
                    case 2:
                        GD.Print("Weapons==",weapons[i]);
                        drobash(upgrades[i]);
                        break;
                    case 3:
                        break;
                }
            }
        
    }

    public void ak47(int[] al){
        if (attack is Attack tack)
        {
            float[] akd = [3600, 3, 30, 1, 100, 5], akn = [0, 0, 0, 0, 0, 0], aks = [0, 0, 0, 0, 0, 0];
            //akd = ak default akn ak normal aks ak super
            switch (al[0]){
                case 0:
                    akn = [0, 0, 0, 0, 0, 0];
                break;
                case 1:
                    akn = [200, 1, -4, 0, 0, 0];
                break;
                case 2:
                    akn = [-150, -0.5f, 0, 1, 0, 0];
                break;
                case 3:
                    akn = [0, 2, -10, 1, 0, 5];
                break;
            }

            switch (al[1])
            {
                case 4:
                    aks = [0, 0, 0, 0, 0, 0]; break; //speed, damage, clipsize, pierce,
                case 5:                                //waste chance, dispersion
                    aks = [0, -0.5f * (akn[1] + akd[1]), 0, 4, -10, 0]; break;
                case 6:
                    aks = [400, 2*(akn[1] + akd[1]), 0, 0, 0, 10]; break;
                case 7:
                    aks = [0, 0, 0, 1, -50, -akd[5]-akn[5]]; break;
            }
            tack.bulletSpeed[0] = (int)(akn[0] + aks[0] + akd[0]);
            tack.damages[0] = akn[1] + aks[1] + akd[1];
            tack.clipSize = (int)(akn[2] + aks[2] + akd[2]);
            tack.tbf[0] = (int)(akn[3] + aks[3] + akd[3]);
            tack.Unbreaking[0] = (int)(akn[4] + aks[4] + akd[4]);
            tack.dispersion[0] = (int)(akn[5] + aks[5] + akd[5]);
        }
    }

    public void drobash(int[] al)
    {
        int[] _def = [3000, 4, 12, 45], _abs = [3000,  4, 12, 45], _ads = [0,  0, 0, 0];
        float[] _fdef = [3, 0.15f], _fabs = [3, 0.15f], _fads = [0, 0];
        if (attack is Attack tack) {
            switch (al[0])
            {
                case 0:
                    _abs[0] = _def[0];
                    _abs[1] = _def[1];
                    _abs[2] = _def[2];
                    _abs[3] = _def[3];
                    _fabs[0] = _fdef[0];
                    _fabs[1] = _fdef[1];
                break;
                case 1:
                    _abs[1] += 1;
                    _abs[2] += 1;
                    _abs[3] = _def[3];
                    _fabs[1] += 0.12f;
                    _fabs[0] -= 0.5f;
                    break;
                case 2:
                    _abs[1] = _def[1];
                    _abs[2] -= 1;
                    _abs[3] -= 5;
                    _fabs[1] -= 0.07f;
                    _fabs[0] += 1.5f;
                    break;
                case 3:
                    _abs[0] += 400;
                    _abs[1] = _def[1];
                    _abs[2] += 3;
                    _abs[3] -= 15;
                    _fabs[0] -= 0.5f;
                    _fabs[1] -= _fdef[1];
                    break;
            }
            switch (al[1]) {
                case 4:
                    tack.obrez = false;
                    _ads[0] = 0; //Скорость пули
                    _ads[1] = 0; //Количиство дробинок от раздела
                    _ads[2] = 0; //Количество патронов
                    _ads[3] = 0; //Разброз
                    _fads[0] = 0;//Урон
                    _fads[1] = 0;//Сухой Угол
                    tack.tbf[1] = 2;//Прошив
                    break;
                case 5:
                    tack.obrez = true;
                    _ads[0] = 0;
                    _ads[1] = _abs[1];
                    _ads[2] = 0;
                    _ads[3] = 20;
                    _fads[0] = _fabs[0];
                    _fads[1] = 0;
                    tack.tbf[1] = 2;
                    break;
                case 6:
                    tack.obrez = false;
                    _ads[0] = 400;
                    _ads[1] = 0;
                    _ads[2] = -1;
                    _ads[3] = 0;
                    _fads[0] = 0;
                    _fads[1] = -0.1f;
                    tack.tbf[1] = 4;
                    break;
                case 7:
                    tack.obrez = false;
                    _ads[0] = 0;
                    _ads[1] = 0;
                    _ads[2] = 0;
                    _ads[3] = 0;
                    _fads[0] = 0;
                    _fads[1] = 0;
                    tack.tbf[1] = 2;
                    break;
            }
            tack.bulletSpeed[1] = _abs[0] + _ads[0];
            tack.dullcount = _abs[1] + _ads[1];
            tack.clipSize = _abs[2] + _ads[2];
            tack.dispersion[1] = _abs[3] + _ads[3];
            tack.damages[1] = _fabs[0] + _fads[0];
            tack.angle = _fdef[1];
            GD.Print(tack.dullcount);
        }
    }

    public void knife()
    {
        
    }

    public void rifle()
    {
        
    }

    public void pistol()
    {
        
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
