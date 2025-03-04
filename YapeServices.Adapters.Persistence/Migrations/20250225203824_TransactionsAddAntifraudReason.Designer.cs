﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Yape.Adapters.Services.Repositories.Persistance;

#nullable disable

namespace YapeServices.Database.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250225203824_TransactionsAddAntifraudReason")]
    partial class TransactionsAddAntifraudReason
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("YapeServices.Entities.Models.Transaction", b =>
                {
                    b.Property<string>("TransactionExternalId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AntifraudReason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ExecutedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("SourceAccountId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TargetAccountId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TransactionStatus")
                        .HasColumnType("int");

                    b.Property<int>("TransferTypeId")
                        .HasColumnType("int");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("TransactionExternalId");

                    b.ToTable("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
