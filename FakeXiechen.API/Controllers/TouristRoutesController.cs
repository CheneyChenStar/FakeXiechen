using FakeXiechen.API.DTOs;
using FakeXiechen.API.Servers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FakeXiechen.API.Models;
using System.Text.RegularExpressions;
using FakeXiechen.API.ResourceParameters;
using Microsoft.AspNetCore.JsonPatch;
using FakeXiecheng.API.Helper;
using Microsoft.AspNetCore.Authorization;
using FakeXiechen.API.Helper;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Net.Http.Headers;

namespace FakeXiechen.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TouristRoutesController : ControllerBase
    {
        private readonly IPropertyMappingService _propertyMapping;
        private readonly ITouristRouteRepository _touristRoutesRepository;
        private readonly IMapper _mapper;
        private readonly IUrlHelper _urlHelper;

        public TouristRoutesController(
            ITouristRouteRepository touristRoutesRepository,
            IMapper mapper,
            IActionContextAccessor actionContextAccessor,
            IUrlHelperFactory urlHelperFactory,
            IPropertyMappingService propertyMapping
            )
        {
            _propertyMapping = propertyMapping;
            _touristRoutesRepository = touristRoutesRepository;
            _mapper = mapper;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);


        }

        private string GenerateTouristRouteResourceURL(
            TouristRouteResourceParameters touristRouteResource,
            PaginationResourceParameters paginationResource,
            ResourceUrlType type
            )
        {
            return type switch
            {
                ResourceUrlType.PreviousPage => _urlHelper.Link("GetTouristRoutes",
                new
                {
                    fields = touristRouteResource.Fields,
                    OrderBy =  touristRouteResource.OrderBy,
                    keyword = touristRouteResource.Keyword,
                    rating = touristRouteResource.Ratingg,
                    pageNumber = paginationResource.PageNumber - 1,
                    pageSize = paginationResource.PageSize
                }),
                ResourceUrlType.NextPage => _urlHelper.Link("GetTouristRoutes",
                new
                {
                    fields = touristRouteResource.Fields,
                    OrderBy = touristRouteResource.OrderBy,
                    keyword = touristRouteResource.Keyword,
                    rating = touristRouteResource.Ratingg,
                    pageNumber = paginationResource.PageNumber + 1,
                    pageSize = paginationResource.PageSize
                }),
                _ => _urlHelper.Link("GetTouristRoutes",
                new
                {
                    fields = touristRouteResource.Fields,
                    OrderBy = touristRouteResource.OrderBy,
                    keyword = touristRouteResource.Keyword,
                    rating = touristRouteResource.Ratingg,
                    pageNumber = paginationResource.PageNumber,
                    pageSize = paginationResource.PageSize
                }
                )
            };
        }

        private IEnumerable<LinkDto> CreateLinkForSingleTouristRoute(Guid touristRouteId, string fields)
        {
            var links = new List<LinkDto>();

            links.Add(
                new LinkDto(
                        Url.Link("GetTouristRouteById", new { touristRouteId, fields }),
                        "self",
                        "GET" 
                    )
                );
            //全局更新
            links.Add(
                new LinkDto(
                    Url.Link("UpdateTouristRoute", new {touristRouteId}),
                    "update",
                    "PUT"
                    )
                );
            //局部更新
            links.Add(
                new LinkDto(
                    Url.Link("LocallyUpdateTouristRoute", new { touristRouteId }),
                    "Locally update",
                    "PATCH"
                    )
                );
            //删除
            links.Add(
                new LinkDto(
                    
                    Url.Link("DeleteTouritstRoute", new { touristRouteId }),
                    "delete",
                    "DELETE"
                    )
                );
            //获取路线图片
            links.Add(
                new LinkDto(
                    Url.Link("GetTouristRoutePicturesByRouteId", new {touristRouteId}),
                    "get pictures",
                    "GET"
                    )
                );

            // 添加新图片
            links.Add(
                new LinkDto(
                    Url.Link("CreateTouristRoutePicture", new {touristRouteId}),
                    "create picture",
                    "POST"
                    )
                );

            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForTouristRouteList(
            TouristRouteResourceParameters routeResourceParameters,
            PaginationResourceParameters paginationResourceParameters
            )
        {
            var links = new List<LinkDto>();

            //添加self，自我链接
            links.Add(new LinkDto(
                    GenerateTouristRouteResourceURL(
                      routeResourceParameters, paginationResourceParameters, ResourceUrlType.CurrentPage),
                    "self",
                    "GET"
                ));

            // "api/touristRoutes
            // POST
            links.Add(new LinkDto(
                    Url.Link("CreateTouristRoute", null),
                    "create_tourist_route",
                    "POST"
                ));

            return links;

        }

        //1.application/json
        //2.application/vnd.cxing.hateoas+json  改自定义媒体类型需要在在Sartup类中的ConfigureServices中注册
        [HttpGet(Name = "GetTouristRoutes")]
        [HttpHead]
        public async Task<IActionResult> GetTouristRoutes(
            [FromQuery] TouristRouteResourceParameters touristRouteResource,   //  会将url参数自动映射至parameters对象中相同的字段，例如keyword=>parameters.Keyword，不区分大小写
            [FromQuery] PaginationResourceParameters paginationResource,
            [FromHeader(Name = "Accept")] string mediaType  //请求的媒体头中可能不止一个Accept，是个坑
            )
        {
            if (!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue parseMediaType))
            {
                return BadRequest();
            }
            if (!_propertyMapping.IsMappingExits<TouristRouteDto,TouristRoute>(touristRouteResource.OrderBy))
            {
                return BadRequest("排序参数错误");
            }

            if (!_propertyMapping.IsPropertiesExists<TouristRouteDto>(touristRouteResource.Fields))
            {
                return BadRequest("非法的塑形参数");
            }

            var touristRoutesFromRepo =  await _touristRoutesRepository.GetTouristRoutesAsync(
                touristRouteResource.Keyword,
                touristRouteResource.OperatorType,
                touristRouteResource.RatingValue,
                paginationResource.PageSize,
                paginationResource.PageNumber,
                touristRouteResource.OrderBy
                );

            if (touristRoutesFromRepo == null || touristRoutesFromRepo.Count() <= 0)
            {
                return NotFound("没有旅游路线");
            }
            var touristRouteDtos = _mapper.Map<IEnumerable<TouristRouteDto>>(touristRoutesFromRepo);

            var nextPageLink = touristRoutesFromRepo.HasNext
                ? GenerateTouristRouteResourceURL(touristRouteResource, paginationResource, ResourceUrlType.NextPage)
                : null;
            var privousPageLink = touristRoutesFromRepo.HasPrevious
                ? GenerateTouristRouteResourceURL(touristRouteResource, paginationResource, ResourceUrlType.PreviousPage)
                : null; 

            // x-pagination
            var paginationMetadata = new
            {
                privousPageLink,
                nextPageLink,
                totalCount = touristRoutesFromRepo.TotalCount,
                pageSize = touristRoutesFromRepo.PageSize,
                currentPage = touristRoutesFromRepo.CurrentPage,
                totalPages = touristRoutesFromRepo.TotalPages
            };

            Response.Headers.Add("x-pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));


            var shapedDtoList = touristRouteDtos.ShapeData(touristRouteResource.Fields);
            
            if (parseMediaType.MediaType == "application/vnd.cxing.hateoas+json")
            {
                var linkDto = CreateLinksForTouristRouteList(touristRouteResource, paginationResource);

                var shapedDtoWithLinkList = shapedDtoList.Select(t =>
                {
                    var touristRouteDictionary = t as IDictionary<string, object>;
                    var links = CreateLinkForSingleTouristRoute((Guid)touristRouteDictionary["Id"], null);
                    touristRouteDictionary.Add("links", links);
                    return touristRouteDictionary;
                });

                var result = new
                {
                    value = shapedDtoWithLinkList,
                    links = linkDto
                };

                return Ok(result);
            }

            return Ok(shapedDtoList);
        }

        [HttpGet("{touristRouteId}", Name = "GetTouristRouteById")]
        [HttpHead("{touristRouteId}")]
        public async Task<IActionResult> GetTouristRouteByIdAsync(Guid touristRouteId, string fields)
        {
            if (!_propertyMapping.IsPropertiesExists<TouristRouteDto>(fields))
            {
                return BadRequest("非法的塑形参数");
            }
            var touristRouteFromRepo = await _touristRoutesRepository.GetTouristRouteAsync(touristRouteId);
            if (touristRouteFromRepo == null)
            {
                return NotFound($"Id为[{touristRouteId}]的旅游路线未找到!");
            }

            var touristRouteDto = _mapper.Map<TouristRouteDto>(touristRouteFromRepo);//将实体映射至TouristRouteDto类型的实体并返回

            //return  Ok(touristRouteDto.ShapeData(fields));

            var linksDto = CreateLinkForSingleTouristRoute(touristRouteId, fields);

            var result = touristRouteDto.ShapeData(fields) as IDictionary<string, object>;
            result.Add("links", linksDto);

            return Ok(result);
        }

        [HttpPost(Name = "CreateTouristRoute")]
        [Authorize( AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CreateTouristRouteAsync(
            [FromBody] TouristRouteForCreationDto creationDto
        )
        {
            var touristRouteModel = _mapper.Map<TouristRoute>(creationDto);
            _touristRoutesRepository.AddTouristRoute(touristRouteModel);
            await _touristRoutesRepository.SaveAsync();
            var touristRouteDtoToReturn = _mapper.Map<TouristRouteDto>(touristRouteModel);

            var links = CreateLinkForSingleTouristRoute(touristRouteModel.Id, null);

            var result = touristRouteDtoToReturn.ShapeData(null) as IDictionary<string, object>;
            result.Add("links", links);

            return CreatedAtRoute(
                "GetTouristRouteById",
                new { touristRouteId = result["Id"] },
                result //post请求成功后输出的数据
                );
        }

        [HttpPut("{touristRouteId}", Name = "UpdateTouristRoute")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateTouristRouteAsync(
            [FromRoute] Guid touristRouteId,
            [FromBody] TouristRouteForUpdateDto touristRouteForUpdateDto
            )
        {
            if ( ! await _touristRoutesRepository.IsExitsForTouristRouteAsync(touristRouteId))
            {
                return NotFound("未找到对应的旅游路线，更新失败！");
            }
            var touristRouteModel = await _touristRoutesRepository.GetTouristRouteAsync(touristRouteId);
            _mapper.Map(touristRouteForUpdateDto, touristRouteModel);

            await _touristRoutesRepository.SaveAsync();

            return NoContent();
        }

        [HttpPatch("{touristRouteId}", Name = "LocallyUpdateTouristRoute")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> LocallyUpdateTouristRouteAsync(
            [FromRoute] Guid touristRouteId,
            [FromBody] JsonPatchDocument<TouristRouteForUpdateDto> jsonPatchDocument
            )
        {
            if (!await _touristRoutesRepository.IsExitsForTouristRouteAsync(touristRouteId))
            {
                return NotFound("未找到对应的旅游路线，更新失败！");
            }
            var touristRouteModel = await _touristRoutesRepository.GetTouristRouteAsync(touristRouteId);
            var touristPatch = _mapper.Map<TouristRouteForUpdateDto>(touristRouteModel);
            jsonPatchDocument.ApplyTo(touristPatch, ModelState);

            if (!TryValidateModel(touristPatch))    //启用对象TouristRouteForUpdateDto具有的Attribute验证，成功返回True，否则False
            {
                return ValidationProblem(ModelState);
                //return BadRequest();
            }

            _mapper.Map(touristPatch, touristRouteModel);
            await _touristRoutesRepository.SaveAsync();

            return NoContent();
        }

        [HttpDelete("{touristRouteId}", Name = "DeleteTouritstRoute")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteTouritstRouteAsync([FromRoute] Guid touristRouteId)
        {
            if (! await _touristRoutesRepository.IsExitsForTouristRouteAsync(touristRouteId))
            {
                return NotFound("未找到对应的旅游路线，更新失败！");
            }
            var touristRouteModel = await _touristRoutesRepository.GetTouristRouteAsync(touristRouteId);
            _touristRoutesRepository.DeleteTouristRoute(touristRouteModel);
            await _touristRoutesRepository.SaveAsync();

            return NoContent();  
        }
        
        [HttpDelete("({touristRouteIds})")] //列表参数需要用()
        [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteTouristRoutesByIds(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))]     
            [FromRoute] 
            IEnumerable<Guid> touristRouteIds
            )
        {
            if (touristRouteIds == null)
            {
                return BadRequest();
            }
            IEnumerable<TouristRoute> touristRoutesToRemoveFromRepo = await _touristRoutesRepository.GetTouristRoutesByIdsAsync(touristRouteIds);
            _touristRoutesRepository.DeleteTouristRoutesByListModel(touristRoutesToRemoveFromRepo);
            await _touristRoutesRepository.SaveAsync();

            return NoContent();
        }

    }
}
