﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RSSCargo.DAL.DataContext;

#nullable disable

namespace RSSCargo.DAL.Migrations
{
    [DbContext(typeof(RsscargoContext))]
    partial class RsscargoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0-preview.2.23128.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("RSSCargo.DAL.Models.Cargo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("cargos_pk");

                    b.HasIndex(new[] { "Name" }, "cargos_name_index")
                        .IsUnique();

                    b.ToTable("cargos", (string)null);
                });

            modelBuilder.Entity("RSSCargo.DAL.Models.CargoFeed", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("Id"));

                    b.Property<int>("CargoId")
                        .HasColumnType("integer")
                        .HasColumnName("cargo_id");

                    b.Property<string>("RssFeed")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)")
                        .HasColumnName("rss_feed");

                    b.HasKey("Id")
                        .HasName("cargo_feeds_pk");

                    b.HasIndex(new[] { "CargoId", "RssFeed" }, "cargo_feeds_rss_feed_index")
                        .IsUnique();

                    b.ToTable("cargo_feeds", (string)null);
                });

            modelBuilder.Entity("RSSCargo.DAL.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("email");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("password");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("username");

                    b.HasKey("Id")
                        .HasName("users_pk");

                    b.HasIndex(new[] { "Email" }, "users_email_index")
                        .IsUnique();

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("RSSCargo.DAL.Models.UserCargo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("Id"));

                    b.Property<int>("CargoId")
                        .HasColumnType("integer")
                        .HasColumnName("cargo_id");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("user_cargos_pk");

                    b.HasIndex("CargoId");

                    b.HasIndex(new[] { "UserId", "CargoId" }, "user_cargos_cargo_id_index")
                        .IsUnique();

                    b.ToTable("user_cargos", (string)null);
                });

            modelBuilder.Entity("RSSCargo.DAL.Models.UserFeed", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("Id"));

                    b.Property<string>("RssFeed")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)")
                        .HasColumnName("rss_feed");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("user_feeds_pk");

                    b.HasIndex(new[] { "UserId", "RssFeed" }, "user_feeds_rss_feed_index")
                        .IsUnique();

                    b.ToTable("user_feeds", (string)null);
                });

            modelBuilder.Entity("RSSCargo.DAL.Models.CargoFeed", b =>
                {
                    b.HasOne("RSSCargo.DAL.Models.Cargo", "Cargo")
                        .WithMany("CargoFeeds")
                        .HasForeignKey("CargoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("cargo_feeds_cargo_id_fk");

                    b.Navigation("Cargo");
                });

            modelBuilder.Entity("RSSCargo.DAL.Models.UserCargo", b =>
                {
                    b.HasOne("RSSCargo.DAL.Models.Cargo", "Cargo")
                        .WithMany("UserCargos")
                        .HasForeignKey("CargoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("user_cargos_cargo_id_fk");

                    b.HasOne("RSSCargo.DAL.Models.User", "User")
                        .WithMany("UserCargos")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("user_cargos_user_id_fk");

                    b.Navigation("Cargo");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RSSCargo.DAL.Models.UserFeed", b =>
                {
                    b.HasOne("RSSCargo.DAL.Models.User", "User")
                        .WithMany("UserFeeds")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("user_cargos_user_id_fk");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RSSCargo.DAL.Models.Cargo", b =>
                {
                    b.Navigation("CargoFeeds");

                    b.Navigation("UserCargos");
                });

            modelBuilder.Entity("RSSCargo.DAL.Models.User", b =>
                {
                    b.Navigation("UserCargos");

                    b.Navigation("UserFeeds");
                });
#pragma warning restore 612, 618
        }
    }
}