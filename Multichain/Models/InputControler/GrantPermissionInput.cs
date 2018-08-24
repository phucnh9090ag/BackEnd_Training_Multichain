public class GrantPermissionInput
{
    public string address { get; set; }
    public bool isAdmin { get; set; }
    public bool isReceive { get; set; }
    public bool isSend { get; set; }
    public bool isConnect { get; set; }
    public bool isCreate { get; set; }
    public bool isIssue { get; set; }
    public bool isMine { get; set; }
    public bool isActivate { get; set; }
}