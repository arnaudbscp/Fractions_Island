using System;


public class Student : User
{
    public int money = 15;
    public string[] progression = new string[] { "Not Started" };
    public string[] temps = new string[] { "Not Started" };
    public string[] nb_erreurs = new string[] { "Not Started" };
    public string[] clics_sur_aide = new string[] { "Not Started" };
    public string classe = "CP";
    public int score_invisible = 0;
    public string[] avancement_carte = new string[] { "Not Started" };

    public Student(string email, string username, string password) : base(email, username, password)
    { }


}