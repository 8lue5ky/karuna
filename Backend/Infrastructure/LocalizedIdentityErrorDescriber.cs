using Backend.ResourceFiles;

namespace Backend.Infrastructure
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Localization;

    internal class LocalizedIdentityErrorDescriber : IdentityErrorDescriber
    {
        private readonly IStringLocalizer<Resource> _localizer;

        public LocalizedIdentityErrorDescriber(IStringLocalizer<Resource> localizer)
        {
            _localizer = localizer;
        }

        public override IdentityError DefaultError()
            => GetLocalizedError(nameof(DefaultError));

        public override IdentityError ConcurrencyFailure()
            => GetLocalizedError(nameof(ConcurrencyFailure));

        public override IdentityError PasswordMismatch()
            => GetLocalizedError(nameof(PasswordMismatch));

        public override IdentityError InvalidToken()
            => GetLocalizedError(nameof(InvalidToken));

        public override IdentityError LoginAlreadyAssociated()
            => GetLocalizedError(nameof(LoginAlreadyAssociated));

        public override IdentityError InvalidUserName(string userName)
            => GetLocalizedError(nameof(InvalidUserName), userName);

        public override IdentityError InvalidEmail(string email)
            => GetLocalizedError(nameof(InvalidEmail), email);

        public override IdentityError DuplicateUserName(string userName)
            => GetLocalizedError(nameof(DuplicateUserName), userName);

        public override IdentityError DuplicateEmail(string email)
            => GetLocalizedError(nameof(DuplicateEmail), email);

        public override IdentityError InvalidRoleName(string role)
            => GetLocalizedError(nameof(InvalidRoleName), role);

        public override IdentityError DuplicateRoleName(string role)
            => GetLocalizedError(nameof(DuplicateRoleName), role);

        public override IdentityError UserAlreadyHasPassword()
            => GetLocalizedError(nameof(UserAlreadyHasPassword));

        public override IdentityError UserLockoutNotEnabled()
            => GetLocalizedError(nameof(UserLockoutNotEnabled));

        public override IdentityError UserAlreadyInRole(string role)
            => GetLocalizedError(nameof(UserAlreadyInRole), role);

        public override IdentityError UserNotInRole(string role)
            => GetLocalizedError(nameof(UserNotInRole), role);

        public override IdentityError PasswordTooShort(int length)
            => GetLocalizedError(nameof(PasswordTooShort), length);

        public override IdentityError PasswordRequiresNonAlphanumeric()
            => GetLocalizedError(nameof(PasswordRequiresNonAlphanumeric));

        public override IdentityError PasswordRequiresDigit()
            => GetLocalizedError(nameof(PasswordRequiresDigit));

        public override IdentityError PasswordRequiresLower()
            => GetLocalizedError(nameof(PasswordRequiresLower));

        public override IdentityError PasswordRequiresUpper()
            => GetLocalizedError(nameof(PasswordRequiresUpper));

        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
            => GetLocalizedError(nameof(PasswordRequiresUniqueChars), uniqueChars);

        private IdentityError GetLocalizedError(string key, params object[] args)
            => new()
            {
                Code = key,
                Description = args.Length == 0
                    ? _localizer[key]
                    : string.Format(_localizer[key], args)
            };
    }


}
