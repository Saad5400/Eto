﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjectEtoPrototype.Data;

#nullable disable

namespace ProjectEtoPrototype.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ProjectEtoPrototype.Models.Bank", b =>
                {
                    b.Property<int>("BankId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BankId"), 1L, 1);

                    b.Property<float>("Balance")
                        .HasColumnType("real");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("BankId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Banks");
                });

            modelBuilder.Entity("ProjectEtoPrototype.Models.DailyTask", b =>
                {
                    b.Property<int>("TaskID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TaskID"), 1L, 1);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("TaskID");

                    b.HasIndex("UserId");

                    b.ToTable("DailyTasks");
                });

            modelBuilder.Entity("ProjectEtoPrototype.Models.Operation", b =>
                {
                    b.Property<int>("OperationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OperationId"), 1L, 1);

                    b.Property<float>("Amount")
                        .HasColumnType("real");

                    b.Property<int>("BankId")
                        .HasColumnType("int");

                    b.Property<string>("Class")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("OperationId");

                    b.HasIndex("BankId");

                    b.ToTable("Operations");
                });

            modelBuilder.Entity("ProjectEtoPrototype.Models.Preference", b =>
                {
                    b.Property<int>("PreferenceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PreferenceId"), 1L, 1);

                    b.Property<DateTime>("CaloriesLstDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("CurrentCalories")
                        .HasColumnType("int");

                    b.Property<int>("MaxCalories")
                        .HasColumnType("int");

                    b.Property<int>("SurahId")
                        .HasColumnType("int");

                    b.Property<string>("Theme")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("VerseId")
                        .HasColumnType("int");

                    b.HasKey("PreferenceId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Preferences");
                });

            modelBuilder.Entity("ProjectEtoPrototype.Models.User", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ProjectEtoPrototype.Models.Bank", b =>
                {
                    b.HasOne("ProjectEtoPrototype.Models.User", "User")
                        .WithOne("Bank")
                        .HasForeignKey("ProjectEtoPrototype.Models.Bank", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProjectEtoPrototype.Models.DailyTask", b =>
                {
                    b.HasOne("ProjectEtoPrototype.Models.User", "User")
                        .WithMany("DailyTasks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProjectEtoPrototype.Models.Operation", b =>
                {
                    b.HasOne("ProjectEtoPrototype.Models.Bank", "Bank")
                        .WithMany("Operations")
                        .HasForeignKey("BankId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bank");
                });

            modelBuilder.Entity("ProjectEtoPrototype.Models.Preference", b =>
                {
                    b.HasOne("ProjectEtoPrototype.Models.User", "User")
                        .WithOne("Preference")
                        .HasForeignKey("ProjectEtoPrototype.Models.Preference", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProjectEtoPrototype.Models.Bank", b =>
                {
                    b.Navigation("Operations");
                });

            modelBuilder.Entity("ProjectEtoPrototype.Models.User", b =>
                {
                    b.Navigation("Bank")
                        .IsRequired();

                    b.Navigation("DailyTasks");

                    b.Navigation("Preference")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
