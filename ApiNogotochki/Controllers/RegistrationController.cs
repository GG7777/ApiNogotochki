using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ApiNogotochki.Model;
using ApiNogotochki.Repository;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace ApiNogotochki.Controllers
{
    [Controller]
    [Route("[controller]")]
    public class RegistrationController : ControllerBase
    {
        public RegistrationController(UserMetaRepository userMetaRepository)
        {
            this.userMetaRepository = userMetaRepository;
        }
        
        [NotNull]
        [HttpPost]
        public IActionResult Register([CanBeNull] [FromBody] UserMeta userMeta)
        {
            if (userMeta == null)
                return BadRequest("Body is required");

            if (string.IsNullOrEmpty(userMeta.Email))
                return BadRequest("Email is required");

            if (string.IsNullOrEmpty(userMeta.PhoneNumber))
                return BadRequest("PhoneNumber is required");

            if (string.IsNullOrEmpty(userMeta.Password))
                return BadRequest("Password is required");

            var userMetaError = ValidateUserMeta(userMeta);
            if (userMetaError != null)
                return BadRequest(userMetaError);
            
            var uniqueError = CheckUnique(userMeta.Email, userMeta.PhoneNumber);
            if (uniqueError != null)
                return BadRequest(uniqueError);

            userMeta.Email = userMeta.Email.Trim();
            userMeta.PhoneNumber = userMeta.PhoneNumber.Trim();
            userMeta.Password = EncryptPassword(userMeta.Password);

            return Ok(userMetaRepository.SaveNewUser(userMeta));
        }

        [NotNull]
        private string EncryptPassword([NotNull] string password)
        {
            using var md5 = MD5.Create();
            var hashBytes = md5.ComputeHash(Encoding.ASCII.GetBytes(password));
            return string.Join("", hashBytes.Select(x => x.ToString("X2")));
        }

        [CanBeNull]
        private string ValidateUserMeta([NotNull] UserMeta userMeta)
        {
            if (!userMeta.Email.Contains("@"))
                return "Email is not valid";

            if (userMeta.PhoneNumber.Any(x => !int.TryParse(x.ToString(), out _)))
                return "Phone number is not valid";
            
            if (userMeta.Password.Length < 8)
                return "Password is not valid";

            return null;
        }

        [CanBeNull]
        private string CheckUnique([NotNull] string email, [NotNull] string phoneNumber)
        {
            if (userMetaRepository.FindByEmail(email) != null)
                return $"User with email = '{email}' already exists";

            if (userMetaRepository.FindByPhoneNumber(phoneNumber) != null)
                return $"User with phone number = '{phoneNumber}' already exists";

            return null;
        }

        private readonly UserMetaRepository userMetaRepository;
    }
}