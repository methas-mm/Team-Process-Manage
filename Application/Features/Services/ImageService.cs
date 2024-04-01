using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Services
{
    public class ImageService : IService
    {
        private readonly ICleanDbContext _context;
        public ImageService(ICleanDbContext context)
        {
            _context = context;
        }
    }
}
