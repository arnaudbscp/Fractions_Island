﻿using System;


public class Student : User
{
    public int money = 0;
    public int[] progression = new int[] {1,1}; // Partie et Niveau
    public string[] temps = new string[] { "Not Started" }; // Partie;Niveau;Temps
    public string[] nb_erreurs = new string[] { "Not Started" }; // "Partie:Niveau:NomZone:ErreurTotalDuNiveau"
    public string[] clics_sur_aide = new string[] { "Not Started" }; // "Partie:Niveau"
    public string classe;
    public int score_invisible = 0; // 
    public string[] avancement_carte = new string[] { "Not Started" };
    public int avatar = 0;

    public Student(string email, string username, string password, string classe) : base(email, username, password)
    {
        if (classe.Equals(""))
            this.classe = "CP";
        else
            this.classe = classe;
    }


}