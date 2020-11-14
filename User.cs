using System;
public class User
{
    public string password;
    public string email;
    public string username;

    public User(string email, string username, string password)
    {
        this.password = password;
        this.email = email;
        this.username = username;
    }
}