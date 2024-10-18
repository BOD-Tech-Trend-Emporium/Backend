namespace Api.src.Auth.domain.dto
{
    public class ChangePasswordDto
    {
        public string email { get; set; }
        public string password { get; set; }
        public string SecurityQuestionAnswer { get; set; }
        public int SecurityQuestion { get; set; }
    }
}
