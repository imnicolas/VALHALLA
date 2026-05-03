using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ENTITY;

namespace DAL.Security
{
    public class IntegrityService
    {
        private readonly CryptographyService _cryptoService;

        public IntegrityService()
        {
            _cryptoService = new CryptographyService();
        }

        public void CalculateAndSetDVH(IIntegrityEntity entity)
        {
            string concatData = entity.GetConcatDataForDVH();
            entity.DVH = _cryptoService.CalculateDVH(concatData);
        }

        public bool ValidateDVH(IIntegrityEntity entity)
        {
            string concatData = entity.GetConcatDataForDVH();
            string expectedDVH = _cryptoService.CalculateDVH(concatData);
            return entity.DVH == expectedDVH;
        }

        public string CalculateDVV(IEnumerable<IIntegrityEntity> entities)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var entity in entities.OrderBy(e => e.DVH)) // Sorting to maintain consistency
            {
                sb.Append(entity.DVH);
            }
            return _cryptoService.CalculateDVH(sb.ToString());
        }

        // In a real scenario, CheckDatabaseIntegrity() would:
        // 1. Fetch all records for each sensitive table.
        // 2. Validate DVH for each record.
        // 3. Calculate DVV for each table and compare it with the stored DVV in the 'DVV' table.
        // 4. Return an IntegrityResult (IsValid, FailingTableName, FailingRecordId).
    }
}
