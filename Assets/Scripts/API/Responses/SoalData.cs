using System.Collections.Generic;

[System.Serializable]
public class SoalData 
{
    public int id;
    public string ref_no;
    public string category_id;
    public string class_id;
    public string semester_id;
    public string topic_id;
    public string packet_id;
    public string answer;
    public string position;
    public string created_at;
    public string updated_at;
    public string category_label;
    public string class_label;
    public string semester_label;
    public string topic_label;
    public string packet_label;
    public List<GambarSoal> details;
}

[System.Serializable]
public class GambarSoal
{
    public int id;
    public string question_id;
    public string is_question_image;
    public string ref_no;
    public string media;
    public string position;
    public string created_at;
    public string updated_at;
    public string file_url;
}
