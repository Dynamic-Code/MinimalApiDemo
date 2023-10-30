using AutoMapper;
using FluentValidation;
using MagicVilla_CoupounAPI;
using MagicVilla_CoupounAPI.Data;
using MagicVilla_CoupounAPI.EndPoints;
using MagicVilla_CoupounAPI.Models;
using MagicVilla_CoupounAPI.Models.DTO;
using MagicVilla_CoupounAPI.Repository;
using MagicVilla_CoupounAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Xml.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddScoped<ICouponResopsitory, CouponRepository>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureCouponEndpoints();
app.UseHttpsRedirection();


app.Run();
