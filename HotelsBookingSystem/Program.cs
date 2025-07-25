
using HotelsBookingSystem.Context;
using HotelsBookingSystem.Profile;
using HotelsBookingSystem.Service;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;


namespace HotelsBookingSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<HotelDbContext>(opt => opt.UseInMemoryDatabase("RoomsTestDB"));
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<IHotelsService, HotelsService>();
            builder.Services.AddScoped<IRoomsServices, RoomsServices>();
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddAutoMapper(typeof(BookingProfile));
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
