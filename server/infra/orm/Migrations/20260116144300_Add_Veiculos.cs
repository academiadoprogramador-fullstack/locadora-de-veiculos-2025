using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocadoraDeVeiculos.Infraestrutura.Orm.Migrations
{
    /// <inheritdoc />
    public partial class Add_Veiculos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Veiculos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GrupoVeiculosId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Modelo = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Marca = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Ano = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Imagem = table.Column<string>(type: "nvarchar(350)", nullable: true),
                    CapacidadeTanque = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TipoCombustivel = table.Column<int>(type: "int", nullable: false),
                    Alugado = table.Column<bool>(type: "bit", nullable: false),
                    CriadoEmUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExcluidoEmUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Excluido = table.Column<bool>(type: "bit", nullable: false),
                    EmpresaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Veiculos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Veiculos_AspNetUsers_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Veiculos_GruposVeiculos_GrupoVeiculosId",
                        column: x => x.GrupoVeiculosId,
                        principalTable: "GruposVeiculos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Veiculos_EmpresaId_Excluido",
                table: "Veiculos",
                columns: new[] { "EmpresaId", "Excluido" });

            migrationBuilder.CreateIndex(
                name: "IX_Veiculos_GrupoVeiculosId",
                table: "Veiculos",
                column: "GrupoVeiculosId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Veiculos");
        }
    }
}
