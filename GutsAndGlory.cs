using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace GutsAndGlory
{
    public class Initialize
    {
        MBSubModuleBase mbsmb = new MBSubModuleBase();
            OnSubModuleLoad()
        {
             InformationManager.DisplayMessage(new InformationMessage("Guts And Glory Loaded"));
        }

        OnKill()
        {
            if(Damage > 35f)
            {
                RemoveMeshesWithTag(this.tag)
            }
        }
    }
}
