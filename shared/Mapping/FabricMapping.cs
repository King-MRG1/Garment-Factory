using Database.Models;
using Shared.Dtos.FabricDtos;
using Shared.Dtos.ReportsDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Mapping
{
    public static class FabricMapping
    {
        public static ViewFabricDto ToFabricDto(this Fabric fabric)
        {
            return new ViewFabricDto
            {
                Id = fabric.Id,
                Fabric_Name = fabric.Fabric_Name,
                Metres = fabric.Metres,
                Price = fabric.Price,
                Trader_Name = fabric.Trader.Trader_Name,
                Date_Added = fabric.DateAdded,
                Last_Update = fabric.Last_Update
            };
        }
        public static Fabric ToFabric(this CreateFabricDto createFabricDto)
        {
            return new Fabric
            {
                Fabric_Name = createFabricDto.Fabric_Name,
                Metres = createFabricDto.Metres,
                Price = createFabricDto.Price,
                Trader_Id = createFabricDto.Trader_Id,
                DateAdded = DateOnly.FromDateTime(DateTime.Now),
                Last_Update = DateOnly.FromDateTime(DateTime.Now)
            };
        }
        public static void UpdateFabric(this Fabric fabric, UpdateFabricDto updateFabricDto)
        {
            fabric.Fabric_Name = updateFabricDto.Fabric_Name;
            fabric.Metres = updateFabricDto.Metres;
            fabric.Price = updateFabricDto.Price;
            fabric.Trader_Id = updateFabricDto.Trader_Id;
            fabric.Last_Update = DateOnly.FromDateTime(DateTime.Now);
        }
        public static ViewFabricReportDto ToFabricReportDto(this Fabric fabric)
        {
            return new ViewFabricReportDto
            {
                FabricName = fabric.Fabric_Name,
                TotalMeters = fabric.Metres,
                TotalPrice = fabric.Price
            };

        }
    }
}
