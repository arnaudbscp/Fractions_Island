using System;

public class Teacher : User
{

    public string[] classes;


    public Teacher(string email, string username, string password, string[] classes) : base(email, username, password)
    {
        this.classes = classes;
    }
}
