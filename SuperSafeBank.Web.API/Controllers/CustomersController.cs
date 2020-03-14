﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SuperSafeBank.Web.API.Commands;
using SuperSafeBank.Web.API.DTOs;
using SuperSafeBank.Web.API.Queries;

namespace SuperSafeBank.Web.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCustomerDto dto, CancellationToken cancellationToken = default)
        {
            if (null == dto)
                return BadRequest();
            var command = new CreateCustomer(Guid.NewGuid(), dto.FirstName, dto.LastName);
            await _mediator.Publish(command, cancellationToken);
            return CreatedAtAction("GetCustomer", new { id = command.Id }, command);
        }

        [HttpGet, Route("{id:guid}", Name = "GetCustomer")]
        public async Task<IActionResult> GetCustomer(Guid id, CancellationToken cancellationToken= default)
        {
            var query = new CustomerById(id);
            var result = await _mediator.Send(query, cancellationToken);
            if (null == result) 
                return NotFound();
            return Ok(result);
        }
    }
}