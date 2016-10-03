// Copyright(c) Daniel Veintimilla 2016.

namespace CSharpAssignment.Model
{
    /// <summary>Used to return user token to web service response.</summary>
    public class SessionModel : BaseModel
    {
        public string Token { get; set; }

        public SessionModel()
        {
            Token = string.Empty;
        }
    }
}