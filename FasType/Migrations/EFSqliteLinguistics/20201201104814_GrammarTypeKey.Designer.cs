﻿// <auto-generated />
using FasType.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FasType.Migrations.EFSqliteLinguistics
{
    [DbContext(typeof(EFSqliteLinguisticsContext))]
    [Migration("20201201104814_GrammarTypeKey")]
    partial class GrammarTypeKey
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("FasType.Models.Linguistics.Grammars.GrammarTypeRecord", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("Position")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Repr")
                        .HasColumnType("TEXT");

                    b.HasKey("Name");

                    b.ToTable("GrammarTypes");
                });

            modelBuilder.Entity("FasType.Models.Linguistics.SyllableAbbreviationRecord", b =>
                {
                    b.Property<string>("FullForm")
                        .HasColumnType("TEXT");

                    b.Property<int>("Position")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ShortForm")
                        .HasColumnType("TEXT");

                    b.ToTable("AbbreviationMethods");
                });
#pragma warning restore 612, 618
        }
    }
}
