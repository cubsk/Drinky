using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Drinky.DAC
{
    public class Glassware
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }

    }

    public class GlasswareMapping : ClassMap<Glassware>
    {

        public GlasswareMapping()
        {
            Table("Glassware");
            Id(x => x.Id, "Id");
            Map(x => x.Name, "Name");
        }

    }
}
