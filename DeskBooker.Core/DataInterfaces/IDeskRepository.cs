using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeskBooker.Core.Domain;

public interface IDeskRepository
{
    Task<List<Desk>> GetAvailableDesks(DateTime date);
}