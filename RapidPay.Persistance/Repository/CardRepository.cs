using Microsoft.EntityFrameworkCore;
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
    public class CardRepository : BaseRepository<Card>, ICardRepository
    {
        public CardRepository(AppDbContext context) : base(context) { }

        public async Task<Card> GetByCardNumberAsync(string cardNumber)
        {
            return await _context.Cards.Include(c => c.Transactions).FirstOrDefaultAsync(c => c.CardNumber == cardNumber);
        }
    }
}
