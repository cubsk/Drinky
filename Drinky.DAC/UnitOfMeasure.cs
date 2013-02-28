using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Drinky.DAC
{
    public class UnitOfMeasure
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Abbreviation { get; set; }
        public virtual int DisplayOrder { get; set; }
        public virtual double DefaultIncrement { get; set; }
        public virtual bool IsDefault { get; set; }
        public virtual int? Denominator { get; set; }
        public virtual int SortOrder { get; set; }

        public virtual string DescribeQuantity(double quantity)
        {
            return UnitOfMeasure.DescribeQuantity(quantity, Abbreviation);
        }

        public static string DescribeQuantity(double quantity, string abbreviation)
        {

            Fraction value = new Fraction(quantity);
            return string.Format("{0} {1}", value.Describe(), abbreviation);
        }
    }

    public class UnitOfMeasureMapping : ClassMap<UnitOfMeasure>
    {

        public UnitOfMeasureMapping()
        {
            Table("UnitOfMeasure");
            Id(x => x.Id, "Id");
            Map(x => x.Name, "Name");
            Map(x => x.Abbreviation, "Abbreviation");
            Map(x => x.DisplayOrder, "DisplayOrder");
            Map(x => x.DefaultIncrement, "DefaultIncrement");
            Map(x => x.IsDefault, "IsDefault");
            Map(x => x.Denominator, "Denominator");
            Map(x => x.SortOrder, "SortOrder");


        }

    }
}
