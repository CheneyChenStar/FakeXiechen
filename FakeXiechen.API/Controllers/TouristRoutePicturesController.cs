using AutoMapper;
using FakeXiechen.API.DTOs;
using FakeXiechen.API.Models;
using FakeXiechen.API.Servers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiechen.API.Controllers
{
    [Route("api/touristRoutes/{touristRouteId}/pictures")]
    [ApiController]
    public class TouristRoutePicturesController : ControllerBase
    {
        private readonly ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;

        public TouristRoutePicturesController(
            ITouristRouteRepository touristRouteRepository,
            IMapper mapper
            )
        {
            _touristRouteRepository = touristRouteRepository ?? 
                throw new ArgumentNullException(nameof(touristRouteRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet(Name = "GetTouristRoutePicturesByRouteId")]
        public async Task<IActionResult> GetTouristRoutePicturesByRouteIdAsync(Guid touristRouteId)
        {
            if (! await _touristRouteRepository.IsExitsForTouristRouteAsync(touristRouteId))
            {
                return NotFound("选定旅游路线不存在");
            }
            var pictures = await _touristRouteRepository.GetTouristRoutePicturesAsync(touristRouteId);
            if (pictures == null)
            {
                return NotFound("该路线尚未存在图片");
            }

            return Ok(_mapper.Map<IEnumerable<TouristRoutePictureDto>>(pictures));
        }

        [HttpGet("{pictureId}", Name = "GetTouristRoutePicture")]
        public async Task<IActionResult> GetTouristRoutePictureAsync(Guid touristRouteId, int pictureId) //之所以要添加Guid是为了判断父资源是否存在，通过父资源取得子资源
        {
            
            if (! await _touristRouteRepository.IsExitsForTouristRouteAsync(touristRouteId))
            {
                return NotFound("未发现该旅游路线");
            }
            var picture = await _touristRouteRepository.GetTouristRoutePictureAsync(pictureId);
            if (picture == null)
            {
                return NotFound("图片不存在");
            }
            return Ok(_mapper.Map<TouristRoutePictureDto>(picture));
        }

        [HttpPost(Name = "CreateTouristRoutePicture")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CreateTouristRoutePictureAsync(
            [FromRoute] Guid touristRouteId,
            [FromBody] TouristRoutePictureForCreationDto pictureForCreationDto
        )
        {
            if (!await _touristRouteRepository.IsExitsForTouristRouteAsync(touristRouteId))
            {
                return NotFound("旅游路线不存在");
            }
            // 映射为模型对象，并添加进数据库，同时将模型对象映射为返回的dto对象，最后返回201，并包含资源回调URL
            var pictureModel = _mapper.Map<TouristRoutePicture>(pictureForCreationDto);
            _touristRouteRepository.AddTouristRoutePicture(touristRouteId,pictureModel);
            await _touristRouteRepository.SaveAsync();
            var pictureDto = _mapper.Map<TouristRoutePictureDto>( pictureModel);

            return CreatedAtRoute(
                "GetTouristRoutePicture", //回调URL映射的API 名字
                new //回填URL API的路由 api/touristRoutes/{touristRouteId}/pictures/{pictureId}
                {
                    touristRouteId = pictureModel.TouristRouteId,
                    pictureId = pictureModel.Id
                },
                pictureDto //返回的数据
                );
        }

        [HttpDelete("{pictureId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeletePictureAsync(
            [FromRoute] Guid touristRouteId,
            [FromRoute] int pictureId
            )
        {
            if (! await _touristRouteRepository.IsExitsForTouristRouteAsync(touristRouteId))
            {
                return NotFound("旅游路线不存在");
            }

            var pictureModel = await _touristRouteRepository.GetTouristRoutePictureAsync(pictureId);
            _touristRouteRepository.DeleteTouristRoutePicture(pictureModel);
            await _touristRouteRepository.SaveAsync();

            return NoContent(); //此处后续应当包含API的自我发现

        }



    }
}
