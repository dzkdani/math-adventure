using System.Collections.Generic;

[System.Serializable]
public class AccountData
{
    public int id;
    public string person_id;
    public string ref_no;
    public string username;
    public string name;
    public string email;
    public string created_at;
    public string updated_at;
    public List<object> permission_group_id;
    public List<object> permission;
    public List<object> permission_group;
}
