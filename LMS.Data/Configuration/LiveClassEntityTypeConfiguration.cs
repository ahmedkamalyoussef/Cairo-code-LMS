﻿using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Domain.Configuration
{
    public class LiveClassEntityTypeConfiguration : IEntityTypeConfiguration<LiveClass>
    {
        public void Configure(EntityTypeBuilder<LiveClass> builder)
        {
            builder
                .HasOne(a => a.Creator)
                .WithMany(q => q.LiveClasses)
                .HasForeignKey(a => a.CreatorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}