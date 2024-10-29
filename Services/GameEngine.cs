using Microsoft.EntityFrameworkCore;
using W9_assignment_template.Data;
using W9_assignment_template.Models;

namespace W9_assignment_template.Services;

public class GameEngine
{
    private readonly GameContext _context;

    public GameEngine(GameContext context)
    {
        _context = context;
    }

    public void DisplayRooms()
    {
        var rooms = _context.Rooms.Include(r => r.Characters).ToList();

        foreach (var room in rooms)
        {
            Console.WriteLine($"Room: {room.Name} - {room.Description}");
            foreach (var character in room.Characters)
            {
                Console.WriteLine($"    Character: {character.Name}, Level: {character.Level}");
            }
        }
    }

    public void DisplayCharacters()
    {
        var characters = _context.Characters.ToList();
        if (characters.Any())
        {
            Console.WriteLine("\nCharacters:");
            foreach (var character in characters)
            {
                Console.WriteLine($"Character ID: {character.Id}, Name: {character.Name}, Level: {character.Level}, Room ID: {character.RoomId}");
            }
        }
        else
        {
            Console.WriteLine("No characters available.");
        }
    }

    public void AddRoom()
    {
        Console.Write("Enter Room name: ");
        var name = Console.ReadLine();

        Console.Write("Enter Room description: ");
        var description = Console.ReadLine();

        var room = new Room
        {
            Name = name,
            Description = description
        };

        _context.Rooms.Add(room);
        _context.SaveChanges();

        Console.WriteLine($"Room '{name}' added to the game.");
    }

    public void AddCharacter()
    // Add a new character to a room
    {

        Console.Write("Enter Room ID for the character to reside in: ");
        try
        {
            var roomId = int.Parse(Console.ReadLine());
            // Find the room by Id by using LINQ
            var targetroom = _context.Rooms.FirstOrDefault(obj => obj.Id == roomId);

            if (targetroom == null)
            {
                // If the room doesn't exist, return
                Console.WriteLine("Room not found.");
                return;
            }
            else
            {
                // If the room exists, create a character to add to the room
                Console.Write("Enter character name: ");
                var name = Console.ReadLine();

                Console.Write("Enter character level: ");
                try
                {
                    var level = int.Parse(Console.ReadLine());
                    var character = new Character
                    {
                        Name = name,
                        Level = level,
                        RoomId = roomId,
                        Room = targetroom
                    };

                    _context.Characters.Add(character);

                    targetroom.Characters.Add(character);
                    // Save changes to database
                    _context.SaveChanges();

                    Console.WriteLine($"Character '{name}' added to the game.");
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input: Please enter an integer for level.");
                    return;
                }
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid input: Please enter an integer for Room ID.");
            return;
        }
    }

    public void FindCharacter()
    // Find a character by name
    {

        Console.Write("Enter character name to search: ");
        var name = Console.ReadLine();

        // Find the character by name using LINQ
        var character = _context.Characters.FirstOrDefault(obj => obj.Name == name);
        if (character == null)
        {
            // If character doesn't exist, display a message indicating the character was not found
            Console.WriteLine("No character found");
            return;
        }
        else
        {
            // If the character exists, display the character's information
            Console.WriteLine($"Character ID: {character.Id}, Name: {character.Name}, Level: {character.Level}, Room ID: {character.RoomId}");
        }
    }
}