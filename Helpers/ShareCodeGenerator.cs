using System;
using System.Linq;

namespace Ervilhinha.Helpers
{
    /// <summary>
    /// Helper para gerar códigos de partilha únicos e amigáveis
    /// </summary>
    public static class ShareCodeGenerator
    {
        private static readonly Random _random = new Random();
        
        // Caracteres permitidos (sem confusão: sem 0, O, I, 1, l)
        private const string AllowedChars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

        /// <summary>
        /// Gera um código de partilha único e legível
        /// Formato: ABC-DEF-GHI (3 grupos de 3 caracteres)
        /// </summary>
        public static string Generate()
        {
            var part1 = GenerateRandomString(3);
            var part2 = GenerateRandomString(3);
            var part3 = GenerateRandomString(3);

            return $"{part1}-{part2}-{part3}";
        }

        /// <summary>
        /// Gera um código curto (6 caracteres sem hífens)
        /// Formato: ABCDEF
        /// </summary>
        public static string GenerateShort()
        {
            return GenerateRandomString(6);
        }

        /// <summary>
        /// Gera um código longo (12 caracteres com hífens)
        /// Formato: ABCD-EFGH-IJKL
        /// </summary>
        public static string GenerateLong()
        {
            var part1 = GenerateRandomString(4);
            var part2 = GenerateRandomString(4);
            var part3 = GenerateRandomString(4);

            return $"{part1}-{part2}-{part3}";
        }

        /// <summary>
        /// Valida se um código tem formato válido
        /// </summary>
        public static bool IsValid(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return false;

            // Remove hífens para validação
            var cleanCode = code.Replace("-", "");

            // Deve ter entre 6 e 12 caracteres
            if (cleanCode.Length < 6 || cleanCode.Length > 12)
                return false;

            // Todos os caracteres devem estar na lista permitida
            return cleanCode.All(c => AllowedChars.Contains(char.ToUpper(c)));
        }

        /// <summary>
        /// Normaliza um código (remove espaços, converte para maiúsculas)
        /// </summary>
        public static string Normalize(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return string.Empty;

            return code.Trim().ToUpper().Replace(" ", "");
        }

        private static string GenerateRandomString(int length)
        {
            return new string(Enumerable.Range(0, length)
                .Select(_ => AllowedChars[_random.Next(AllowedChars.Length)])
                .ToArray());
        }
    }
}
