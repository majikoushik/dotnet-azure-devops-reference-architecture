using Claims.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Claims.Api.Data.Migrations;

[DbContext(typeof(ClaimsDbContext))]
partial class ClaimsDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("ProductVersion", "10.0.9");

        modelBuilder.Entity("Claims.Api.Domain.ClaimRecord", b =>
        {
            b.Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("uniqueidentifier");

            b.Property<string>("ClaimNumber")
                .IsRequired()
                .HasMaxLength(32)
                .HasColumnType("nvarchar(32)");

            b.Property<string>("CustomerId")
                .IsRequired()
                .HasMaxLength(64)
                .HasColumnType("nvarchar(64)");

            b.Property<decimal>("EstimatedAmount")
                .HasPrecision(18, 2)
                .HasColumnType("decimal(18,2)");

            b.Property<string>("LossDescription")
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnType("nvarchar(500)");

            b.Property<string>("PolicyNumber")
                .IsRequired()
                .HasMaxLength(64)
                .HasColumnType("nvarchar(64)");

            b.Property<string>("Status")
                .IsRequired()
                .HasMaxLength(32)
                .HasColumnType("nvarchar(32)");

            b.Property<DateTimeOffset>("SubmittedAt")
                .HasColumnType("datetimeoffset");

            b.HasKey("Id");

            b.HasIndex("ClaimNumber")
                .IsUnique();

            b.ToTable("Claims");
        });
    }
}
