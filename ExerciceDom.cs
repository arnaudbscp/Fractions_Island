using System;


public class ExerciceDom
{
    public int[] Dom1 = new int[] { 0, 2 }; // Nombre attendu; Numéro Slot
    public int[] Dom2 = new int[] { 0, 1 };
    public int[] Dom3 = new int[] { 0, 4 };
    public string consigneDemo = "";
    public string boutonSon1 = "";
    public string boutonSon2 = "";
    public string boutonSon3 = "";

    public ExerciceDom(int[] b1, int[] b2, int[] b3, string bs1, string bs2, string bs3, string consigne)
    {
        Dom1 = b1;
        Dom2 = b2;
        Dom3 = b3;
        boutonSon1 = bs1;
        boutonSon2 = bs2;
        boutonSon3 = bs3;
        consigneDemo = consigne;
    }

}