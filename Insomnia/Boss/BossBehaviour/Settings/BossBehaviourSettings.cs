using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BigBossBattle {


    public class BossBehaviourSettings {
        public bool onHitBehaviour;

        public BossBehaviourSettings(bool _onHitBehaviour = false) {
            onHitBehaviour = _onHitBehaviour;
        }
    }
}
