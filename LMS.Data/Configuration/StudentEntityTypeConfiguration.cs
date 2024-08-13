﻿using LMS.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Domain.Configuration
{
    public class StudentEntityTypeConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            //builder
            //    .HasMany(sc => sc.StudentCourses)
            //    .WithOne(s => s.Student)
            //    .HasForeignKey(s => s.StudentId)
            //    .OnDelete(DeleteBehavior.NoAction);
            builder
                .HasMany(s => s.ExamResults)
                .WithOne(e => e.Student)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasMany(c => c.Evaluations)
                .WithOne(e => e.Student)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
