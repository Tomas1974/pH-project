﻿using System.ComponentModel.DataAnnotations;

namespace infrastructure.DataModels;

public class UserModel
{
    public string email { get; set; }
    public string username { get; set; }
    public string password { get; set; }
    public string address { get; set; }
    public int zipcode { get; set; }
    public int? cvr { get; set; }
}



public class LoginModel
{
    public string username { get; set; }
    public string password { get; set; }
   
}









public class UserSaveToDatabaseModel
{
    public string email { get; set; }
    public string username { get; set; }
    public string hash { get; set; }
    public string salt { get; set; }
    public string address { get; set; }
    public int zipcode { get; set; }
    public int? cvr { get; set; }
}