using System.Collections.Generic;

[System.Serializable]
public class Response<T> 
{
    public T data; 
    public bool is_error;
    public int error_code; 
    public int status;
    public string message;
    public List<string> errors;
    public List<object> _meta;
    public List<object> trace;
}
