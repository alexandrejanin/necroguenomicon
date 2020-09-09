using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GamePhase {
    public abstract string Text(GameController controller);

    public virtual void Start(GameController controller) {}

    public abstract GamePhase Update(GameController controller);
}
