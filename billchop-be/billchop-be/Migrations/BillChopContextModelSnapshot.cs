﻿// <auto-generated />
using System;
using BillChopBE.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BillChopBE.Migrations
{
    [DbContext(typeof(BillChopContext))]
    partial class BillChopContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("BillChopBE.DataAccessLayer.Models.Bill", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("GroupContextId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("LoanerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Total")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("GroupContextId");

                    b.HasIndex("LoanerId");

                    b.ToTable("Bills");
                });

            modelBuilder.Entity("BillChopBE.DataAccessLayer.Models.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("BillChopBE.DataAccessLayer.Models.Loan", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("BillId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("LoaneeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("BillId");

                    b.HasIndex("LoaneeId");

                    b.ToTable("Loans");
                });

            modelBuilder.Entity("BillChopBE.DataAccessLayer.Models.Payment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("GroupContextId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PayerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ReceiverId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("GroupContextId");

                    b.HasIndex("PayerId");

                    b.HasIndex("ReceiverId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("BillChopBE.DataAccessLayer.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("GroupUser", b =>
                {
                    b.Property<Guid>("GroupsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UsersId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("GroupsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("GroupUser");
                });

            modelBuilder.Entity("BillChopBE.DataAccessLayer.Models.Bill", b =>
                {
                    b.HasOne("BillChopBE.DataAccessLayer.Models.Group", "GroupContext")
                        .WithMany("Bills")
                        .HasForeignKey("GroupContextId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BillChopBE.DataAccessLayer.Models.User", "Loaner")
                        .WithMany("Bills")
                        .HasForeignKey("LoanerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("GroupContext");

                    b.Navigation("Loaner");
                });

            modelBuilder.Entity("BillChopBE.DataAccessLayer.Models.Loan", b =>
                {
                    b.HasOne("BillChopBE.DataAccessLayer.Models.Bill", "Bill")
                        .WithMany("Loans")
                        .HasForeignKey("BillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BillChopBE.DataAccessLayer.Models.User", "Loanee")
                        .WithMany("Loans")
                        .HasForeignKey("LoaneeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Bill");

                    b.Navigation("Loanee");
                });

            modelBuilder.Entity("BillChopBE.DataAccessLayer.Models.Payment", b =>
                {
                    b.HasOne("BillChopBE.DataAccessLayer.Models.Group", "GroupContext")
                        .WithMany()
                        .HasForeignKey("GroupContextId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BillChopBE.DataAccessLayer.Models.User", "Payer")
                        .WithMany("PaymentsMade")
                        .HasForeignKey("PayerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("BillChopBE.DataAccessLayer.Models.User", "Receiver")
                        .WithMany("PaymentsReceived")
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("GroupContext");

                    b.Navigation("Payer");

                    b.Navigation("Receiver");
                });

            modelBuilder.Entity("GroupUser", b =>
                {
                    b.HasOne("BillChopBE.DataAccessLayer.Models.Group", null)
                        .WithMany()
                        .HasForeignKey("GroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BillChopBE.DataAccessLayer.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BillChopBE.DataAccessLayer.Models.Bill", b =>
                {
                    b.Navigation("Loans");
                });

            modelBuilder.Entity("BillChopBE.DataAccessLayer.Models.Group", b =>
                {
                    b.Navigation("Bills");
                });

            modelBuilder.Entity("BillChopBE.DataAccessLayer.Models.User", b =>
                {
                    b.Navigation("Bills");

                    b.Navigation("Loans");

                    b.Navigation("PaymentsMade");

                    b.Navigation("PaymentsReceived");
                });
#pragma warning restore 612, 618
        }
    }
}
