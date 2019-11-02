using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniEshop.DAL;
using MiniEshop.Domain;
using MiniEshop.Domain.DTO;

namespace MiniEshop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodController
        : ControllerBase
    {
        private readonly IEshopRepository _repository;

        private readonly IMapper _mapper;

        public GoodController(IEshopRepository repository
            , IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("{categoryid}/count")]
        public Task<int> GetAsync(Guid categoryId)
        {
            return _repository.GetGoodCountAsync(categoryId);
        }

        [HttpGet("{categoryid}")]
        public async Task<GoodDTO[]> GetAsync(Guid categoryId
            , [FromQuery] int skip
            , [FromQuery] int limit)
        {
            return _mapper.Map<GoodDTO[]>(await _repository.GetGoodsAsync(categoryId, skip, limit));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]GoodDTO good)
        {
            if (ModelState.IsValid)
            {
                Good g = _mapper.Map<Good>(good);
                int countUpdate = await _repository.CreateGoodAsync(g);
                if (countUpdate == 1)
                {
                    return Ok(good);
                }
                else
                    return BadRequest("It is failed to add the good to db");
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody]GoodDTO good)
        {
            if (ModelState.IsValid)
            {
                Good g = _mapper.Map<Good>(good);
                int countUpdate = await _repository.UpdateGoodAsync(g);
                if (countUpdate == 1)
                {
                    return Ok(good);
                }
                else
                    return BadRequest("It is failed to update the current good");
            }
            return BadRequest(ModelState);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery]List<Guid> ids)
        {
            var good = await _repository.DeleteGoodAsync(ids);
            return Ok(good);
        }
    }
}