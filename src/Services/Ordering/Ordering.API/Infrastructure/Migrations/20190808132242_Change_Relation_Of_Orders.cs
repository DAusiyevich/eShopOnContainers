﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Ordering.API.Infrastructure.Migrations
{
    public partial class Change_Relation_Of_Orders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CouponCode",
                schema: "ordering",
                table: "orders",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CouponValue",
                schema: "ordering",
                table: "orders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PointsUsed",
                schema: "ordering",
                table: "orders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Discount",
                schema: "ordering",
                table: "orders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CouponCode",
                schema: "ordering",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "CouponValue",
                schema: "ordering",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "PointsUsed",
                schema: "ordering",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "Discount",
                schema: "ordering",
                table: "orders");
        }
    }
}
