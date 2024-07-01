﻿// <auto-generated />
using System;
using DistributedCodingCompetition.ApiService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DistributedCodingCompetition.ApiService.Migrations
{
    [DbContext(typeof(ContestContext))]
    [Migration("20240701061153_MDRendered")]
    partial class MDRendered
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ContestUser", b =>
                {
                    b.Property<Guid>("AdministeredContestsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("AdministratorsId")
                        .HasColumnType("uuid");

                    b.HasKey("AdministeredContestsId", "AdministratorsId");

                    b.HasIndex("AdministratorsId");

                    b.ToTable("ContestUser");
                });

            modelBuilder.Entity("ContestUser1", b =>
                {
                    b.Property<Guid>("EnteredContestsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ParticipantsId")
                        .HasColumnType("uuid");

                    b.HasKey("EnteredContestsId", "ParticipantsId");

                    b.HasIndex("ParticipantsId");

                    b.ToTable("ContestUser1");
                });

            modelBuilder.Entity("ContestUser2", b =>
                {
                    b.Property<Guid>("BannedContestsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("BannedId")
                        .HasColumnType("uuid");

                    b.HasKey("BannedContestsId", "BannedId");

                    b.HasIndex("BannedId");

                    b.ToTable("ContestUser2");
                });

            modelBuilder.Entity("DistributedCodingCompetition.ApiService.Models.Ban", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("End")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("IssuerId")
                        .HasColumnType("uuid");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Start")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("IssuerId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Ban");
                });

            modelBuilder.Entity("DistributedCodingCompetition.ApiService.Models.Contest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("MinimumAge")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Open")
                        .HasColumnType("boolean");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.Property<bool>("Public")
                        .HasColumnType("boolean");

                    b.Property<string>("RenderedDescription")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Contests");
                });

            modelBuilder.Entity("DistributedCodingCompetition.ApiService.Models.JoinCode", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<bool>("Admin")
                        .HasColumnType("boolean");

                    b.Property<bool>("CloseAfterUse")
                        .HasColumnType("boolean");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("ContestId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Creation")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("CreatorId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Expiration")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("ContestId");

                    b.HasIndex("CreatorId");

                    b.ToTable("JoinCodes");
                });

            modelBuilder.Entity("DistributedCodingCompetition.ApiService.Models.Problem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ContestId")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Difficulty")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.Property<string>("RenderedDescription")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ContestId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Problems");
                });

            modelBuilder.Entity("DistributedCodingCompetition.ApiService.Models.Submission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("ContestId")
                        .HasColumnType("uuid");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("ProblemId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("SubmissionTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("SubmitterId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ContestId");

                    b.HasIndex("ProblemId");

                    b.HasIndex("SubmitterId");

                    b.ToTable("Submissions");
                });

            modelBuilder.Entity("DistributedCodingCompetition.ApiService.Models.TestCase", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Input")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Output")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("ProblemId")
                        .HasColumnType("uuid");

                    b.Property<bool>("Sample")
                        .HasColumnType("boolean");

                    b.Property<int>("Weight")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ProblemId");

                    b.ToTable("TestCases");
                });

            modelBuilder.Entity("DistributedCodingCompetition.ApiService.Models.TestCaseResult", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Error")
                        .HasColumnType("text");

                    b.Property<int>("ExecutionTime")
                        .HasColumnType("integer");

                    b.Property<string>("Output")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Passed")
                        .HasColumnType("boolean");

                    b.Property<Guid>("SubmissionId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TestCaseId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("SubmissionId");

                    b.HasIndex("TestCaseId");

                    b.ToTable("TestCaseResults");
                });

            modelBuilder.Entity("DistributedCodingCompetition.ApiService.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("BanId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("Creation")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("JoinCodeId")
                        .HasColumnType("uuid");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("JoinCodeId");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ContestUser", b =>
                {
                    b.HasOne("DistributedCodingCompetition.ApiService.Models.Contest", null)
                        .WithMany()
                        .HasForeignKey("AdministeredContestsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DistributedCodingCompetition.ApiService.Models.User", null)
                        .WithMany()
                        .HasForeignKey("AdministratorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ContestUser1", b =>
                {
                    b.HasOne("DistributedCodingCompetition.ApiService.Models.Contest", null)
                        .WithMany()
                        .HasForeignKey("EnteredContestsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DistributedCodingCompetition.ApiService.Models.User", null)
                        .WithMany()
                        .HasForeignKey("ParticipantsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ContestUser2", b =>
                {
                    b.HasOne("DistributedCodingCompetition.ApiService.Models.Contest", null)
                        .WithMany()
                        .HasForeignKey("BannedContestsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DistributedCodingCompetition.ApiService.Models.User", null)
                        .WithMany()
                        .HasForeignKey("BannedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DistributedCodingCompetition.ApiService.Models.Ban", b =>
                {
                    b.HasOne("DistributedCodingCompetition.ApiService.Models.User", "Issuer")
                        .WithMany()
                        .HasForeignKey("IssuerId");

                    b.HasOne("DistributedCodingCompetition.ApiService.Models.User", "User")
                        .WithOne("Ban")
                        .HasForeignKey("DistributedCodingCompetition.ApiService.Models.Ban", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Issuer");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DistributedCodingCompetition.ApiService.Models.Contest", b =>
                {
                    b.HasOne("DistributedCodingCompetition.ApiService.Models.User", "Owner")
                        .WithMany("OwnedContests")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("DistributedCodingCompetition.ApiService.Models.JoinCode", b =>
                {
                    b.HasOne("DistributedCodingCompetition.ApiService.Models.Contest", "Contest")
                        .WithMany("JoinCodes")
                        .HasForeignKey("ContestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DistributedCodingCompetition.ApiService.Models.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId");

                    b.Navigation("Contest");

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("DistributedCodingCompetition.ApiService.Models.Problem", b =>
                {
                    b.HasOne("DistributedCodingCompetition.ApiService.Models.Contest", null)
                        .WithMany("Problems")
                        .HasForeignKey("ContestId");

                    b.HasOne("DistributedCodingCompetition.ApiService.Models.User", "Owner")
                        .WithMany("Problems")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("DistributedCodingCompetition.ApiService.Models.Submission", b =>
                {
                    b.HasOne("DistributedCodingCompetition.ApiService.Models.Contest", null)
                        .WithMany("Submissions")
                        .HasForeignKey("ContestId");

                    b.HasOne("DistributedCodingCompetition.ApiService.Models.Problem", "Problem")
                        .WithMany()
                        .HasForeignKey("ProblemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DistributedCodingCompetition.ApiService.Models.User", "Submitter")
                        .WithMany("Submissions")
                        .HasForeignKey("SubmitterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Problem");

                    b.Navigation("Submitter");
                });

            modelBuilder.Entity("DistributedCodingCompetition.ApiService.Models.TestCase", b =>
                {
                    b.HasOne("DistributedCodingCompetition.ApiService.Models.Problem", "Problem")
                        .WithMany("TestCases")
                        .HasForeignKey("ProblemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Problem");
                });

            modelBuilder.Entity("DistributedCodingCompetition.ApiService.Models.TestCaseResult", b =>
                {
                    b.HasOne("DistributedCodingCompetition.ApiService.Models.Submission", "Submission")
                        .WithMany("Results")
                        .HasForeignKey("SubmissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DistributedCodingCompetition.ApiService.Models.TestCase", "TestCase")
                        .WithMany()
                        .HasForeignKey("TestCaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Submission");

                    b.Navigation("TestCase");
                });

            modelBuilder.Entity("DistributedCodingCompetition.ApiService.Models.User", b =>
                {
                    b.HasOne("DistributedCodingCompetition.ApiService.Models.JoinCode", null)
                        .WithMany("Users")
                        .HasForeignKey("JoinCodeId");
                });

            modelBuilder.Entity("DistributedCodingCompetition.ApiService.Models.Contest", b =>
                {
                    b.Navigation("JoinCodes");

                    b.Navigation("Problems");

                    b.Navigation("Submissions");
                });

            modelBuilder.Entity("DistributedCodingCompetition.ApiService.Models.JoinCode", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("DistributedCodingCompetition.ApiService.Models.Problem", b =>
                {
                    b.Navigation("TestCases");
                });

            modelBuilder.Entity("DistributedCodingCompetition.ApiService.Models.Submission", b =>
                {
                    b.Navigation("Results");
                });

            modelBuilder.Entity("DistributedCodingCompetition.ApiService.Models.User", b =>
                {
                    b.Navigation("Ban");

                    b.Navigation("OwnedContests");

                    b.Navigation("Problems");

                    b.Navigation("Submissions");
                });
#pragma warning restore 612, 618
        }
    }
}
