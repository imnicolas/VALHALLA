using System;
using DAL.Security;

namespace BLL
{
    public class SystemService
    {
        private readonly IntegrityService _integrityService;

        public SystemService()
        {
            _integrityService = new IntegrityService();
        }

        public void PerformStartupIntegrityCheck()
        {
            // In a real scenario, this would call _integrityService to validate DVV and DVH
            // For now, we simulate the validation.
            // If it fails, it throws an exception.
            
            // Example:
            // if (!_integrityService.CheckDatabaseIntegrity())
            // {
            //     throw new Exception("ERROR DE INTEGRIDAD: La base de datos ha sido alterada externamente.");
            // }
            
            // Console.WriteLine("Integrity check passed.");
        }
    }
}
