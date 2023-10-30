using AutoMapper;
using FluentValidation;
using MagicVilla_CoupounAPI.Models.DTO;
using MagicVilla_CoupounAPI.Models;
using MagicVilla_CoupounAPI.Repository.IRepository;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace MagicVilla_CoupounAPI.EndPoints
{
    public static class CouponEndpoints
    {
        public static void ConfigureCouponEndpoints(this WebApplication app)
        {
            app.MapGet("/api/coupon", GetAllCoupon).WithName("GetCoupons").Produces<APIResponse>(200);

            app.MapGet("/api/coupon/{id:int}", GetCoupon).WithName("GetCoupon").Produces<APIResponse>(201);

            app.MapPost("api/coupon", CreateCoupn).WithName("CreateCoupon").Accepts<CouponCreateDTO>("application/json").Produces<APIResponse>(201).Produces(400);

            app.MapPut("api/coupon", UpdateCoupon).WithName("EditedCoupon").Accepts<CouponEditedDTO>("application/json");

            app.MapDelete("api/coupon/{id:int}", DeleteCoupon);

        }

        private async static Task<IResult> GetAllCoupon(ICouponResopsitory couponResopsitory, ILogger<Program> logger)
        {
            APIResponse response = new APIResponse();
            logger.Log(LogLevel.Information, "Getting all Coupons");
            response.Result = await couponResopsitory.GetAllAsync();
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;

            return Results.Ok(response);
        }

        private async static Task<IResult> DeleteCoupon(ICouponResopsitory couponResopsitory, int id)
        {
            APIResponse response = new APIResponse() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };

            Coupon couponToDelete = await couponResopsitory.GetAsync(id);
            if (couponToDelete != null)
            {
                await couponResopsitory.RemoveAsync(couponToDelete);
                await couponResopsitory.SaveAsync();
                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.NoContent;
                return Results.Ok(response);
            }
            else
            {
                response.ErrorMessages.Add("Invalid ID");
                return Results.BadRequest(response);
            }

        }
        private async static Task<IResult> UpdateCoupon(ICouponResopsitory couponResopsitory, IMapper _mapper,
                 IValidator<CouponEditedDTO> _validation,
                [FromBody] CouponEditedDTO coupon_E_DTO)
        {
            APIResponse response = new APIResponse() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };
            var validateResult = await _validation.ValidateAsync(coupon_E_DTO);
            if (!validateResult.IsValid)
            {
                response.ErrorMessages.Add(validateResult.Errors.FirstOrDefault().ToString());
                return Results.BadRequest(response);
            }


            await couponResopsitory.UpdateAsync(_mapper.Map<Coupon>(coupon_E_DTO));
            await couponResopsitory.SaveAsync();

            response.Result = _mapper.Map<CouponDTO>(await couponResopsitory.GetAsync(coupon_E_DTO.Id));
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            return Results.Ok(response);

        }
        private async static Task<IResult> CreateCoupn(ICouponResopsitory couponResopsitory, IMapper _mapper,
                IValidator<CouponCreateDTO> _validation,
                [FromBody] CouponCreateDTO coupon_C_DTO)
        {
            APIResponse response = new APIResponse() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };
            var validateResult = await _validation.ValidateAsync(coupon_C_DTO);

            if (!validateResult.IsValid)
            {
                response.ErrorMessages.Add(validateResult.Errors.FirstOrDefault().ToString());
                return Results.BadRequest(response);
            }
            if (await couponResopsitory.GetAsync(coupon_C_DTO.Name) != null)
            {
                response.ErrorMessages.Add("Coupon Name Already Exists");
                return Results.BadRequest(response);
            }

            Coupon coupon = _mapper.Map<Coupon>(coupon_C_DTO);

            await couponResopsitory.CreateAsync(coupon);
            await couponResopsitory.SaveAsync();
            CouponDTO couponDTO = _mapper.Map<CouponDTO>(coupon);

            response.Result = couponDTO;
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.Created;
            return Results.Ok(response);
        }
        private async static Task<IResult> GetCoupon(ICouponResopsitory couponResopsitory, int id)
        {
            APIResponse response = new APIResponse();
            response.Result = await couponResopsitory.GetAsync(id);
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            return Results.Ok(response.Result);
        }
    }
}
