using CommandsService.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace CommandsService.Data
{
    public class CommandRepo : ICommandRepo
    {
        private readonly AppDBContext _context;

        public CommandRepo(AppDBContext context)
        {
            _context = context;
        }

        public void CreateCommand(int platformId, Command command)
        {
            if(command == null)
            {
                throw new ArgumentNullException(nameof(command));               
            }
            command.PlatformId = platformId;

            _context.Commands.Add(command);
        }

        public void CreatePlatform(Platform plat)
        {
            if (plat == null)
            {
                throw new ArgumentNullException(nameof(plat));
            }
            _context.platforms.Add(plat);
        }

        public bool ExternalPatformExists(int ExternalPlatformId)
        {
            return _context.platforms.Any(p=>p.ExternalId == ExternalPlatformId);   
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.platforms.ToList();
        }

        public Command GetCommand(int platformId, int commandId)
        {
          return _context.Commands
                .Where(c=>c.PlatformId== platformId && c.Id== commandId).FirstOrDefault();
                    
        }

        public IEnumerable<Command> GetCommandsFromPlatform(int platFormId)
        {
            return _context.Commands
                   .Where(p => p.PlatformId == platFormId)
                   .OrderBy(x => x.platform.Name);
        }

        public bool PlatformExists(int platformId)
        {
            return _context.platforms.Any(p=>p.Id == platformId);
        }

        public bool SaveChanges()
        {
           return (_context.SaveChanges() >= 0);
        }
    }
}
