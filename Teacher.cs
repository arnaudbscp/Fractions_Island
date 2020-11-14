using System;

public class Teacher : User
{

    public string[] classes = new string[] {"CP","CE1","CE2"};
    public int code_secret = 5566;


    public Teacher(string email, string username, string password) : base(email, username, password)
    { }
}
