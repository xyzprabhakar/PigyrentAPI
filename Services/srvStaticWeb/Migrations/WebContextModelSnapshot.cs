﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using srvStaticWeb.DB;

#nullable disable

namespace srvStaticWeb.Migrations
{
    [DbContext(typeof(WebContext))]
    partial class WebContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("srvStaticWeb.DB.tblAboutUsDetail", b =>
                {
                    b.Property<Guid>("AboutUsDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("AboutUsId")
                        .HasColumnType("char(36)");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<DateTime>("CreatedDt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Details")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("varchar(2048)");

                    b.Property<string>("Heading")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<DateTime>("ModifiedDt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.HasKey("AboutUsDetailId");

                    b.HasIndex("AboutUsId");

                    b.ToTable("tblAboutUsDetail");
                });

            modelBuilder.Entity("srvStaticWeb.DB.tblAboutUsMaster", b =>
                {
                    b.Property<Guid>("AboutUsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<DateTime>("CreatedDt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("DefaultName")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)");

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<DateTime>("ModifiedDt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("AboutUsId");

                    b.ToTable("tblAboutUsMaster");
                });

            modelBuilder.Entity("srvStaticWeb.DB.tblComplaintMaster", b =>
                {
                    b.Property<Guid>("ComplaintId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("ComplaintNo")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)");

                    b.Property<int>("ComplaintStatus")
                        .HasColumnType("int");

                    b.Property<int>("ComplaintType")
                        .HasColumnType("int");

                    b.Property<string>("ContactNo")
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<DateTime>("CreatedDt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("FilePath")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Messages")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("varchar(512)");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<DateTime>("ModifiedDt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.HasKey("ComplaintId");

                    b.ToTable("tblComplaintMaster");
                });

            modelBuilder.Entity("srvStaticWeb.DB.tblComplaintProcess", b =>
                {
                    b.Property<Guid>("ComplaintProcessId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("ComplaintId")
                        .HasColumnType("char(36)");

                    b.Property<int>("ComplaintStatus")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<DateTime>("CreatedDt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Messages")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("varchar(512)");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<DateTime>("ModifiedDt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ProcessBy")
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)");

                    b.HasKey("ComplaintProcessId");

                    b.HasIndex("ComplaintId");

                    b.ToTable("tblComplaintProcess");
                });

            modelBuilder.Entity("srvStaticWeb.DB.tblContactUs", b =>
                {
                    b.Property<Guid>("ContactUsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<DateTime>("CreatedDt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("Messages")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("varchar(512)");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<DateTime>("ModifiedDt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.HasKey("ContactUsId");

                    b.ToTable("tblContactUs");
                });

            modelBuilder.Entity("srvStaticWeb.DB.tblFAQDetail", b =>
                {
                    b.Property<Guid>("FAQDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("varchar(1024)");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<DateTime>("CreatedDt")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("FAQId")
                        .HasColumnType("char(36)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<DateTime>("ModifiedDt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Question")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("FAQDetailId");

                    b.HasIndex("FAQId");

                    b.ToTable("tblFAQDetail");
                });

            modelBuilder.Entity("srvStaticWeb.DB.tblFAQMaster", b =>
                {
                    b.Property<Guid>("FAQId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<DateTime>("CreatedDt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("DefaultQuestion")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<int>("DisplayOrder")
                        .HasMaxLength(32)
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<DateTime>("ModifiedDt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("FAQId");

                    b.ToTable("tblFAQMaster");
                });

            modelBuilder.Entity("srvStaticWeb.DB.tblJoinUsDetail", b =>
                {
                    b.Property<Guid>("JoinUsDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<DateTime>("CreatedDt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Details")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("varchar(2048)");

                    b.Property<string>("Heading")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid?>("JoinUsId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<DateTime>("ModifiedDt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.HasKey("JoinUsDetailId");

                    b.HasIndex("JoinUsId");

                    b.ToTable("tblJoinUsDetail");
                });

            modelBuilder.Entity("srvStaticWeb.DB.tblJoinUsMaster", b =>
                {
                    b.Property<Guid>("JoinUsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<DateTime>("CreatedDt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("DefaultName")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)");

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<DateTime>("ModifiedDt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("JoinUsId");

                    b.ToTable("tblJoinUsMaster");
                });

            modelBuilder.Entity("srvStaticWeb.DB.tblOffice", b =>
                {
                    b.Property<Guid>("OfficeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<DateTime>("CreatedDt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("DefaultLocation")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsHeadOffice")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<DateTime>("ModifiedDt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("OfficeId");

                    b.ToTable("tblOffice");
                });

            modelBuilder.Entity("srvStaticWeb.DB.tblOfficeDetail", b =>
                {
                    b.Property<Guid>("OfficeDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("ContactNo")
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<DateTime>("CreatedDt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("Image")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)");

                    b.Property<string>("Latitude")
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)");

                    b.Property<string>("Longitude")
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<DateTime>("ModifiedDt")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("OfficeId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)");

                    b.HasKey("OfficeDetailId");

                    b.HasIndex("OfficeId");

                    b.ToTable("tblOfficeDetail");
                });

            modelBuilder.Entity("srvStaticWeb.DB.tblAboutUsDetail", b =>
                {
                    b.HasOne("srvStaticWeb.DB.tblAboutUsMaster", "tblAboutUsMaster")
                        .WithMany("AboutUsDetail")
                        .HasForeignKey("AboutUsId");

                    b.Navigation("tblAboutUsMaster");
                });

            modelBuilder.Entity("srvStaticWeb.DB.tblComplaintProcess", b =>
                {
                    b.HasOne("srvStaticWeb.DB.tblComplaintMaster", "tblComplaintMaster")
                        .WithMany("ComplaintProcess")
                        .HasForeignKey("ComplaintId");

                    b.Navigation("tblComplaintMaster");
                });

            modelBuilder.Entity("srvStaticWeb.DB.tblFAQDetail", b =>
                {
                    b.HasOne("srvStaticWeb.DB.tblFAQMaster", "tblFAQMaster")
                        .WithMany("FAQDetail")
                        .HasForeignKey("FAQId");

                    b.Navigation("tblFAQMaster");
                });

            modelBuilder.Entity("srvStaticWeb.DB.tblJoinUsDetail", b =>
                {
                    b.HasOne("srvStaticWeb.DB.tblJoinUsMaster", "tblJoinUsMaster")
                        .WithMany("JoinUsDetail")
                        .HasForeignKey("JoinUsId");

                    b.Navigation("tblJoinUsMaster");
                });

            modelBuilder.Entity("srvStaticWeb.DB.tblOfficeDetail", b =>
                {
                    b.HasOne("srvStaticWeb.DB.tblOffice", "tblOffice")
                        .WithMany("OfficeDetail")
                        .HasForeignKey("OfficeId");

                    b.Navigation("tblOffice");
                });

            modelBuilder.Entity("srvStaticWeb.DB.tblAboutUsMaster", b =>
                {
                    b.Navigation("AboutUsDetail");
                });

            modelBuilder.Entity("srvStaticWeb.DB.tblComplaintMaster", b =>
                {
                    b.Navigation("ComplaintProcess");
                });

            modelBuilder.Entity("srvStaticWeb.DB.tblFAQMaster", b =>
                {
                    b.Navigation("FAQDetail");
                });

            modelBuilder.Entity("srvStaticWeb.DB.tblJoinUsMaster", b =>
                {
                    b.Navigation("JoinUsDetail");
                });

            modelBuilder.Entity("srvStaticWeb.DB.tblOffice", b =>
                {
                    b.Navigation("OfficeDetail");
                });
#pragma warning restore 612, 618
        }
    }
}
