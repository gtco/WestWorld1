using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WestWorld1
{
    public enum Location
    {
        Shack,
        Goldmine,
        Bank,
        Saloon
    }

    public abstract class Entity
    {
        public int Id { get; set; }
        public static int NextValidId { get; set; }
        public abstract void Update();

        public Entity(int id)
        {
            SetId(id);
        }

        public void SetId(int value)
        {
            Id = value;
            NextValidId = Id + 1;
        }
    }

    public class Miner : Entity
    {
        public const int ComfortLevel = 5;
        private const int MaxNuggets = 3;
        private const int ThirstLevel = 5;
        private const int TirednessThreshold = 5;

        private State State { get; set; }
        public Location Location { get; set; }
        public int GoldCarried { get; set; }

        public int MoneyInBank { get; set; }
        public int Thirst { get; set; }
        public int Fatigue { get; set; }
        public bool PocketsFull => GoldCarried >= MaxNuggets;
        public bool Thirsty => Thirst >= ThirstLevel;
        public bool Fatigued => Fatigue >= TirednessThreshold;

        public Miner(int id) : base(id)
        {
            Location = Location.Shack;
            GoldCarried = 0;
            MoneyInBank = 0;
            Thirst = 0;
            Fatigue = 0;
            State = GoHomeAndSleepUntilRested.Instance;
        }

        public override void Update()
        {
            Thirst += 1;
            State.Execute(this);
        }

        public void ChangeState(State state)
        {
            State.Exit(this);
            State = state;
            State.Enter(this);
        }

        public void BuyAndDrinkWhiskey()
        {
            Thirst = 0;
            MoneyInBank -= 2;
        }

    }
}
