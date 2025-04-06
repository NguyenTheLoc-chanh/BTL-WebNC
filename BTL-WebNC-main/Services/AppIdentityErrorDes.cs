using Microsoft.AspNetCore.Identity;

namespace BTL_WEBNC.Services
{
    public class AppIdentityErrorDes : IdentityErrorDescriber
    {
        public override IdentityError DuplicateEmail(string email)
        {
            return base.DuplicateEmail(email);
        }
        public override IdentityError DuplicateRoleName(string role)
        {
            var er = base.DuplicateRoleName(role);

            return new IdentityError()
            {
                Code = er.Code,
                Description = $"Role có tên {role} đã tồn tại."
            };
        }

    }
}