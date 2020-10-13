using System;
using ExporterWeb.Helpers.Services;
using Microsoft.AspNetCore.Identity;

namespace ExporterWeb.Helpers
{
    public class MultiLanguageIdentityErrorDescriber : IdentityErrorDescriber
    {
        private readonly ErrorsLocalizationService _localizer;

        public MultiLanguageIdentityErrorDescriber(ErrorsLocalizationService localizer)
        {
            _localizer = localizer;
        }
        
        public override IdentityError DefaultError()
        {
            return new IdentityError
            {
                Code = nameof(DefaultError),
                Description = _localizer["An unknown failure has occurred."]
            };
        }

        public override IdentityError PasswordMismatch()
        {
            return new IdentityError
            {
                Code = nameof(PasswordMismatch),
                Description = _localizer["Incorrect password."]
            };
        }

        public override IdentityError LoginAlreadyAssociated()
        {
            return new IdentityError
            {
                Code = nameof(LoginAlreadyAssociated),
                Description = _localizer["A user with this login already exists."]
            };
        }

        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(InvalidUserName),
                Description =
                    string.Format(_localizer["User name '{userName}' is invalid, can only contain letters or digits."])
            };
        }
        
        public override IdentityError InvalidEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(InvalidEmail),
                Description = string.Format(_localizer["Email '{email}' is invalid."], email)
            };
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateUserName),
                Description = string.Format(_localizer["User Name {0} is already taken."], userName)
            };
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateEmail),
                Description = string.Format(_localizer["Email '{email}' is already taken."])
            };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = nameof(PasswordTooShort),
                Description = string.Format(_localizer["Passwords must be at least {length} characters."], length)
            };
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresNonAlphanumeric),
                Description = "Passwords must have at least one non alphanumeric character."
            };
        }

        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresDigit),
                Description = _localizer["Passwords must have at least one digit ('0'-'9')."]
            };
        }

        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresLower),
                Description = _localizer["Passwords must have at least one lowercase ('a'-'z')."]
            };
        }

        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresUpper),
                Description = _localizer["Passwords must have at least one uppercase ('A'-'Z')."]
            };
        }
    }
}