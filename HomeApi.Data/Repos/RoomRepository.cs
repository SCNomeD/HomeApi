﻿using System;
using System.Linq;
using System.Threading.Tasks;
using HomeApi.Data.Models;
using HomeApi.Data.Queries;
using Microsoft.EntityFrameworkCore;

namespace HomeApi.Data.Repos
{
    /// <summary>
    /// Репозиторий для операций с объектами типа "Room" в базе
    /// </summary>
    public class RoomRepository : IRoomRepository
    {
        private readonly HomeApiContext _context;
        
        public RoomRepository (HomeApiContext context)
        {
            _context = context;
        }
        
        /// <summary>
        ///  Найти комнату по имени
        /// </summary>
        public async Task<Room> GetRoomByName(string name)
        {
            return await _context.Rooms.Where(r => r.Name == name).FirstOrDefaultAsync();
        }
        
        /// <summary>
        ///  Добавить новую комнату
        /// </summary>
        public async Task AddRoom(Room room)
        {
            var entry = _context.Entry(room);
            if (entry.State == EntityState.Detached)
                await _context.Rooms.AddAsync(room);
            
            await _context.SaveChangesAsync();
        }

        /// <summary>
        ///  Найти комнату по ID
        /// </summary>
        public async Task<Room> GetRoomById(Guid id)
        {
            return await _context.Rooms.Where(r => r.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        ///  Обновить комнату
        /// </summary>
        public async Task UpdateRoom(Room room, UpdateRoomQuery query)
        {
            if (!string.IsNullOrEmpty(query.NewName))
                room.Name = query.NewName;

            if (query.NewArea != null)
                room.Area = (int)query.NewArea;

            if (query.NewGasConnected != null)
                room.GasConnected = (bool)query.NewGasConnected;

            if (query.NewVoltage != null)
                room.Voltage = (int)query.NewVoltage;

            // Добавляем в базу 
            var entry = _context.Entry(room);
            if (entry.State == EntityState.Detached)
                _context.Rooms.Update(room);

            // Сохраняем изменения в базе 
            await _context.SaveChangesAsync();
        }
    }
}