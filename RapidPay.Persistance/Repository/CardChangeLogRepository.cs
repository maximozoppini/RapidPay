using RapidPay.Application.Repository;
using RapidPay.Domain.Entities;
using RapidPay.Entities.Context;
using RapidPay.Persistance.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Persistance.Repository
{
    public class CardChangeLogRepository : BaseRepository<CardChangeLog>, ICardChangeLogRepository
    {
        public CardChangeLogRepository(AppDbContext context) : base(context) { }

    }
}
