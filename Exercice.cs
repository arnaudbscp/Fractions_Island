using System;


public class Exercice
{
    public int[] Bas1 = new int[] { 0, 2 }; // Nombre attendu; Numéro Slot
    public int[] Haut1 = new int[] { 0, 1 };
    public int[] Bas2 = new int[] { 0, 4 };
    public int[] Haut2 = new int[] { 0, 3 };
    public int[] Bas3 = new int[] { 1, 6 };
    public int[] Haut3 = new int[] { 1, 5 };
    public int[] Bas4 = new int[] { 1, 8 };
    public int[] Haut4 = new int[] { 1, 7 };
    public string consigneDemo = "";
    public string boutonSon1 = "";
    public string boutonSon2 = "";
    public string boutonSon3 = "";
    public string boutonSon4 = "";

    public Exercice(int[] b1, int[] b2, int[] b3, int[] b4, int[] h1, int[] h2, int[] h3, int[] h4, string consigne, string bs1, string bs2, string bs3, string bs4)
    {
        Bas1 = b1;
        Haut1 = h1;
        Bas2 = b2;
        Haut2 = h2;
        Bas3 = b3;
        Haut3 = h3;
        Bas4 = b4;
        Haut4 = h4;
        consigneDemo = consigne;
        boutonSon1 = bs1;
        boutonSon2 = bs2;
        boutonSon3 = bs3;
        boutonSon4 = bs4;
    }

}