using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Packaging;

namespace electFlemingPackage.Migrations
{
    public class electFlemingPackageMigrationPlan : PackageMigrationPlan
    {
        public electFlemingPackageMigrationPlan()
            : base("ElectMaureenFlemingWebsiteKit")
        {
        }

        protected override void DefinePlan()
        {
            To<ImportPackageXmlMigration>(new Guid("36608059-8b5c-4792-abd5-bfdbb3f8059f"));
        }
    }
}
