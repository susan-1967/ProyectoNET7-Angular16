using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VintageStore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SpReportes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE uspReportSales (@DateStart DATE, @DateEnd DATE)
                as
                BEGIN

	                 SELECT
		                C.nombre ProductoName,
		                SUM(S.Total) AS Total
	                FROM Venta S (NOLOCK)
	                INNER JOIN producto C (NOLOCK) ON S.productoId = C.Id
	                AND C.[Status] = 1
	                AND S.fechaventa BETWEEN @DateStart AND @DateEnd
	                GROUP BY C.nombre
	                ORDER BY 2 DESC

                END
                GO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE uspReportSales");
        }
    }
}
