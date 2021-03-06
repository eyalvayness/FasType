﻿// <auto-generated />
using System;
using FasType.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FasType.Migrations
{
    [DbContext(typeof(EFSqliteAbbreviationContext))]
    [Migration("20201120142052_AddedPluralAndGenre")]
    partial class AddedPluralAndGenre
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("FasType.Models.Abbreviations.BaseAbbreviation", b =>
                {
                    b.Property<Guid>("Key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FullForm")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("ShortForm")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Key");

                    b.ToTable("Abbreviations");

                    b.HasDiscriminator<string>("Discriminator").HasValue("BaseAbbreviation");
                });

            modelBuilder.Entity("FasType.Models.Abbreviations.SimpleAbbreviation", b =>
                {
                    b.HasBaseType("FasType.Models.Abbreviations.BaseAbbreviation");

                    b.Property<string>("GenreForm")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("PluralForm")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.ToTable("Abbreviations");

                    b.HasDiscriminator().HasValue("SimpleAbbreviation");
                });
#pragma warning restore 612, 618
        }
    }
}
