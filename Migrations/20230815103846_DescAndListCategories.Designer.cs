﻿// <auto-generated />
using System;
using FinanceTrackerAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FinanceTrackerAPI.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230815103846_DescAndListCategories")]
    partial class DescAndListCategories
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.21")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("FinanceTrackerAPI.Model.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ExpenseItemId")
                        .HasColumnType("int");

                    b.Property<int?>("IncomeItemId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ExpenseItemId");

                    b.HasIndex("IncomeItemId");

                    b.ToTable("categories");
                });

            modelBuilder.Entity("FinanceTrackerAPI.Model.ExpenseItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("expenseItems");
                });

            modelBuilder.Entity("FinanceTrackerAPI.Model.IncomeItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("incomeItems");
                });

            modelBuilder.Entity("FinanceTrackerAPI.Model.Category", b =>
                {
                    b.HasOne("FinanceTrackerAPI.Model.ExpenseItem", null)
                        .WithMany("Categories")
                        .HasForeignKey("ExpenseItemId");

                    b.HasOne("FinanceTrackerAPI.Model.IncomeItem", null)
                        .WithMany("Categories")
                        .HasForeignKey("IncomeItemId");
                });

            modelBuilder.Entity("FinanceTrackerAPI.Model.ExpenseItem", b =>
                {
                    b.Navigation("Categories");
                });

            modelBuilder.Entity("FinanceTrackerAPI.Model.IncomeItem", b =>
                {
                    b.Navigation("Categories");
                });
#pragma warning restore 612, 618
        }
    }
}
