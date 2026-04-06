using Database.Models;
using Shared.Dtos.ModelDtos;
using Shared.Dtos.OrderDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Mapping
{
    public static class ModelMapping
    {
        public static ViewModelDto ToModelDto(this Model model)
        {
            return new ViewModelDto
            {
                Id = model.Id,
                Model_Name = model.Model_Name,
                Price_Trader_Cash = model.Price_Trader_Cash,
                Price_Trader_Rent = model.Price_Trader_Rent,
                Price_Stitcher = model.Price_Stitcher,
                Price_Iron = model.Price_Ironer,
                Price_Cutter = model.Price_Cutter,
                Total_Units = model.Total_Units,
                Last_Update = model.Last_Update,
                Orders = model.OrderModels != null 
                    ? model.OrderModels
                    .Where(om => om.Order != null)
                    .Select(om => om.Order.ToOrderDto())
                    .DistinctBy(o => o.Id)
                    .ToList()
                    : new List<ViewOrderDto>()
            };
        }
        public static Model ToModel(this CreateModelDto createModelDto)
        {
            return new Model
            {
                Model_Name = createModelDto.Model_Name,
                Price_Trader_Cash = createModelDto.Price_Trader_Cash,
                Price_Trader_Rent = createModelDto.Price_Trader_Rent,
                Price_Stitcher = createModelDto.Price_Stitcher,
                Price_Ironer = createModelDto.Price_Iron,
                Price_Cutter = createModelDto.Price_Cutter,
                Last_Update = DateOnly.FromDateTime(DateTime.Now)
            };
        }
        public static void UpdateModel(this Model model, UpdateModelDto updateModelDto)
        {
            model.Model_Name = updateModelDto.Model_Name;
            model.Price_Trader_Cash = updateModelDto.Price_Trader_Cash;
            model.Price_Trader_Rent = updateModelDto.Price_Trader_Rent;
            model.Price_Stitcher = updateModelDto.Price_Stitcher;
            model.Price_Ironer = updateModelDto.Price_Iron;
            model.Price_Cutter = updateModelDto.Price_Cutter;
            model.Last_Update = DateOnly.FromDateTime(DateTime.Now);
        }
    }
}
