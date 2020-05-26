using System;
using System.Collections.Generic;
using DeskBooker.Core.Domain;

public interface IDeskRepository
{
    List<Desk> GetAvailableDesks(DateTime date);
}