﻿// <auto-generated />
using System;
using System.Collections.Generic;
using DistributedCodingCompetition.CodeExecution.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DistributedCodingCompetition.CodeExecution.Migrations
{
    [DbContext(typeof(ExecRunnerContext))]
    [Migration("20240626043623_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DistributedCodingCompetition.CodeExecution.Models.ExecRunner", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Authenticated")
                        .HasColumnType("boolean");

                    b.Property<bool>("Available")
                        .HasColumnType("boolean");

                    b.Property<bool>("Enabled")
                        .HasColumnType("boolean");

                    b.Property<string>("Endpoint")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<List<string>>("Languages")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<bool>("Live")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<List<string>>("Packages")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SystemInfo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Weight")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("ExecRunners");
                });
#pragma warning restore 612, 618
        }
    }
}
