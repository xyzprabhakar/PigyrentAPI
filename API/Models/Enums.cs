namespace API.Models
{
    public enum enmErrorMessage
    {
        IdentifierRequired=1,
        IdentifierLength=2,
    }

    public enum enmMenuDisplayType
    {
        None=0,
        VisibleBeforeSignIn=1,
        VisibleAfterSignIn = 2,
        AlwaysVisible=3
    }
}
