using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using RSS_Feeds.Models.DTOs;

namespace RSS_Feeds.Architecture.Helpers
{
    public static class PasswordHasherHelper
    {
        private static readonly PasswordHasher<UsuarioDTO> _hasher = new();

        public static string Hash(string password)
        {
            return _hasher.HashPassword(null!, password);
        }

        public static bool Verify(string hash, string password)
        {
            var result = _hasher.VerifyHashedPassword(
                null!,
                hash,
                password
            );

            return result == PasswordVerificationResult.Success
                || result == PasswordVerificationResult.SuccessRehashNeeded;
        }
    }
}
