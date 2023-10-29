using AutoMapper;
using FluentValidation;
using MagicVilla_CoupounAPI;
using MagicVilla_CoupounAPI.Data;
using MagicVilla_CoupounAPI.Models;
using MagicVilla_CoupounAPI.Models.DTO;
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
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/coupon", (ApplicationDbContext _db, ILogger<Program> logger) => 
{
    APIResponse response = new APIResponse();
    logger.Log(LogLevel.Information,"Getting all Coupons");
    response.Result = _db.Coupons;
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.OK;

    return Results.Ok(response);
}).WithName("GetCoupons").Produces<APIResponse>(200);

app.MapGet("/api/coupon/{id:int}", async (ApplicationDbContext _db, int id) =>
{
    APIResponse response = new APIResponse();
    response.Result = await _db.Coupons.FirstOrDefaultAsync(x => x.Id == id);
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.OK;
    return Results.Ok(response.Result);
}).WithName("GetCoupon").Produces<APIResponse>(201);

app.MapPost("api/coupon", async (ApplicationDbContext _db, IMapper _mapper, 
    IValidator <CouponCreateDTO> _validation, 
    [FromBody] CouponCreateDTO coupon_C_DTO) =>
{
    APIResponse response = new APIResponse() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };
    var validateResult = await _validation.ValidateAsync(coupon_C_DTO);

    if(!validateResult.IsValid)
    {
        response.ErrorMessages.Add(validateResult.Errors.FirstOrDefault().ToString());
        return Results.BadRequest(response);
    }
    if (_db.Coupons.FirstOrDefaultAsync(x=>x.Name.ToLower() == coupon_C_DTO.Name.ToLower()) != null)
    {
        response.ErrorMessages.Add("Coupon Name Already Exists");
        return Results.BadRequest(response);
    }

    Coupon coupon = _mapper.Map<Coupon>(coupon_C_DTO);

    _db.Coupons.Add(coupon);
    await _db.SaveChangesAsync();
    CouponDTO couponDTO = _mapper.Map<CouponDTO>(coupon);

    response.Result = couponDTO;
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.Created;
    return Results.Ok(response);
}).WithName("CreateCoupon").Accepts<CouponCreateDTO>("application/json").Produces<APIResponse>(201).Produces(400);

app.MapPut("api/coupon", async (ApplicationDbContext _db, IMapper _mapper,
     IValidator<CouponEditedDTO> _validation,
    [FromBody] CouponEditedDTO coupon_E_DTO) =>
{
    APIResponse response = new APIResponse() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };
    var validateResult = await _validation.ValidateAsync(coupon_E_DTO);
    if (!validateResult.IsValid)
    {
        response.ErrorMessages.Add(validateResult.Errors.FirstOrDefault().ToString());
        return Results.BadRequest(response);
    }


    Coupon couponToEdit = await _db.Coupons.FirstOrDefaultAsync(x => x.Id == coupon_E_DTO.Id);
    couponToEdit.Name = coupon_E_DTO.Name;
    couponToEdit.Percentage = coupon_E_DTO.Percentage;
    couponToEdit.IsActive =  coupon_E_DTO.IsActive;
    couponToEdit.LastUpdated = DateTime.Now;
   // _db.Coupons.Update(_mapper.Map<Coupon>(coupon_E_DTO));
    await _db.SaveChangesAsync();

    response.Result = _mapper.Map<CouponDTO>(couponToEdit);
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.OK;
    return Results.Ok(response);

}).WithName("EditedCoupon").Accepts<CouponEditedDTO>("application/json");


app.MapDelete("api/coupon/{id:int}", async (ApplicationDbContext _db, int id) =>
{
    APIResponse response = new APIResponse() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };
   
    Coupon couponToDelete = await _db.Coupons.FirstOrDefaultAsync(x => x.Id == id);
    if (couponToDelete != null)
    {
        _db.Coupons.Remove(couponToDelete);
        await _db.SaveChangesAsync();
        response.IsSuccess = true;
        response.StatusCode = HttpStatusCode.NoContent;
        return Results.Ok(response);
    }
    else
    {
        response.ErrorMessages.Add("Invalid ID");
        return Results.BadRequest(response);
    }

});

app.UseHttpsRedirection();


app.Run();
