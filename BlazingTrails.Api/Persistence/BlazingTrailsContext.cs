using BlazingTrails.Api.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazingTrails.Api.Persistence
{
    public class BlazingTrailsContext : DbContext
    {
        public DbSet<Trail> Trails { get; set; }
        public DbSet<RouteInstruction> RouteInstructions { get; set; }

        public BlazingTrailsContext(DbContextOptions<BlazingTrailsContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new TrailConfig());
            modelBuilder.ApplyConfiguration(new RouteInstructionConfig());
        }
    }
}
