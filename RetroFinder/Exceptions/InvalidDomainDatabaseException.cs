using System;

namespace RetroFinder.Exceptions
{
    public class InvalidDomainDatabaseException: Exception
    {
        public InvalidDomainDatabaseException(string msg) : base(msg)
        {
            /*
             * Throwed when protein_domains.fa has not expected format.
             * In best case scenario, there is no chance exception will be thrown.
             */
        }
    }
}
