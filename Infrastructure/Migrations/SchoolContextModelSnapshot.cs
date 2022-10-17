﻿// <auto-generated />
using System;
using ContosoUniversityBlazor.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations;

[DbContext(typeof(SchoolContext))]
partial class SchoolContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "6.0.0")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

        modelBuilder.Entity("ContosoUniversityBlazor.Domain.Entities.Course", b =>
            {
                b.Property<int>("CourseID")
                    .HasColumnType("int")
                    .HasColumnName("CourseID");

                b.Property<int>("Credits")
                    .HasColumnType("int");

                b.Property<int>("DepartmentID")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasColumnName("DepartmentID")
                    .HasDefaultValueSql("((1))");

                b.Property<string>("Title")
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.HasKey("CourseID");

                b.HasIndex("DepartmentID");

                b.ToTable("Course", (string)null);
            });

        modelBuilder.Entity("ContosoUniversityBlazor.Domain.Entities.CourseAssignment", b =>
            {
                b.Property<int>("CourseID")
                    .HasColumnType("int")
                    .HasColumnName("CourseID");

                b.Property<int>("InstructorID")
                    .HasColumnType("int")
                    .HasColumnName("InstructorID");

                b.HasKey("CourseID", "InstructorID");

                b.HasIndex("InstructorID");

                b.ToTable("CourseAssignment", (string)null);
            });

        modelBuilder.Entity("ContosoUniversityBlazor.Domain.Entities.Department", b =>
            {
                b.Property<int>("DepartmentID")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasColumnName("DepartmentID");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DepartmentID"), 1L, 1);

                b.Property<decimal>("Budget")
                    .HasColumnType("money");

                b.Property<int?>("InstructorID")
                    .HasColumnType("int")
                    .HasColumnName("InstructorID");

                b.Property<string>("Name")
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.Property<byte[]>("RowVersion")
                    .IsConcurrencyToken()
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnType("rowversion");

                b.Property<DateTime>("StartDate")
                    .HasColumnType("datetime2");

                b.HasKey("DepartmentID");

                b.HasIndex("InstructorID");

                b.ToTable("Department", (string)null);
            });

        modelBuilder.Entity("ContosoUniversityBlazor.Domain.Entities.Enrollment", b =>
            {
                b.Property<int>("EnrollmentID")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasColumnName("EnrollmentID");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EnrollmentID"), 1L, 1);

                b.Property<int>("CourseID")
                    .HasColumnType("int")
                    .HasColumnName("CourseID");

                b.Property<int?>("Grade")
                    .HasColumnType("int");

                b.Property<int>("StudentID")
                    .HasColumnType("int")
                    .HasColumnName("StudentID");

                b.HasKey("EnrollmentID");

                b.HasIndex("CourseID");

                b.HasIndex("StudentID");

                b.ToTable("Enrollment", (string)null);
            });

        modelBuilder.Entity("ContosoUniversityBlazor.Domain.Entities.Instructor", b =>
            {
                b.Property<int>("ID")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasColumnName("ID");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                b.Property<string>("FirstMidName")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)")
                    .HasColumnName("FirstName");

                b.Property<DateTime>("HireDate")
                    .HasColumnType("datetime2");

                b.Property<string>("LastName")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.Property<string>("ProfilePictureName")
                    .HasMaxLength(200)
                    .HasColumnType("nvarchar(200)");

                b.HasKey("ID");

                b.ToTable("Instructor", (string)null);
            });

        modelBuilder.Entity("ContosoUniversityBlazor.Domain.Entities.OfficeAssignment", b =>
            {
                b.Property<int>("InstructorID")
                    .HasColumnType("int")
                    .HasColumnName("InstructorID");

                b.Property<string>("Location")
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.HasKey("InstructorID");

                b.ToTable("OfficeAssignment", (string)null);
            });

        modelBuilder.Entity("ContosoUniversityBlazor.Domain.Entities.Student", b =>
            {
                b.Property<int>("ID")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasColumnName("ID");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                b.Property<DateTime>("EnrollmentDate")
                    .HasColumnType("datetime2");

                b.Property<string>("FirstMidName")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)")
                    .HasColumnName("FirstName");

                b.Property<string>("LastName")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.Property<string>("ProfilePictureName")
                    .HasMaxLength(200)
                    .HasColumnType("nvarchar(200)");

                b.HasKey("ID");

                b.ToTable("Student", (string)null);
            });

        modelBuilder.Entity("ContosoUniversityBlazor.Domain.Entities.Course", b =>
            {
                b.HasOne("ContosoUniversityBlazor.Domain.Entities.Department", "Department")
                    .WithMany("Courses")
                    .HasForeignKey("DepartmentID")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Department");
            });

        modelBuilder.Entity("ContosoUniversityBlazor.Domain.Entities.CourseAssignment", b =>
            {
                b.HasOne("ContosoUniversityBlazor.Domain.Entities.Course", "Course")
                    .WithMany("CourseAssignments")
                    .HasForeignKey("CourseID")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("ContosoUniversityBlazor.Domain.Entities.Instructor", "Instructor")
                    .WithMany("CourseAssignments")
                    .HasForeignKey("InstructorID")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Course");

                b.Navigation("Instructor");
            });

        modelBuilder.Entity("ContosoUniversityBlazor.Domain.Entities.Department", b =>
            {
                b.HasOne("ContosoUniversityBlazor.Domain.Entities.Instructor", "Administrator")
                    .WithMany("Departments")
                    .HasForeignKey("InstructorID");

                b.Navigation("Administrator");
            });

        modelBuilder.Entity("ContosoUniversityBlazor.Domain.Entities.Enrollment", b =>
            {
                b.HasOne("ContosoUniversityBlazor.Domain.Entities.Course", "Course")
                    .WithMany("Enrollments")
                    .HasForeignKey("CourseID")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("ContosoUniversityBlazor.Domain.Entities.Student", "Student")
                    .WithMany("Enrollments")
                    .HasForeignKey("StudentID")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Course");

                b.Navigation("Student");
            });

        modelBuilder.Entity("ContosoUniversityBlazor.Domain.Entities.OfficeAssignment", b =>
            {
                b.HasOne("ContosoUniversityBlazor.Domain.Entities.Instructor", "Instructor")
                    .WithOne("OfficeAssignment")
                    .HasForeignKey("ContosoUniversityBlazor.Domain.Entities.OfficeAssignment", "InstructorID")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Instructor");
            });

        modelBuilder.Entity("ContosoUniversityBlazor.Domain.Entities.Course", b =>
            {
                b.Navigation("CourseAssignments");

                b.Navigation("Enrollments");
            });

        modelBuilder.Entity("ContosoUniversityBlazor.Domain.Entities.Department", b =>
            {
                b.Navigation("Courses");
            });

        modelBuilder.Entity("ContosoUniversityBlazor.Domain.Entities.Instructor", b =>
            {
                b.Navigation("CourseAssignments");

                b.Navigation("Departments");

                b.Navigation("OfficeAssignment");
            });

        modelBuilder.Entity("ContosoUniversityBlazor.Domain.Entities.Student", b =>
            {
                b.Navigation("Enrollments");
            });
#pragma warning restore 612, 618
    }
}
