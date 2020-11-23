using System;


public class Student : User
{
    public int money = 0;
    public string[] progression = new string[] { "Not Started" };
    public string[] temps = new string[] { "Not Started" };
    public string[] nb_erreurs = new string[] { "Not Started" };
    public string[] clics_sur_aide = new string[] { "Not Started" };
    public string classe;
    public int score_invisible = 0;
    public string[] avancement_carte = new string[] { "Not Started" };
    public int avatar = 0;

    public Student(string email, string username, string password, string classe) : base(email, username, password)
    {
        this.classe = classe;
    }


}