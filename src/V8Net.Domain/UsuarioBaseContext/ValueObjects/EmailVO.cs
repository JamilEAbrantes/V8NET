using FluentValidator.Validation;
using System.Text.RegularExpressions;
using V8Net.Shared.ValueObjects;

namespace V8Net.Domain.UsuarioBaseContext.ValueObjects
{
    public class EmailVO : ValueObject
    {
        public EmailVO() { }

        public EmailVO(string email)
        {
            Email = email;

            AddNotifications(new ValidationContract()
                .IsTrue(Validate(email), "Email", "Email inválido")
            );
        }

        public string Email { get; private set; }

        public bool Validate(string email)
        {
            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            var match = regex.Match(email);
            return match.Success;
        }

        public override string ToString() => $"[ { GetType().Name } - Email: { Email } ]";
    }
}
