using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DoctorLoan.Infrastructure.Migrations;

/// <inheritdoc />
public partial class AddEntityName : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "MedicalRecord",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                MedicalRecordNo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                CustomerId = table.Column<int>(type: "integer", nullable: false),
                DateCreated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                Status = table.Column<int>(type: "integer", nullable: false),
                OtherMedicalHistory = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                ParentId = table.Column<int>(type: "integer", nullable: false),
                IsDelete = table.Column<bool>(type: "boolean", nullable: false),
                Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                CreatedBy = table.Column<int>(type: "integer", nullable: false),
                LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                LastModifiedBy = table.Column<int>(type: "integer", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MedicalRecord", x => x.Id);
                table.ForeignKey(
                    name: "FK_MedicalRecord_Customers_CustomerId",
                    column: x => x.CustomerId,
                    principalTable: "Customers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "MedicalRecordsCategory",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                Code = table.Column<string>(type: "text", nullable: true),
                ParentId = table.Column<string>(type: "ltree", nullable: false),
                MetaTitle = table.Column<string>(type: "text", nullable: true),
                Content = table.Column<string>(type: "text", nullable: true),
                Sort = table.Column<int>(type: "integer", nullable: false),
                Slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                Status = table.Column<int>(type: "integer", nullable: false),
                IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                CreatedBy = table.Column<int>(type: "integer", nullable: false),
                LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                LastModifiedBy = table.Column<int>(type: "integer", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MedicalRecordsCategory", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "SymptomGroups",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Name = table.Column<string>(type: "character varying(70)", maxLength: 70, nullable: false),
                IsDelete = table.Column<bool>(type: "boolean", nullable: false),
                Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                CreatedBy = table.Column<int>(type: "integer", nullable: false),
                LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                LastModifiedBy = table.Column<int>(type: "integer", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SymptomGroups", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "MedicalRecordMedia",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                MediaId = table.Column<long>(type: "bigint", nullable: false),
                MedicalRecordId = table.Column<int>(type: "integer", nullable: false),
                OrderBy = table.Column<int>(type: "integer", nullable: false),
                Status = table.Column<int>(type: "integer", nullable: false),
                IsDelete = table.Column<bool>(type: "boolean", nullable: false),
                Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                CreatedBy = table.Column<int>(type: "integer", nullable: false),
                LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                LastModifiedBy = table.Column<int>(type: "integer", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MedicalRecordMedia", x => x.Id);
                table.ForeignKey(
                    name: "FK_MedicalRecordMedia_Medias_MediaId",
                    column: x => x.MediaId,
                    principalTable: "Medias",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_MedicalRecordMedia_MedicalRecord_MedicalRecordId",
                    column: x => x.MedicalRecordId,
                    principalTable: "MedicalRecord",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "MedicalRecordsCategoryMapping",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                MedicalRecordId = table.Column<int>(type: "integer", nullable: false),
                MedicalRecordsCategoryId = table.Column<int>(type: "integer", nullable: false),
                Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                CreatedBy = table.Column<int>(type: "integer", nullable: false),
                LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                LastModifiedBy = table.Column<int>(type: "integer", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MedicalRecordsCategoryMapping", x => x.Id);
                table.ForeignKey(
                    name: "FK_MedicalRecordsCategoryMapping_MedicalRecord_MedicalRecordId",
                    column: x => x.MedicalRecordId,
                    principalTable: "MedicalRecord",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_MedicalRecordsCategoryMapping_MedicalRecordsCategory_Medica~",
                    column: x => x.MedicalRecordsCategoryId,
                    principalTable: "MedicalRecordsCategory",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Symptoms",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Name = table.Column<string>(type: "character varying(70)", maxLength: 70, nullable: false),
                SymptomGroupId = table.Column<int>(type: "integer", nullable: false),
                parentId = table.Column<int>(type: "integer", nullable: false),
                IsDelete = table.Column<bool>(type: "boolean", nullable: false),
                Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                CreatedBy = table.Column<int>(type: "integer", nullable: false),
                LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                LastModifiedBy = table.Column<int>(type: "integer", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Symptoms", x => x.Id);
                table.ForeignKey(
                    name: "FK_Symptoms_SymptomGroups_SymptomGroupId",
                    column: x => x.SymptomGroupId,
                    principalTable: "SymptomGroups",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "MedicalRecordsSymptoms",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                MedicalRecordId = table.Column<int>(type: "integer", nullable: true),
                SymptomId = table.Column<int>(type: "integer", nullable: true),
                Value = table.Column<string>(type: "text", nullable: true),
                Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                CreatedBy = table.Column<int>(type: "integer", nullable: false),
                LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                LastModifiedBy = table.Column<int>(type: "integer", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MedicalRecordsSymptoms", x => x.Id);
                table.ForeignKey(
                    name: "FK_MedicalRecordsSymptoms_MedicalRecord_MedicalRecordId",
                    column: x => x.MedicalRecordId,
                    principalTable: "MedicalRecord",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_MedicalRecordsSymptoms_Symptoms_SymptomId",
                    column: x => x.SymptomId,
                    principalTable: "Symptoms",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_MedicalRecord_CustomerId",
            table: "MedicalRecord",
            column: "CustomerId");

        migrationBuilder.CreateIndex(
            name: "IX_MedicalRecord_MedicalRecordNo",
            table: "MedicalRecord",
            column: "MedicalRecordNo",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_MedicalRecordMedia_MediaId",
            table: "MedicalRecordMedia",
            column: "MediaId");

        migrationBuilder.CreateIndex(
            name: "IX_MedicalRecordMedia_MedicalRecordId",
            table: "MedicalRecordMedia",
            column: "MedicalRecordId");

        migrationBuilder.CreateIndex(
            name: "IX_MedicalRecordsCategoryMapping_MedicalRecordId",
            table: "MedicalRecordsCategoryMapping",
            column: "MedicalRecordId");

        migrationBuilder.CreateIndex(
            name: "IX_MedicalRecordsCategoryMapping_MedicalRecordsCategoryId",
            table: "MedicalRecordsCategoryMapping",
            column: "MedicalRecordsCategoryId");

        migrationBuilder.CreateIndex(
            name: "IX_MedicalRecordsSymptoms_MedicalRecordId",
            table: "MedicalRecordsSymptoms",
            column: "MedicalRecordId");

        migrationBuilder.CreateIndex(
            name: "IX_MedicalRecordsSymptoms_SymptomId",
            table: "MedicalRecordsSymptoms",
            column: "SymptomId");

        migrationBuilder.CreateIndex(
            name: "IX_Symptoms_SymptomGroupId",
            table: "Symptoms",
            column: "SymptomGroupId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "MedicalRecordMedia");

        migrationBuilder.DropTable(
            name: "MedicalRecordsCategoryMapping");

        migrationBuilder.DropTable(
            name: "MedicalRecordsSymptoms");

        migrationBuilder.DropTable(
            name: "MedicalRecordsCategory");

        migrationBuilder.DropTable(
            name: "MedicalRecord");

        migrationBuilder.DropTable(
            name: "Symptoms");

        migrationBuilder.DropTable(
            name: "SymptomGroups");
    }
}