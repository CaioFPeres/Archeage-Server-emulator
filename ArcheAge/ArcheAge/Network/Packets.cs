﻿using ArcheAgeGame.ArcheAge.Network.Connections;
using LocalCommons.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArcheAgeGame.Properties;

namespace ArcheAgeGame.ArcheAge.Network
{
    /// <summary>
    /// Sends Request To Login Server For Authorization.
    /// </summary>
    public sealed class Net_RegisterArcheAgeGame : NetPacket
    {
        public Net_RegisterArcheAgeGame() : base(0x00, true)
        {
            Settings m_Default = Settings.Default;
            ns.Write((byte)m_Default.Game_Id);
            ns.Write((short)m_Default.ArcheAge_Port);

            ns.WriteDynamicASCII(m_Default.ArcheAge_IP);
            ns.WriteDynamicASCII(m_Default.Game_Password);
        }
    }

    /// <summary>
    /// Sends To Login Server Information About That Character Count Update For Specified Account Is Required.
    /// </summary>
    public sealed class Net_UpdateCharacterCount : NetPacket
    {
        public Net_UpdateCharacterCount(int accountId, int characters) : base(0x02, true)
        {
            ns.Write((int)accountId);
            ns.Write((int)characters);
        }
    }
}
