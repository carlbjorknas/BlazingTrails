﻿using Ardalis.ApiEndpoints;
using BlazingTrails.Api.Persistence;
using BlazingTrails.Shared.Features.ManageTrails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BlazingTrails.Api.Features.ManageTrails
{
    public class UploadTrailImageEndpoint : BaseAsyncEndpoint
        .WithRequest<int>
        .WithResponse<bool>
    {
        private readonly BlazingTrailsContext _database;

        public UploadTrailImageEndpoint(BlazingTrailsContext database)
        {
            _database = database;
        }

        [HttpPost(UploadTrailImageRequest.RouteTemplate)]
        public override async Task<ActionResult<bool>> HandleAsync([FromRoute] int trailId, CancellationToken cancellationToken = default)
        {
            var trail = await _database.Trails.SingleOrDefaultAsync(_ => _.Id == trailId, cancellationToken);
            if (trail is null)
            {
                return BadRequest("Trail does not exist.");
            }

            var file = Request.Form.Files[0];
            if (file.Length == 0)
            {
                return BadRequest("No image found.");
            }

            var filename = $"{Guid.NewGuid()}.jpg";
            var saveLocation = Path.Combine(Directory.GetCurrentDirectory(), "Images", filename);

            var resizeOptions = new ResizeOptions
            {
                Mode = ResizeMode.Pad,
                Size = new Size(640, 426)
            };

            using var image = Image.Load(file.OpenReadStream());
            image.Mutate(_ => _.Resize(resizeOptions));
            await image.SaveAsJpegAsync(saveLocation, cancellationToken: cancellationToken);

            trail.Image = filename;
            await _database.SaveChangesAsync(cancellationToken);

            return Ok(true);
        }
    }
}
