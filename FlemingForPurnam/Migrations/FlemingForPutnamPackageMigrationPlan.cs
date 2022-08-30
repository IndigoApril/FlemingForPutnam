using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Packaging;

namespace FlemingForPutnam.Migrations
{
    public class FlemingForPutnamPackageMigrationPlan : PackageMigrationPlan
    {
        public FlemingForPutnamPackageMigrationPlan()
            : base("FlemingForPutnamKit")
        {
        }

        protected override void DefinePlan()
        {
            To<ImportPackageXmlMigration>(new Guid("36608059-8b5c-4792-abd5-bfdbb3f8059f"));
        }
    }
}
