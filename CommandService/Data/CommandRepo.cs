using System;
using System.Collections.Generic;
using System.Linq;
using CommandService.Models;

namespace CommandService.Data
{
    public class CommandRepo : ICommandRepo
    {
        private readonly AppDbContext _context;

        public CommandRepo(AppDbContext context)
        {
            _context = context;
        }

        void ICommandRepo.CreateCommand(int platformId, Command command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            command.PlatformId = platformId;
            _context.Commands.Add(command);
        }

        void ICommandRepo.CreatePlatform(Platform platform)
        {
            if (platform == null)
            {
                throw new ArgumentNullException(nameof(platform));
            }

            _context.Platforms.Add(platform);
        }

        IEnumerable<Platform> ICommandRepo.GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        Command ICommandRepo.GetCommand(int platformId, int commandId)
        {
            return _context.Commands
                .SingleOrDefault(c => c.Id == commandId && c.PlatformId == platformId);

        }

        IEnumerable<Command> ICommandRepo.GetCommandsForPlatform(int platformId)
        {
            return _context.Commands
                .Where(c => c.PlatformId == platformId)
                .OrderBy(c => c.Platform.Name);
        }

        bool ICommandRepo.PlatformExists(int platformId)
        {
            return _context.Platforms.Any(c => c.Id == platformId);
        }

        bool ICommandRepo.SaveChanges()
        {
            return (_context.SaveChanges() > 0);
        }
    }
}