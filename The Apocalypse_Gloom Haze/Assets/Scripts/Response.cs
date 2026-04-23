using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Response
{
    public bool issuccess;
    public string notification;
    public Account data;
}

[Serializable]
public class Account
{
    public int id;
    public string email;
    public string password;
    public string userName;
    public DateTime CreateAt;
}

[Serializable]
public class ResponseAccountList
{
    public bool issuccess;
    public string notification;
    public List<Account> data;
}

[Serializable]
public class ResponseCharacter
{
    public bool issuccess;
    public string notification;
    public CharacterData data;
}

[Serializable]
public class CharacterData
{
    public int id;
    public int accountId;        // Thêm field này (API có trả về)
    public string characterName;
    public string email;
    public int level;
    public int experience;       // Hoặc đổi thành "exp" nếu API dùng "exp"
    public string createdAt;     // Thêm field này (API có trả về)
}

[Serializable]
public class ResponseCharacterList
{
    public bool issuccess;
    public string notification;
    public List<CharacterData> data;
}