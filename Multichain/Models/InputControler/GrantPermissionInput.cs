public class GrantPermissionInput
{
    public string Address { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsReceive { get; set; }
    public bool IsSend { get; set; }
    public bool IsConnect { get; set; }
    public bool IsCreate { get; set; }
    public bool IsIssue { get; set; }
    public bool IsMine { get; set; }
    public bool IsActivate { get; set; }
}