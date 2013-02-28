using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Drinky.DAC
{
    public class UserInfo
    {
        public virtual Guid Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual string UserName { get; set; }

    }

    public class UserInfoMapping : ClassMap<UserInfo>
    {

        public UserInfoMapping()
        {
            Table("UserInfo");
            Id(x => x.Id, "UserId");
            Map(x => x.FirstName, "FirstName");
            Map(x => x.LastName, "LastName");
            Map(x => x.DisplayName, "DisplayName");
            Map(x => x.UserName, "UserName");

        }

    }
}
