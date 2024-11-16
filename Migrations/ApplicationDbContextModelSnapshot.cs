﻿// <auto-generated />
using System;
using BmisApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BmisApi.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BmisApi.Models.Household", b =>
                {
                    b.Property<int>("HouseholdId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("HouseholdId"));

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("HeadId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("LastUpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("HouseholdId")
                        .HasName("household_pkeys");

                    b.ToTable("households", (string)null);
                });

            modelBuilder.Entity("BmisApi.Models.Resident", b =>
                {
                    b.Property<int>("ResidentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ResidentId"));

                    b.Property<DateOnly>("Birthday")
                        .HasColumnType("date");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("HouseholdHead")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<int?>("HouseholdId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("LastUpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Occupation")
                        .HasColumnType("text");

                    b.Property<bool>("RegisteredVoter")
                        .HasColumnType("boolean");

                    b.Property<int>("Sex")
                        .HasColumnType("integer");

                    b.HasKey("ResidentId")
                        .HasName("resident_pkeys");

                    b.HasIndex("HouseholdId");

                    b.ToTable("residents", (string)null);
                });

            modelBuilder.Entity("BmisApi.Models.Resident", b =>
                {
                    b.HasOne("BmisApi.Models.Household", "Household")
                        .WithMany("Members")
                        .HasForeignKey("HouseholdId");

                    b.Navigation("Household");
                });

            modelBuilder.Entity("BmisApi.Models.Household", b =>
                {
                    b.Navigation("Members");
                });
#pragma warning restore 612, 618
        }
    }
}