using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Configurations
{
    internal class UserResultConfiguration : IEntityTypeConfiguration<UserResult>
    {
        public void Configure(EntityTypeBuilder<UserResult> builder)
        {
            builder.HasKey(us => new { us.UserId, us.ResultId });

            builder
                .HasOne(us => us.User)
                .WithMany(u => u.UserResults)
                .HasForeignKey(us => us.UserId);
            builder
                .HasOne(us => us.Result)
                .WithMany(u => u.UserResults)
                .HasForeignKey(us => us.ResultId);
        }
    }
}
