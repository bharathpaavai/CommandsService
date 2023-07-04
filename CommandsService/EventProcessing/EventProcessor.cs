using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using System.Linq.Expressions;
using System.Text.Json;

namespace CommandsService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, AutoMapper.IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }
        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType) {
                case EventType.platformPublished:
                    AddPlatform(message);
                    break;
                default:
                    break;
            
            }


        }

        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("--> Determine Events");

            var eventtype = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

            switch(eventtype.Event)
            {
                case "platform Published":
                    Console.WriteLine("Platform Published Event is detected");
                    return EventType.platformPublished;

                default:
                    Console.WriteLine("Platform Published Event is not determined");
                    return EventType.undetermined;
            }
        }

        private void AddPlatform(string platformPublishedMessage)
        {

            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

                var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);

                try
                {
                    var platform = _mapper.Map<Platform>(platformPublishedDto);
                    if (!repo.ExternalPatformExists(platform.ExternalId))
                    {
                        repo.CreatePlatform(platform);
                        repo.SaveChanges();
                    }
                    else
                    {
                        Console.WriteLine("platformId already exists");
                    }

                }
                catch(Exception ex)
                {
                    Console.WriteLine($"--> Could not add platform to DB {ex.Message}");
                }

            }
        }
    }


    

    enum EventType
    {
        platformPublished,
        undetermined

    }
}
