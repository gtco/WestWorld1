using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WestWorld1
{
    public abstract class State
    {
        public abstract void Exit(Miner entity);
        public abstract void Enter(Miner entity);
        public abstract void Execute(Miner entity);
    }


    public class EnterMineAndDigForGold : State
    {
        public static EnterMineAndDigForGold Instance { get; set; } = new EnterMineAndDigForGold();

        public override void Enter(Miner entity)
        {
            if (entity.Location != Location.Goldmine)
            {
                Console.WriteLine($"{entity.Id} : Walking to the gold mine.");
                entity.Location = Location.Goldmine;
            }
        }

        public override void Execute(Miner entity)
        {
            entity.GoldCarried += 1;
            entity.Fatigue += 1;
            Console.WriteLine($"{entity.Id} : Picking up a gold nugget.");

            if (entity.PocketsFull)
            {
                entity.ChangeState(VisitBankAndDepositGold.Instance);
            }

            if (entity.Thirsty)
            {
                entity.ChangeState(QuenchThirst.Instance);
            }

        }

        public override void Exit(Miner entity)
        {
            Console.WriteLine($"{entity.Id} : Leaving the gold mine.");
        }
    }

    public class VisitBankAndDepositGold : State
    {
        public static VisitBankAndDepositGold Instance { get; set; } = new VisitBankAndDepositGold();

        public override void Enter(Miner entity)
        {
            if (entity.Location != Location.Bank)
            {
                Console.WriteLine($"{entity.Id} : Walking to the bank.");
                entity.Location = Location.Bank;
            }
        }

        public override void Execute(Miner entity)
        {
            entity.MoneyInBank += entity.GoldCarried;
            entity.GoldCarried = 0;
            Console.WriteLine($"{entity.Id} : Gold deposited, savings = {entity.MoneyInBank}.");

            if (entity.MoneyInBank >= Miner.ComfortLevel)
            {
                entity.ChangeState(GoHomeAndSleepUntilRested.Instance);
            }
            else
            {
                entity.ChangeState(EnterMineAndDigForGold.Instance);
            }
        }

        public override void Exit(Miner entity)
        {
            Console.WriteLine($"{entity.Id} : Leaving the bank.");
        }

    }

    public class GoHomeAndSleepUntilRested : State
    {
        public static GoHomeAndSleepUntilRested Instance { get; set; } = new GoHomeAndSleepUntilRested();

        public override void Enter(Miner entity)
        {
            if (entity.Location != Location.Shack)
            {
                Console.WriteLine($"{entity.Id} : Walking home.");
                entity.Location = Location.Shack;
            }
        }

        public override void Execute(Miner entity)
        {
            if (!entity.Fatigued)
            {
                Console.WriteLine($"{entity.Id} : Rested.");
                entity.ChangeState(EnterMineAndDigForGold.Instance);
            }
            else
            {
                entity.Fatigue -= 1;
                Console.WriteLine($"{entity.Id} : Sleeping.");
            }
        }

        public override void Exit(Miner entity)
        {
            Console.WriteLine($"{entity.Id} : Leaving the house.");
        }
    }

    public class QuenchThirst : State
    {
        public static QuenchThirst Instance { get; set; } = new QuenchThirst();

        public override void Enter(Miner entity)
        {
            if (entity.Location != Location.Saloon)
            {
                Console.WriteLine($"{entity.Id} : Walking to the saloon.");
                entity.Location = Location.Saloon;
            }
        }

        public override void Execute(Miner entity)
        {
            if (entity.Thirsty)
            {
                entity.BuyAndDrinkWhiskey();
                Console.WriteLine($"{entity.Id} : Drinking Whiskey.");

                entity.ChangeState(EnterMineAndDigForGold.Instance);
            }
            else
            {
                Console.WriteLine("ERROR - INVALID STATE");                
            }
        }

        public override void Exit(Miner entity)
        {
            Console.WriteLine($"{entity.Id} : Leaving the saloon.");
        }
    }

}
