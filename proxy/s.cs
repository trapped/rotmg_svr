using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer
{
    enum StatsType : byte
    {
        MaximumHP = 0,
        HP = 1,
        Size = 2,
        MaximumMP = 3,
        MP = 4,
        ExperienceGoal = 5,
        Experience = 6,
        Level = 7,
        Inventory0 = 8,
        Inventory1 = 9,
        Inventory2 = 10,
        Inventory3 = 11,
        Inventory4 = 12,
        Inventory5 = 13,
        Inventory6 = 14,
        Inventory7 = 15,
        Inventory8 = 16,
        Inventory9 = 17,
        Inventory10 = 18,
        Inventory11 = 19,
        Attack = 20,
        Defense = 21,
        Speed = 22,
        Vitality = 26,
        Wisdom = 27,
        Dexterity = 28,
        Effects = 29,
        Stars = 30,
        Name = 31,
        Texture1 = 32,
        Texture2 = 33,
        MerchantMerchandiseType = 34,
        Credits = 35,
        SellablePrice = 36,
        PortalLocked = 37,
        AccountId = 38,
        CurrentFame = 39,
        SellablePriceCurrency = 40,
        ObjectConnection = 41,
        /*
         * Mask :F0F0F0F0
         * each byte -> type
         * 0:Dot
         * 1:ShortLine
         * 2:L
         * 3:Line
         * 4:T
         * 5:Cross
         * 0x21222112
        */
        MerchantRemainingCount = 42,
        MerchantRemainingMinute = 43,
        MerchantDiscount = 44,
        SellableRankRequirement = 45,
        HPBoost = 46,
        MPBoost = 47,
        AttackBonus = 48,
        DefenseBonus = 49,
        SpeedBonus = 50,
        VitalityBonus = 51,
        WisdomBonus = 52,
        DexterityBonus = 53,
        OwnerAccountId = 54,
        NameChangerStar = 55,
        NameChosen = 56,
        Fame = 57,
        FameGoal = 58,
        PlayerInDanger = 59,
        ____Unknown60 = 60,
        ____Unknown61 = 61,
        Guild = 62,
        GuildRank = 63,
        ____Unknown64 = 64,
    }
    struct TimedPosition
    {
        public int Time;
        public Position Position;
        public static TimedPosition Read(NReader rdr)
        {
            TimedPosition ret = new TimedPosition();
            ret.Time = rdr.ReadInt32();
            ret.Position = Position.Read(rdr);
            return ret;
        }
        public void Write(NWriter wtr)
        {
            wtr.Write(Time);
            Position.Write(wtr);
        }
    }
    struct Position
    {
        public float X;
        public float Y;
        public static Position Read(NReader rdr)
        {
            Position ret = new Position();
            ret.X = rdr.ReadSingle();
            ret.Y = rdr.ReadSingle();
            return ret;
        }
        public void Write(NWriter wtr)
        {
            wtr.Write(X);
            wtr.Write(Y);
        }
    }

    struct ObjectDef
    {
        public short ObjectType;
        public ObjectStats Stats;
        public static ObjectDef Read(NReader rdr)
        {
            ObjectDef ret = new ObjectDef();
            ret.ObjectType = rdr.ReadInt16();
            ret.Stats = ObjectStats.Read(rdr);
            return ret;
        }
        public void Write(NWriter wtr)
        {
            wtr.Write(ObjectType);
            Stats.Write(wtr);
        }
    }

    struct ObjectStats
    {
        public int Id;
        public Position Position;
        public KeyValuePair<StatsType, object>[] Stats;
        public static ObjectStats Read(NReader rdr)
        {
            ObjectStats ret = new ObjectStats();
            ret.Id = rdr.ReadInt32();
            ret.Position = Position.Read(rdr);
            ret.Stats = new KeyValuePair<StatsType, object>[rdr.ReadInt16()];
            for (var i = 0; i < ret.Stats.Length; i++)
            {
                StatsType type = (StatsType)rdr.ReadByte();
                if (type == StatsType.Guild || type == StatsType.Name)
                    ret.Stats[i] = new KeyValuePair<StatsType, object>(type, rdr.ReadUTF());
                else
                    ret.Stats[i] = new KeyValuePair<StatsType, object>(type, rdr.ReadInt32());
            }

            return ret;
        }
        public void Write(NWriter wtr)
        {
            wtr.Write(Id);
            Position.Write(wtr);

            wtr.Write((short)Stats.Length);
            foreach (var i in Stats)
            {
                wtr.Write((byte)i.Key);
                if (i.Value is string) wtr.WriteUTF(i.Value as string);
                else wtr.Write((int)i.Value);
            }
        }
    }
}
